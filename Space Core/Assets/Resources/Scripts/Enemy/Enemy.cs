using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool IsSelected { get; set; }
    public GameObject HackArea { get; private set; }
    private Transform QTEPointLeft;
    private Transform QTEPointRight;
    public GameObject QTEPanel { get; private set; }
    private bool lookingLeft = false;
    private bool moveLeft = false;
    public float Speed { get; private set; }
    public Transform[] MovePoints;

    private void Awake()
    {
        MaxHealth = 85;
        Health = MaxHealth;
        RotationPoint = transform.Find("RotationPoint").gameObject;
        Weapon = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
        QTEPointLeft = transform.Find("QTEPointLeft");
        QTEPointRight = transform.Find("QTEPointRight");
        HackArea = transform.Find("HackArea").gameObject;
        QTEPanel = transform.Find("QTE_Canvas").gameObject;
        QTEPanel.SetActive(false);
    }

    protected new void Start()
    {
        base.Start();
        StartCoroutine(PositionCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        if (IsSelected)
        {
            QTEPanel.SetActive(true);
            UpdateQTEManagerPosition();
        }

        if (IsPlayerInRange())
        {
            Debug.Log("In Range");
            Vector3 playerPosition = Player.instance.transform.position;
            Vector3 myPosition = transform.position;
            Vector3 diff = playerPosition - myPosition;
            AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            Fire();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
            Destroy(other.gameObject);
        }
        else if(other.tag == "HackProjectile")
        {
            IsSelected = true;
            HackArea.SetActive(true);
            UpdateQTEManagerPosition();
        }
        if (IsSelected)
        {
            QTEPanel.SetActive(true);
            UpdateQTEManagerPosition();
        }
    }

    private void UpdateQTEManagerPosition()
    {
        Vector2 pos = Player.instance.transform.position;
        float distToLeft = Vector2.Distance(pos, QTEPointLeft.position);
        float distToRight = Vector2.Distance(pos, QTEPointRight.position);
        if (distToLeft < distToRight)
        {
            QTEPanel.transform.position = QTEPointRight.position;
        }
        else
        {
            QTEPanel.transform.position = QTEPointLeft.position;
        }
    }

    public override void Fire()
    {
        Weapon.Fire();
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(float speed)
    {
        if(moveLeft)
        {
            transform.position -= new Vector3(speed, 0, 0);
        }
        else
        {
            transform.position += new Vector3(speed, 0, 0);
        }
    }

    public override void AimWeapon(float angle)
    {
        bool pointLeft = Mathf.Abs(angle) > 90;
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
        if (lookingLeft)
        {
            angle *= -1;
        }

        RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public override void Jump()
    {
        throw new System.NotImplementedException();
    }

    public override void JumpCancel()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public bool IsPlayerInRange()
    {
        Vector3 playerPos = Player.instance.transform.position;
        return (Vector3.Distance(playerPos, transform.position) < 5);
    }

    private IEnumerator PositionCheck()
    {
        while(true)
        {
            Move(Speed*Time.deltaTime);
            foreach(Transform MovePoint in MovePoints)
            {
                if (transform.position.x == MovePoint.position.x)
                {
                    moveLeft = !(moveLeft);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
