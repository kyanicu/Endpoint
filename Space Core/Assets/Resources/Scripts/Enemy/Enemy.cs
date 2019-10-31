using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool IsSelected { get; set; }
    public float PatrolRange { get; set; }
    public GameObject HackArea { get; protected set; }
    protected Transform QTEPointLeft;
    protected Transform QTEPointRight;
    public GameObject QTEPanel { get; protected set; }
    protected bool lookingLeft = false;
    protected bool moveLeft = false;
    public float Speed { get; set; }
    public GameObject[] MovePoints;

    protected void Awake()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
        Weapon = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
        Weapon.BulletSource = Bullet.BulletSource.Enemy;
        QTEPointLeft = transform.Find("QTEPointLeft");
        QTEPointRight = transform.Find("QTEPointRight");
        HackArea = transform.Find("HackArea").gameObject;
        QTEPanel = transform.Find("QTE_Canvas_Group").gameObject;
        QTEPanel.SetActive(false);

        // Instantiate left, right movement boundaries
        GameObject left = new GameObject();
        GameObject right = new GameObject();
        left.transform.position = new Vector3(transform.position.x - PatrolRange, 0, 0);
        right.transform.position = new Vector3(transform.position.x + PatrolRange, 0, 0);
        MovePoints = new GameObject[2];
        MovePoints[0] = left;
        MovePoints[1] = right;
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
            UpdateQTEManagerPosition();
            Vector3 playerPosition = Player.instance.transform.position;
            Vector3 myPosition = transform.position;
            Vector3 diff = playerPosition - myPosition;
            AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            Fire();
        }
        else
        {
            StartCoroutine(PositionCheck());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == Bullet.BulletSource.Player)
            {
                TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("HackProjectile"))
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

    protected void UpdateQTEManagerPosition()
    {
        Vector2 pos = Player.instance.transform.position;
        float distToLeft = Vector2.Distance(pos, QTEPointLeft.position);
        float distToRight = Vector2.Distance(pos, QTEPointRight.position);
        Vector3 newScale = gameObject.transform.localScale;
        newScale.x *= -1;
        if (distToLeft < distToRight)
        {
            QTEPanel.transform.position = QTEPointRight.position;
            QTEPanel.transform.localScale = newScale;
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
        Weapon.Reload();
    }

    public override void Move(float speed)
    {
        if (moveLeft)
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
        StopCoroutine(PositionCheck());
        return (Vector3.Distance(playerPos, transform.position) < 20);
    }

    protected IEnumerator PositionCheck()
    {
        while (!IsPlayerInRange())
        {
            Vector2 pos = new Vector2(transform.position.x, 0);
            float Dist0 = Vector2.Distance(pos, MovePoints[0].transform.position);
            float Dist1 = Vector2.Distance(pos, MovePoints[1].transform.position);
            if (Dist0 < .5 || Dist1 < .5)
            {
                moveLeft = !moveLeft;
            }
            Move(Speed * Time.deltaTime);
            yield return new WaitForSeconds(.01f);
        }
    }
}
