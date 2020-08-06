using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketArms : BossWeapon
{
    public float LaunchSpeed;
    public Animator Animator;
    public float SitTime;
    public Collider2D collider;
    public State state;

    private ArmAnimationState ArmAnimationState;
    private bool hurtBoxActive;
    private bool hit;
    private bool takeAim;
    private float sitTimer;
    private Transform LastPlayerPosition;
    private Quaternion baseRotation;

    private void Start()
    {
        state = State.ConnectedToBoss;
    }

    public override bool Activate(int behavior)
    {
        switch (behavior)
        {
            case 0:
                return true;
            case 1:
                transform.parent = null;
                takeAim = true;
                state = State.Independent;
                return true;
        }

        return false;
    }

    protected override void Update()
    {
        base.Update();

        if (state == State.Independent && !Stunned)
        {
            AttackPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hit && other.CompareTag("Player"))
        {
            hit = true;
            other.GetComponent<Character>().ReceiveAttack(
                new AttackInfo(
                    Damage,
                    Vector2.zero,
                    KnockBackTime,
                    0,
                    DamageSource.Enemy));
            collider.enabled = false;
        }
        else if (!hit && other.CompareTag("Terrain"))
        {
            hit = true;
            collider.enabled = false;
        }
    }

    private void AttackPlayer()
    {
        float step = Time.deltaTime * ProjectileSpeed;
        if (takeAim)
        {
            step *= 2;
            baseRotation = transform.localRotation;
            Vector2 myPosition = transform.position;
            Vector2 diff = (Vector2) PlayerController.instance.transform.position - myPosition;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(transform.localRotation.x + angle, transform.localRotation.y, 0);
            takeAim = false;
            collider.enabled = true;
        }
        else if (!hit && !takeAim)
        {
            transform.position += Time.deltaTime * ProjectileSpeed * Vector3.right;
        }
        else if (hit && !takeAim && sitTimer > 0)
        {
            sitTimer -= Time.deltaTime;
        }
        else if (hit && !takeAim && sitTimer <= 0)
        {
            transform.position = Vector2.MoveTowards(
                                        transform.position,
                                        AttachedBossLocation.position,
                                        step);
            transform.localRotation = baseRotation;
            if (transform.position.x - AttachedBossLocation.position.x < 1.0f && transform.position.y - AttachedBossLocation.position.y < 1.0f)
            {
                transform.parent = AttachedBossLocation;
                transform.position = AttachedBossLocation.position;
                takeAim = true;
                hit = false;
                sitTimer = SitTime;
                state = State.ConnectedToBoss;
            }
        }
    }
}
