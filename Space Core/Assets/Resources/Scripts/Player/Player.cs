using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private PlayerMovement movement;
    private Enemy enemy;
    private bool lookingLeft;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
        Weapon = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
        movement = GetComponent<PlayerMovement>();
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
        float angle = Mathf.Atan2(mouseOnScreen.y - positionOnScreen.y, mouseOnScreen.x - positionOnScreen.x) * Mathf.Rad2Deg;
        
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
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.IsSelected = true;
                enemy.HackArea.SetActive(true);
            }
        }
    }

    public void Switch()
    {
        enemy.HackArea.SetActive(false);
        enemy.gameObject.AddComponent<Player>();
        enemy.gameObject.tag = "Player";
        enemy.enabled = false;
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
