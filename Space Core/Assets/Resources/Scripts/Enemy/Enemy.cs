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
        throw new System.NotImplementedException();
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(float axis)
    {
        throw new System.NotImplementedException();
    }

    public override void AimWeapon(float angle)
    {
        throw new System.NotImplementedException();
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
}
