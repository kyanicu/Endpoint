using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public MediumEnemy Enemy { get; set; }
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
        MaxHealth = 100;
        Health = MaxHealth;
        canSwap = true; 

        RotationPoint = transform.Find("RotationPoint").gameObject;
        Transform WeaponTransform = RotationPoint.transform.Find("WeaponLocation");
        if (WeaponTransform.childCount == 0)
        {
            Weapon = WeaponGenerator.GenerateWeapon(WeaponTransform).GetComponent<Weapon>();
        }
        else
        {
            Weapon = WeaponTransform.GetChild(0).GetComponent<Weapon>();
        }
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
        Health -= damage;
        HUDController.instance.UpdateHealth(MaxHealth, Health);
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
        MaxHealth = Enemy.MaxHealth;
        Health = Enemy.Health;
        Enemy.gameObject.AddComponent<Player>();
        Enemy.gameObject.tag = "Player";
        Enemy.gameObject.name = "Player";
        Destroy(Enemy.HackArea.gameObject);
        Destroy(Enemy.QTEPanel.gameObject);
        Destroy(Enemy);
        Enemy = null;
        Destroy(gameObject);
        HUDController.instance.UpdateHealth(MaxHealth, Health);
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
