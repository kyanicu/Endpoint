﻿using System.Collections;
using UnityEngine;

//We'll need to figure out a way to decouple scene loading from player
using UnityEngine.SceneManagement;


public class Player : Character
{
    public Enemy Enemy { get; set; }
    private bool lookingLeft;
    private bool canSwap;
    private GameObject hackProj;

    const float HACK_AREA_LENGTH = 22.5f;
    public string Class { get; private set; }

    private const float COOLDOWN_TIME = 2.5f; 

    [SerializeField]
    private float iFrameTime = 2;

    private bool hasIFrames;

    /// <summary>
    /// Update HUD after successfully swapping into a new enemy
    /// Start called on new Player component enabled
    /// </summary>
    protected override void Start()
    {
        base.Start();
        HUDController.instance.UpdateHUD(this);
        Weapon.ControlledByPlayer = true;
    }

    protected override void Reset()
    {
        base.Reset();
    }

    protected override void Awake()
    {
        base.Awake();

        MaxHealth = 100;
        Health = MaxHealth;
        canSwap = true;
        isImmortal = false;
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

        ActiveAbility = gameObject.GetComponent<ActiveAbility>();
        PassiveAbility = gameObject.GetComponent<PassiveAbility>();

        if (ActiveAbility == null && PassiveAbility == null)
        {
            AbilityGenerator.AddAbilitiesToCharacter(gameObject);
        }
        else
        {
            
            ActiveAbility.resetOwner(this);
            PassiveAbility.resetOwner(this);
        }

        Weapon.BulletSource = Bullet.BulletSource.Player;
        hackProj = Resources.Load<GameObject>("Prefabs/Hacking/HackProjectile");
        HUDController.instance.UpdateHUD(this);
    }
    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        if (isImmortal || hasIFrames)
            return;

        base.ReceiveAttack(attackInfo);
        StartCoroutine(RunIFrames());

    }
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (!isImmortal && Health - damage > 0)
        {
            HUDController.instance.UpdatePlayer(this);
        }
    }

    private IEnumerator RunIFrames()
    {
        hasIFrames = true;
        yield return new WaitForSeconds(iFrameTime);
        hasIFrames = false;
    }

    protected override void Die()
    {
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;
        SceneManager.LoadScene(0);
    }

    public override void Fire()
    {
        //reload if out of ammo
        if (Weapon.AmmoInClip <= 0 && !Weapon.IsReloading)
        {
            Reload();
        }
        else
        {
            Weapon.Fire();
        }
        HUDController.instance.UpdateAmmo(this);
    }

    public override void Reload()
    {
        //If player is not in hack circle, reload
        if (Enemy == null || Vector3.Distance(transform.position, Enemy.transform.position) > HACK_AREA_LENGTH)
        {
            Weapon.Reload(this);

            //update hud
            HUDController.instance.UpdateAmmo(this);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == Bullet.BulletSource.Enemy)
            {
                ReceiveAttack(new AttackInfo(other.gameObject.GetComponent<Bullet>().Damage/5, other.gameObject.GetComponent<Bullet>().KnockbackImpulse * other.gameObject.transform.right, other.gameObject.GetComponent<Bullet>().StunTime));
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

    /// <summary>
    /// Swaps the player into a new body after a successful hack
    /// </summary>
    public void Switch()
    {
        //Remove attached game objects and components
        Destroy(RotationPoint);
        Destroy(ActiveAbility);
        Destroy(PassiveAbility);
        Destroy(Enemy.HackArea.gameObject);
        Destroy(Enemy.QTEPanel.gameObject);

        //Update player's stats to enemy's 
        MaxHealth = Enemy.MaxHealth;
        Health = Enemy.Health;
        Class = Enemy.Class;
        movement = Enemy.movement;

        //Change the enemy's minimap icon to a player's and remove the enemy's
        MinimapIcon.transform.position = Enemy.MinimapIcon.transform.position;
        MinimapIcon.transform.SetParent(Enemy.transform);
        Destroy(Enemy.MinimapIcon);

        //Update rigidbody
        Rigidbody2D rigidBody = Enemy.gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        rigidBody.simulated = true;

        //Update HUD with new weapon
        HUDController.instance.UpdateAmmo(this);

        //Rename enemy to player
        Enemy.tag = "Player";
        Enemy.name = "Player";
        GameObject enemyObject = Enemy.gameObject;

        //Remove enemy component from new body
        Destroy(Enemy);
        Enemy = null;
        enemyObject.gameObject.AddComponent<Player>();
        Destroy(gameObject);
    }

    /// <summary>
    /// Waits for COOLDOWN_TIME amount of time and then reset ability to hack
    /// </summary>
    /// <returns></returns>
    private IEnumerator implementSwapCooldown()
    {
        yield return new WaitForSeconds(COOLDOWN_TIME);
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
