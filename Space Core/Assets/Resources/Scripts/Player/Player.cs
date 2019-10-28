using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Character Enemy { get; set; }
    private PlayerMovement movement;
    private bool lookingLeft;
    private bool canSwap;
    private GameObject hackProj;

    private const float COOLDOWN_TIME = 2.5f;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();
    }

    private void Awake()
    {
        MaxHealth = 100000;
        Health = MaxHealth;
        canSwap = true; 

        RotationPoint = transform.Find("RotationPoint").gameObject;
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
        HUDController.instance.UpdateAmmo(Weapon);
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
            Destroy(gameObject);
        }
        else
        {
            Health -= damage;
            HUDController.instance.UpdateHealth(MaxHealth, Health);
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
            GameObject hackAttempt = Instantiate(hackProj, Weapon.FireLocation.transform.position, Quaternion.identity);
            hackAttempt.transform.forward = Weapon.FireLocation.transform.right;
            canSwap = false;
            StartCoroutine(implementSwapCooldown());
        }
    }

    public void Switch()
    {
        Destroy(RotationPoint);
        if (Enemy.GetComponent<SmallEnemy>() != null)
        {
            SmallEnemy se = Enemy.GetComponent<SmallEnemy>();
            MaxHealth = se.MaxHealth;
            Health = se.Health;
            Destroy(se.HackArea.gameObject);
            Destroy(se.QTEPanel.gameObject);
        }
        else if (Enemy.GetComponent<MediumEnemy>() != null)
        {
            MediumEnemy me = Enemy.GetComponent<MediumEnemy>();
            MaxHealth = me.MaxHealth;
            Health = me.Health;
            Destroy(me.HackArea.gameObject);
            Destroy(me.QTEPanel.gameObject);
        }
        Camera.main.transform.parent = Enemy.transform;
        Enemy.gameObject.AddComponent<Player>();
        Enemy.tag = "Player";
        Enemy.name = "Player";
        Destroy(Enemy);
        Enemy = null;
        Destroy(gameObject);
        HUDController.instance.UpdateHealth(MaxHealth, Health);
            HUDController.instance.UpdateDiagnosticPanels();
    }

    /// <summary>
    /// Waits for COOLDOWN_TIME amount of time and then reset ability to hack
    /// </summary>
    /// <returns></returns>
    private IEnumerator implementSwapCooldown()
    {
        float timer = 0;
        HUDController.instance.UpdateSwap(timer, COOLDOWN_TIME);
        while (timer < COOLDOWN_TIME)
        {
            timer += .1f;
            yield return new WaitForSeconds(.1f);
            HUDController.instance.UpdateSwap(timer, COOLDOWN_TIME);
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
