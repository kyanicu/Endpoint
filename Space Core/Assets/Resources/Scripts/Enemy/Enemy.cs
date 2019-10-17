using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool IsSelected { get; set; }
    public GameObject HackArea { get; private set; }
    private Transform QTEPointLeft;
    private Transform QTEPointRight;

    private void Awake()
    {
        MaxHealth = 100;
        Health = MaxHealth;
        WeaponGenerator.GenerateWeapon(transform.Find("WeaponLocation"));
        QTEPointLeft = transform.Find("QTEPointLeft");
        QTEPointRight = transform.Find("QTEPointRight");
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
            Destroy(other.gameObject);
        }
        if (IsSelected)
        {
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
            QTEManager.instance.transform.position = QTEPointRight.position;
        }
        else
        {
            QTEManager.instance.transform.position = QTEPointLeft.position;
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

    public override void AimWeapon()
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
