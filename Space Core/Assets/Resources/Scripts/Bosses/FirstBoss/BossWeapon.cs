using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossWeapon : MonoBehaviour
{
    public enum State { ConnectedToBoss, Independent }

    public BossCharacter Owner;
    public int Damage;
    public float StunTime;
    public float KnockBackImpulse;
    public float KnockBackTime;
    public float RateOfFire;
    public float Range;
    public float ProjectileSpeed;
    public string BulletTag;
    public bool Stunned;
    public GameObject FireLocation;
    public Transform AttachedBossLocation;
    public float FireTimer;
    public Transform PointToMoveTo;
    public AudioClip FireSfx;
    protected AudioSource audioSource { get; set; }


    public abstract bool Activate(int behavior);

    public void GoToPoint(Transform Point)
    {

    }

    public void AimWeapon(float angle)
    {
        bool pointLeft = Mathf.Abs(angle) > 90;
        if (pointLeft ^ Owner.lookingLeft)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            newScale = transform.localScale;
            newScale.x *= -1;
            newScale.y *= -1;
            transform.localScale = newScale;
        }
        if (Owner.lookingLeft)
        {
            angle *= -1;
        }

        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    protected virtual void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;

        if (FireTimer >= 0)
        {
            FireTimer -= Time.deltaTime;
        }
    }
}
