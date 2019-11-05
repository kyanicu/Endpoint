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
    private Vector2 startPos;
    public string Class { get; private set; }

    private const float COOLDOWN_TIME = 2.5f;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();
    }

    private void Awake()
    {
        startPos = transform.position;
        MaxHealth = 100;
        Health = MaxHealth;
        canSwap = true;
        startPos = transform.position;
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
            HUDController.instance.UpdateDiagnosticPanels();
        }
        else
        {
            Weapon = WeaponTransform.GetChild(0).GetComponent<Weapon>();
        }
        Weapon.BulletSource = Bullet.BulletSource.Player;
        movement = GetComponent<PlayerMovement>();
        hackProj = Resources.Load<GameObject>("Prefabs/Hacking/HackProjectile");

        HUDController.instance.UpdateHealth(MaxHealth, Health);
        HUDController.instance.UpdateWeapon(Weapon);
        HUDController.instance.updateCharacterClass();
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
        HUDController.instance.UpdateAmmo(Weapon);
    }

    public override void Reload()
    {
        StartCoroutine(Weapon.Reload());
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
            HUDController.instance.UpdateHealth(MaxHealth, Health);
        }
        HUDController.instance.UpdateHealth(MaxHealth, Health);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == Bullet.BulletSource.Enemy)
            {
                TakeDamage(other.gameObject.GetComponent<Bullet>().Damage/5);
                Destroy(other.gameObject);
            }
        }
        else if(other.CompareTag("OB"))
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
        HUDController.instance.RechargeSwap(COOLDOWN_TIME);
    }

    public void Switch()
    {
        Destroy(RotationPoint);
        MaxHealth = Enemy.MaxHealth;
        Health = Enemy.Health;
        Destroy(Enemy.HackArea.gameObject);
        Destroy(Enemy.QTEPanel.gameObject);
        Camera cam = Camera.main;
        Class = Enemy.Class;

        cam.transform.parent = Enemy.transform;
        float camZ = cam.transform.position.z;
        cam.transform.position = Enemy.transform.position;
        cam.transform.position = new Vector3(cam.transform.position.x, 
                                             cam.transform.position.y, 
                                             camZ);

        Rigidbody2D rigidBody = Enemy.gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        rigidBody.simulated = true;
        Enemy.gameObject.AddComponent<ObjectMover>();
        Enemy.gameObject.AddComponent<CharacterController2D>();
        Enemy.gameObject.AddComponent<PlayerMovement>();
        Enemy.gameObject.AddComponent<Player>();
        Enemy.tag = "Player";
        Enemy.name = "Player";
        Destroy(Enemy);
        Enemy = null;
        Destroy(gameObject);
        HUDController.instance.UpdateHealth(MaxHealth, Health);
        HUDController.instance.UpdateWeapon(Weapon);
        HUDController.instance.UpdateDiagnosticPanels();
        HUDController.instance.updateCharacterClass();
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
