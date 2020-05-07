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
    public AIController Enemy;
    public InteractableEnv InteractableObject { private get; set; }

    //Variables for wheel upgrades
    public bool ForceCompensatorActive { get; set; }
    public bool HasSwitched { get; set; }
    public bool HealthRegenOnPickup { get; set; }
    public bool HealOnHack { get; set; }
    public bool ScorchedEarthActive { get; set; }
    public bool IYBKYDActive { get; set; }
    public bool TacticalActive { get; set; }
    public bool ReverseEngineeringProtocolActive { get; set; }
    public bool RateOfFireOptimizerActive { get; set; }
    public bool StealthHackActive { get; set; }
    public bool Undetectable { get; set; }
    public int Shield { get; set; }
    public int ShieldMax { get; set; }

    //constants
    private const float HACK_AREA_LENGTH = 22.5f;
    private const float COOLDOWN_TIME = 2.5f;
    private const float SHIELD_RECHARGE_TIME = 2.5f;
    private const int SNAP_RANGE = 10;
    private float IFRAME_TIME = 3f;
    private bool canSwap;
    private bool routineRunning;

    //setup singleton of the Player Controller
    private static PlayerController _instance;
    public static PlayerController instance { get { return _instance; } }

    /// <summary>
    /// Awake function to setup the singleton
    /// </summary>
    private void Awake()
    {
        routineRunning = false;
        HealthRegenOnPickup = false;
        HasSwitched = false;
        Shield = 0;
        ShieldMax = 0;

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
            Character.MaxHealth = 150;
            Character.Health = 150;
            Character.Class = "medium";
            Character.IsPlayer = true;

            // Enable player canvas on the new character, and update the Player Canvas controller to point to the new canvas.
            Character.WorldspaceCanvas.gameObject.SetActive(true);
            Character.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().InitializeAsPlayerCanvas(Character);
        }
        canSwap = true;
        HUDController.instance.UpdateHUD(Character);
        base.Start();
        Character.SetMeshEmissionColor(Color.red);
    }

    /// <summary>
    /// Method for deselecting the player's hack target
    /// </summary>
    public override void DeselectHackTarget()
    {
        Enemy.DeselectHackTarget();
        if (IYBKYDActive)
        {
            Enemy.Character.ReceiveAttack(new AttackInfo(IYBKYD.DamageOnCancel, Vector2.zero, 0, 0, DamageSource.Player));
        }
        Enemy = null;
    }

    /// <summary>
    /// When player dies, go back to the main menu
    /// </summary>
    public override void Die()
    {
        InputManager.instance.currentState = InputManager.InputState.GAME_OVER;
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Switch function used for swapping control from enemy controller to player controller
    /// </summary>
    public void Switch()
    {
        if(Enemy != null)
        {
            InputManager.instance.currentState = InputManager.InputState.LOADING;
            Enemy.Character.HackArea.gameObject.SetActive(false);
            Enemy.Character.QTEPanel.gameObject.SetActive(false);
            Enemy.Character.movement.runMax = Enemy.BaseRunMax;
            // Disable player canvas on the old character.
            Character.WorldspaceCanvas.gameObject.SetActive(false);

            Character.SetMeshEmissionColor(Color.blue);

            Character.IsPlayer = false;
            Character.Weapon.BulletSource = DamageSource.Enemy;
            Character.Weapon.ControlledByPlayer = false;
            Character.tag = "Enemy";
            Character.name = "Enemy";
            Character.IsBlinking = false;
            Character.Invincible = 0;
            SwapCharacter(Enemy.Character, ref Enemy);

            if(ScorchedEarthActive)
            {
                Enemy.Character.ReceiveAttack(new AttackInfo(ScorchedEarth.DamageOnSwitch, Vector2.zero, 0, 0, DamageSource.Player));
            }

            if(TacticalActive)
            {
                Tactical.ResetAbility();
            }

            if (RateOfFireOptimizerActive)
            {
                RateOfFireOptimizer.ResetAbility();
            }

            // Enable player canvas on the new character, and update the Player Canvas controller to point to the new canvas.
            Character.WorldspaceCanvas.gameObject.SetActive(true);
            Character.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().InitializeAsPlayerCanvas(Character);

            Character.IsPlayer = true;
            Character.Weapon.BulletSource = DamageSource.Player;
            Character.Weapon.ControlledByPlayer = true;
            Character.tag = "Player";
            Character.name = "Player";
            Character.IsBlinking = false;
            Character.Invincible = 0;
            Enemy = null;

            if (TacticalActive)
            {
                Tactical.ResetAbility();
            }

            if (RateOfFireOptimizerActive)
            {
                RateOfFireOptimizer.ApplyAbility();
            }

            if (StealthHackActive)
            {
                if (routineRunning)
                {
                    StopCoroutine(ResetPlayerTag());
                    routineRunning = false;
                }
                StartCoroutine(ResetPlayerTag());
            }

            HUDController.instance.UpdateHUD(Character);
            HasSwitched = true;
        }
    }

    /// <summary>
    /// Delegate that reacts to the player receiving an attack
    /// </summary>
    /// <param name="attackInfo">Information on the attack the player is taking</param>
    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        if (Shield > 0)
        {
            Shield--;
            StartCoroutine(RechargeShield());
            return;
        }

        if (Character.Invincible > 0)
            return;

        if (ReverseEngineeringProtocolActive)
        {
            float ammoGained = attackInfo.damage * ReverseEngineeringProtocol.AmmoRetentionMod;
            if (ammoGained < 0)
            {
                ammoGained = 1;
            }
            Character.Weapon.AddAmmo((int) ammoGained);
            HUDController.instance.UpdateAmmo(Character);
        }

        base.ReceiveAttack(attackInfo);

        if (attackInfo.damageSource != DamageSource.Hazard)
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
            if (!HealthRegenOnPickup)
            {
                Character.Weapon.AddAmmo(collision.gameObject.GetComponent<DroppedAmmo>().Ammo);
            }
            else
            {
                float ammo = collision.gameObject.GetComponent<DroppedAmmo>().Ammo;
                float health = ammo * .2f;
                if (health < 1)
                {
                    health = 1;
                }
                ammo = ammo * .7f;
                if (ammo < 1)
                {
                    ammo = 1;
                }
                Character.Weapon.AddAmmo((int)ammo);
                Character.HealCharacter((int)health);
                HUDController.instance.UpdateHUD(Character);
            }

            // Update the ammo text above player - using ONLY this Enemy's canvas.
            Character.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().UpdatePlayerAmmo();
            
            HUDController.instance.UpdateAmmo(Character);
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Rocket"))
        {
            //Damage applied from Rockets. Can damage self.

            //collision.gameObject.GetComponent<Rocket>().attInfo.knockbackImpulse 
                //= collision.gameObject.GetComponent<Rocket>().KnockbackImpulse * (transform.position - collision.transform.position).normalized;
            ReceiveAttack(collision.gameObject.GetComponentInChildren<ExplosionInformation>().Info);
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
            if (RateOfFireOptimizerActive)
            {
                RateOfFireOptimizer.ResetAbility();
                RateOfFireOptimizer.ApplyAbility();
            }

            Character.Reload();

            //update hud
            HUDController.instance.UpdateAmmo(Character);
        }
    }

    public void AimWeapon(float angle, bool track)
    {
        if (!track)
        {
            Character.AimWeapon(angle, Camera.main);
        }
        else
        {
            AimWeapon(angle);
        }
    }

    /// <summary>
    /// Method for passing the main camera to the character when the player aim's the weapon
    /// </summary>
    /// <param name="angle">New angle the player will be aiming at</param>
    public override void AimWeapon(float angle)
    {
        Vector2 direction = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
        bool previous = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, direction, Character.Weapon.Range);
        //check if we hit an enemy
        if (hit.collider == null || hit.collider.tag != "Enemy")
        {
            //check if we hit an enemy above the range
            for (int i = 0; i < SNAP_RANGE && (hit.collider == null || hit.collider.tag != "Enemy"); i++)
            {
                direction = (Vector2)(Quaternion.Euler(0, 0, angle + i) * Vector2.right);
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, direction, Character.Weapon.Range);
            }
            //if there is still no enemy hit, check below the current angle
            if (hit.collider == null || hit.collider.tag != "Enemy")
            {
                for (int i = 0; i < SNAP_RANGE && (hit.collider == null || hit.collider.tag != "Enemy"); i++)
                {
                    direction = (Vector2)(Quaternion.Euler(0, 0, angle - i) * Vector2.right);
                    hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, direction, Character.Weapon.Range);
                }
            }
        }
        Physics2D.queriesHitTriggers = previous;
        if (hit && hit.collider && hit.collider.tag == "Enemy")
        {
            Vector3 diff = hit.collider.gameObject.transform.position - Character.transform.position;
            angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        }
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

    public override bool Fire()
    {
        if (Character.isStunned > 0)
        {
            return false;
        }

        bool fired = base.Fire();
        if (fired && RateOfFireOptimizerActive)
        {
            RateOfFireOptimizer.ApplyMod();
        }
        else if(!fired && RateOfFireOptimizerActive)
        {
            RateOfFireOptimizer.ResetAbility();
            RateOfFireOptimizer.ApplyAbility();
        }
        return fired;
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

    protected override void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;

        base.Update();

        switch (Character.Class)
        {
            case "heavy":
                CheckMaxSpeed();
                break;
            case "medium":
                CheckRolling();
                break;
        }
        
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
    public bool isFacingLeft
    {
        get
        {
            Vector2 charScale = Character.transform.localScale;
            return charScale.x < 0;
        }
    }

    private IEnumerator RechargeShield()
    {
        if(ShieldMax != 0)
        {
            yield return new WaitForSeconds(SHIELD_RECHARGE_TIME);
            if (ShieldMax != 0)
                Shield++;
        }
    }

    private IEnumerator ResetPlayerTag()
    {
        routineRunning = true;
        Undetectable = true;
        yield return new WaitForSeconds(UndetectableHack.UndetectableTime);
        Undetectable = false;
        routineRunning = false;
    }
}
