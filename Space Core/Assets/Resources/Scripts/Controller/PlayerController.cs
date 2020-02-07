using System.Collections;
using UnityEngine;

//We'll need to figure out a way to decouple scene loading from player
using UnityEngine.SceneManagement;

/// <summary>
/// Controller that is designed to guide the behavior of the character the player is currently in control of
/// </summary>
public class PlayerController : Controller
{

    //Base variables for the player controller
    public AIController Enemy { get; set; }
    public InteractableEnv InteractableObject { private get; set; }
    const float HACK_AREA_LENGTH = 22.5f;
    private const float COOLDOWN_TIME = 2.5f;
    private float IFRAME_TIME = 3f;
    private bool canSwap;

    //setup singleton of the Player Controller
    private static PlayerController _instance;
    public static PlayerController instance { get { return _instance; } }

    /// <summary>
    /// Awake function to setup the singleton
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Start function for initializing delegates and base character behavior
    /// </summary>
    public override void Start()
    {
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        Character.Weapon.ControlledByPlayer = true;
        Character.Weapon.BulletSource = DamageSource.Player;
        if (Character.Class == null)
        {
            Character.MaxHealth = 200;
            Character.Health = 200;
            Character.Class = "medium";
            Character.IsPlayer = true;

            // Enable player canvas on the new character, and update the Player Canvas controller to point to the new canvas.
            Character.WorldspaceCanvas.gameObject.SetActive(true);
            WorldspaceCanvas.instance.UpdateWorldspaceCanvas(Character.WorldspaceCanvas);
        }
        canSwap = true;
        HUDController.instance.UpdateHUD(Character);
        base.Start();
    }

    /// <summary>
    /// Method for deselecting the player's hack target
    /// </summary>
    public override void DeselectHackTarget()
    {
        Enemy.DeselectHackTarget();
        Enemy = null;
    }

    /// <summary>
    /// When player dies, go back to the main menu
    /// </summary>
    public override void Die()
    {
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Switch function used for swapping control from enemy controller to player controller
    /// </summary>
    public void Switch()
    {
        if(Enemy != null)
        {
            Enemy.Character.HackArea.gameObject.SetActive(false);
            Enemy.Character.QTEPanel.gameObject.SetActive(false);
            Enemy.Character.movement.runMax = Enemy.BaseRunMax;
            // Disable player canvas on the old character.
            Character.WorldspaceCanvas.gameObject.SetActive(false);

            Character.IsPlayer = false;
            Character.Weapon.BulletSource = DamageSource.Enemy;
            Character.Weapon.ControlledByPlayer = false;
            Character.tag = "Enemy";
            Character.name = "Enemy";
            Character.IsBlinking = false;
            Character.Invincible = 0;
            SwapCharacter(Enemy.Character, Enemy);

            // Enable player canvas on the new character, and update the Player Canvas controller to point to the new canvas.
            Character.WorldspaceCanvas.gameObject.SetActive(true);
            WorldspaceCanvas.instance.UpdateWorldspaceCanvas(Character.WorldspaceCanvas);

            Character.IsPlayer = true;
            Character.Weapon.BulletSource = DamageSource.Player;
            Character.Weapon.ControlledByPlayer = true;
            Character.tag = "Player";
            Character.name = "Player";
            Character.IsBlinking = false;
            Character.Invincible = 0;
            Enemy = null;
            HUDController.instance.UpdateHUD(Character);
        }
    }

    /// <summary>
    /// Delegate that reacts to the player receiving an attack
    /// </summary>
    /// <param name="attackInfo">Information on the attack the player is taking</param>
    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        if (Character.Invincible > 0)
            return;

        base.ReceiveAttack(attackInfo);

        if (attackInfo.damageSource != DamageSource.Spread && attackInfo.damageSource != DamageSource.Hazard)
        {
            Character.Invincible++;
            StartCoroutine(RunIFrames());
        }
    }

    /// <summary>
    /// Active environment obj if player is near one
    /// </summary>
    public void ActivateEnvironmentObj()
    {
        if (InteractableObject != null)
        {
            InteractableObject.ActivateFunctionality();
        }
    }

    /// <summary>
    /// Delegate that reacts to when the player enters a trigger
    /// </summary>
    /// <param name="collision">Collider information on the trigger</param>
    public override void TriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ammo"))
        {
            Character.Weapon.AddAmmo(collision.gameObject.GetComponent<DroppedAmmo>().Ammo);

            // Update the ammo text above player - using ONLY this Enemy's canvas.
            WorldspaceCanvas.instance.UpdatePlayerAmmo();
            
            HUDController.instance.UpdateAmmo(Character);
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Method for applying damage to the player
    /// </summary>
    /// <param name="damage">the ammount of damage the player istaking</param>
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (Character.Invincible == 0 && Character.Health - damage > 0)
        {
            HUDController.instance.UpdatePlayer(Character);
        }
    }

    /// <summary>
    /// Method that exposes the reload function in the character the player is controlling.
    /// </summary>
    public override void Reload()
    {
        //If player is not in hack circle, reload
        if (Enemy == null || Vector3.Distance(transform.position, Enemy.transform.position) > HACK_AREA_LENGTH)
        {
            Character.Reload();

            //update hud
            HUDController.instance.UpdateAmmo(Character);
        }
    }

    /// <summary>
    /// Method for passing the main camera to the character when the player aim's the weapon
    /// </summary>
    /// <param name="angle">New angle the player will be aiming at</param>
    public override void AimWeapon(float angle)
    {
        Character.AimWeapon(angle, Camera.main);
    }

    /// <summary>
    /// Function responsible for instantiating hack projectile
    /// </summary>
    public void HackSelector()
    {
        if (Enemy == null)
        {
            if (canSwap)
            {
                Vector3 launchRotation = Character.RotationPoint.transform.rotation.eulerAngles;
                GameObject hackAttempt = ObjectPooler.instance.SpawnFromPool("HackProj", Character.Weapon.FireLocation.transform.position, Quaternion.identity);
                hackAttempt.transform.Rotate(launchRotation);
                ResetSwap();
            }
        }
        else
        {
            DeselectHackTarget();
        }
    }

    /// <summary>
    /// function for resetting the swap
    /// </summary>
    private void ResetSwap()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;
        canSwap = false;
        // Disables Swapping for the duration specified in COOLDOWN_TIME.
        StartCoroutine(implementSwapCooldown());
        // Begins HUD animation loop for swapping bar.
        HUDController.instance.UpdateSwap(COOLDOWN_TIME);
    }

    /// <summary>
    /// Waits for COOLDOWN_TIME amount of time and then reset ability to hack
    /// </summary>
    /// <returns></returns>
    private IEnumerator implementSwapCooldown()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            yield return null;

        float counter = 0;
        while (counter < COOLDOWN_TIME)
        {
            //Check that player is not in a menu
            if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            {
                yield return null;
            }
            else
            {
                counter += .1f;
                yield return new WaitForSeconds(.1f);
            }
        }
        canSwap = true;
    }

    /// <summary>
    /// Coroutine for resetting the player's Invincibility once their Iframes are up
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunIFrames()
    {
        yield return new WaitForSeconds(IFRAME_TIME);
        Character.Invincible--;
    }

    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;

        UpdateWorldspaceCanvasDirection();
    }

    protected void UpdateWorldspaceCanvasDirection()
    {
        // If character is facing to the left, x local scale is negative
        // So that the character's UI is always facing the right direction.
        if (isFacingLeft)
        {
            Character.WorldspaceCanvas.transform.localScale = new Vector3(-1, 1, 1);
        }
        //Else reset scale and positions
        else
        {
            Character.WorldspaceCanvas.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Returns whether or not the character is facing left
    //Used in UpdatePlayerCanvasDirection()
    protected bool isFacingLeft
    {
        get
        {
            Vector2 charScale = Character.transform.localScale;
            return charScale.x < 0;
        }
    }
}
