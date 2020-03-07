using UnityEngine;

/// <summary>
/// Homing Radar for the homing bullet. Helps the bullet lock onto a target
/// </summary>
public class HomingRadar : MonoBehaviour
{
    [SerializeField]
    private Bullet parentBullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //set the parent homing bullet to the collision's object's transform
            parentBullet.LockedEnemy = collision.gameObject.transform;
        }
    }
}
