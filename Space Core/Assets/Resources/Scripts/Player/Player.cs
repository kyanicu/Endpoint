using System.Collections;
using UnityEngine;

//We'll need to figure out a way to decouple scene loading from player
using UnityEngine.SceneManagement;


public class Player : Character
{
    public Enemy Enemy { get; set; }
    private PlayerMovement movement;
    private bool lookingLeft;
    private bool canSwap;
    private GameObject hackProj;
    Vector2 newPos;

    const float HACK_AREA_LENGTH = 22.5f;
    public string Class { get; private set; }

    private const float COOLDOWN_TIME = 2.5f;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();
    }

    /// <summary>
    /// Update HUD after successfully swapping into a new enemy
    /// Start called on new Player component enabled
    /// </summary>
    private new void Start()
    {
        base.Start();
        HUDController.instance.UpdateHUD(this);
        Weapon.ControlledByPlayer = true;

    }

    private void Awake()
    {
        MaxHealth = 100;
        Health = MaxHealth;
        canSwap = true;
        if (Class == null)
        {
            Class = "medium";
        }
        ResetSwap();


        RotationPoint = transform.Find("RotationPoint").gameObject;

        
        RotationPoint.transform.localScale = new Vector3(1, 1, 1);
        
        transform.localScale = new Vector3(1, 1, 1);

        Transform WeaponTransform = RotationPoint.transform.Find("WeaponLocation");
        if (WeaponTransform.childCount == 0)
        {
            Weapon = WeaponGenerator.GenerateWeapon(WeaponTransform).GetComponent<Weapon>();
        }
        else
        {
            Weapon = WeaponTransform.GetChild(0).GetComponent<Weapon>();
        }
        Weapon.BulletSource = Bullet.BulletSource.Player;
        movement = GetComponent<PlayerMovement>();
        hackProj = Resources.Load<GameObject>("Prefabs/Hacking/HackProjectile");
        HUDController.instance.UpdateHUD(this);
    }

    public override void Jump()
    {
        movement.Jump();
    }

    public override void JumpCancel()
    {
        movement.JumpCancel();
    }

    public override void Fire()
    {
        Weapon.Fire();
        HUDController.instance.UpdateAmmo(this);
    }

    public override void Reload()
    {
        //If player is not in hack circle, reload
        if (Enemy == null || Vector3.Distance(transform.position, Enemy.transform.position) > HACK_AREA_LENGTH)
        {
            Weapon.Reload();

            //update hud
            HUDController.instance.UpdateAmmo(this);
        }
    }

    public override void Move(float axis)
    {
        movement.Run(axis);
    }

    public override void TakeDamage(int damage)
    {

        if (Health - damage <= 0)
        {
            //We'll need to figure out a way to decouple scene loading from player
            SceneManager.LoadScene(0);
        }
        else
        {
            Health -= damage / 5;
            HUDController.instance.UpdatePlayer(this);
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == Bullet.BulletSource.Enemy)
            {
                TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Ammo"))
        {
            Weapon.AddAmmo(other.gameObject.GetComponent<DroppedAmmo>().Ammo);
            HUDController.instance.UpdateAmmo(this);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("OB"))
        {
            //We'll need to figure out a way to decouple scene loading from player
            SceneManager.LoadScene(0);
        }
    }

    public override void AimWeapon(float angle)
    {
        bool pointLeft = Mathf.Abs(angle) > 90;

        // if the weapon points in the direction that the character is not facing
        if (pointLeft ^ lookingLeft)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            if (Camera.main.transform.localScale.x != 1)
            {
                Camera.main.transform.localScale = newScale;
            }
            newScale = RotationPoint.transform.localScale;
            newScale.x *= -1;
            newScale.y *= -1;
            RotationPoint.transform.localScale = newScale;
            lookingLeft = !lookingLeft;
        }

        if(lookingLeft)
        {
            angle *= -1;
        }

        RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void HackSelector()
    {
        if (canSwap)
        {
            Vector3 launchRotation = RotationPoint.transform.rotation.eulerAngles;
            GameObject hackAttempt = Instantiate(hackProj, Weapon.FireLocation.transform.position, Quaternion.identity);
            hackAttempt.transform.Rotate(launchRotation);
            ResetSwap();
        }
    }

    private void ResetSwap()
    {
        canSwap = false;
        // Disables Swapping for the duration specified in COOLDOWN_TIME.
        StartCoroutine(implementSwapCooldown());
        // Begins HUD animation loop for swapping bar.
        HUDController.instance.UpdateSwap(COOLDOWN_TIME);
    }

    public void Switch()
    {
        Destroy(RotationPoint);
        MaxHealth = Enemy.MaxHealth;
        Health = Enemy.Health;
        Destroy(Enemy.HackArea.gameObject);
        Destroy(Enemy.QTEPanel.gameObject);
        Class = Enemy.Class;

        Rigidbody2D rigidBody = Enemy.gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        rigidBody.simulated = true;
        Enemy.gameObject.AddComponent<ObjectMover>();
        Enemy.gameObject.AddComponent<CharacterController2D>();
        Enemy.gameObject.AddComponent<PlayerMovement>();
        Enemy.gameObject.AddComponent<Player>();
        Camera.main.transform.parent = Enemy.transform;
        Reload();
        HUDController.instance.UpdateAmmo(this);
        Enemy.tag = "Player";
        Enemy.name = "Player";
        Destroy(Enemy);
        Enemy = null;
        Destroy(gameObject);
    }

    /// <summary>
    /// Waits for COOLDOWN_TIME amount of time and then reset ability to hack
    /// </summary>
    /// <returns></returns>
    private IEnumerator implementSwapCooldown()
    {
        float timer = 0;
        while (timer < COOLDOWN_TIME)
        {
            timer += .05f;
            yield return new WaitForSeconds(.05f);
        }
        canSwap = true;
    }

    private static Player _instance = null;

    public static Player instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(Player).Name).AddComponent<Player>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }
}
