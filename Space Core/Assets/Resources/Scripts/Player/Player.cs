using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private PlayerMovement movement;
    public Enemy enemy;
    private bool lookingLeft;
    private GameObject hackProj;
    private float angle;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();
    }

    private void Awake()
    {
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
    }

    public override void AimWeapon()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(RotationPoint.transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        angle = Mathf.Atan2(mouseOnScreen.y - positionOnScreen.y, mouseOnScreen.x - positionOnScreen.x) * Mathf.Rad2Deg;
        
        if (lookingLeft && Mathf.Abs(angle) < 90)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            newScale = RotationPoint.transform.localScale;
            newScale.x *= -1;
            newScale.y *= -1;
            RotationPoint.transform.localScale = newScale;
            lookingLeft = false;
        }
        else if (!lookingLeft && Mathf.Abs(angle) > 90)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            newScale = RotationPoint.transform.localScale;
            newScale.x *= -1;
            newScale.y *= -1;
            RotationPoint.transform.localScale = newScale;
            lookingLeft = true;
        }

        if(lookingLeft)
        {
            angle *= -1;
        }

        RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void HackSelector()
    {
        GameObject hackAttempt = Instantiate(hackProj, transform.position, Quaternion.identity);
        hackAttempt.transform.forward = new Vector3(angle, 0, 0);
    }

    public void Switch()
    {
        Destroy(RotationPoint);
        enemy.gameObject.AddComponent<Player>();
        enemy.gameObject.tag = "Player";
        enemy.gameObject.name = "Player";
        Destroy(enemy.HackArea.gameObject);
        Destroy(enemy.QTEPanel.gameObject);
        Destroy(enemy);
        enemy = null;
        Destroy(gameObject);
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
