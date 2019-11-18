﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool IsSelected { get; set; }
    private bool disabled { get; set; }
    public float PatrolRange { get; set; }
    public GameObject HackArea { get; protected set; }
    private GameObject DropAmmo { get; set; }
    protected Transform QTEPointLeft;
    protected Transform QTEPointRight;
    public GameObject QTEPanel { get; protected set; }
    protected bool lookingLeft = false;
    protected bool moveLeft = false;
    public float Speed { get; set; }
    public GameObject[] MovePoints;
    public string Class { get; set; }

    //Returns whether or not the enemy is facing left
    //Used in UpdateQTEManagerPosition()
    protected bool isFacingLeft 
    { 
        get 
        { 
            Vector2 enemyScale = transform.localScale;
            return enemyScale.x < 0;
        } 
    }

    protected void Awake()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
        Weapon = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
        AbilityGenerator.AddAbilitiesToCharacter(gameObject);
        Weapon.BulletSource = Bullet.BulletSource.Enemy;
        DropAmmo = Resources.Load<GameObject>("Prefabs/Enemy/DroppedAmmo/DroppedAmmo");
        QTEPointLeft = transform.Find("QTEPointLeft");
        QTEPointRight = transform.Find("QTEPointRight");
        HackArea = transform.Find("HackArea").gameObject;
        QTEPanel = transform.Find("QTE_Canvas").gameObject;
        QTEPanel.SetActive(false);
        disabled = false;

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
            // Drop ammo upon death
            if (Weapon.TotalAmmo > 0)
            {
                if (Random.Range(0,10) % 2 == 0)
                {
                    GameObject instantiatedDroppedAmmo = GameObject.Instantiate(DropAmmo, transform.position, Quaternion.identity);
                    DroppedAmmo droppedAmmo = instantiatedDroppedAmmo.GetComponent<DroppedAmmo>();
                    droppedAmmo.Ammo = (Weapon.TotalAmmo < 25) ? 25 : Weapon.TotalAmmo;
                }

            }
            Destroy(gameObject);
        }

        if (IsSelected)
        {
            QTEPanel.SetActive(true);
            UpdateQTEManagerPosition();
        }

        if (!disabled)
        {
            if (IsPlayerInRange())
            {
                Vector3 playerPosition = Player.instance.transform.position;
                Vector3 myPosition = transform.position;
                Vector3 diff = playerPosition - myPosition;
                AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                Fire();
            }
            else
            {
                Vector2 pos = new Vector2(transform.position.x, 0);
                float Dist0 = Vector2.Distance(pos, MovePoints[0].transform.position);
                float Dist1 = Vector2.Distance(pos, MovePoints[1].transform.position);
                if (Dist0 < .5 || Dist1 < .5)
                {
                    moveLeft = !moveLeft;
                }
                Move(Speed);
            }
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
        //Handle piercing shot interaction
        else if (other.CompareTag("PiercingBullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == Bullet.BulletSource.Player)
            {
                TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
                other.gameObject.GetComponent<PiercingBullet>().NumPassed++;
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

    /// <summary>
    /// Updates which side of the enemy the QTE panel is relative to the player's position from the enemy
    /// </summary>
    protected void UpdateQTEManagerPosition()
    {
        //Retrieve enemy local scale
        Vector2 EnemyScale = transform.localScale;

        //If enemy is facing to the left, x local scale is negative
        //so swap panel left and right positions
        if (isFacingLeft)
        {
            QTEPanel.transform.localScale = new Vector3 (-1, 1, 1);
        }
        //Else reset scale and positions
        else
        {
            QTEPanel.transform.localScale = new Vector3(1, 1, 1);
        }

        //Grab player Position
        Vector2 pos = Player.instance.transform.position;

        //Check player distance from right and left side of enemy
        float playerDistanceToLeftSide = Vector2.Distance(pos, QTEPointLeft.position);
        float playerDistanceToRightSide = Vector2.Distance(pos, QTEPointRight.position);

        //If player is closer to left side of enemy
        if (playerDistanceToLeftSide < playerDistanceToRightSide)
        {
            QTEPanel.transform.position = QTEPointRight.position;

        }
        else //Player is closer to the right side of enemy
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
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
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
        return (Vector3.Distance(playerPos, transform.position) < 20);
    }

    public void Freeze()
    {
        StartCoroutine(FreezeTimer());
    }

    private IEnumerator FreezeTimer()
    {
        disabled = true;
        yield return new WaitForSeconds(5);
        disabled = false;
    }
}
