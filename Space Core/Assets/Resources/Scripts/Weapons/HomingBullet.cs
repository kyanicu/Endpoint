using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Homing bullets lock onto nearest enemy in the direction it is going and moves towards that enemy
/// </summary>
public class HomingBullet : Bullet
{
    //Enemy that the bullet will lock onto
    private Transform LockedEnemy;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        LockedEnemy = null;
    }

    /// <summary>
    /// Have the bullet lock onto and change direction to go towards enemy
    /// </summary>
    new void Update()
    {
        if (LockedEnemy == null)
        {
            //calculate vector 2 positon because OverlapCircleAll uses WorldSpace, not local
            Vector2 point = transform.right * (Range / 2);
            point = new Vector2(transform.position.x + point.x, transform.position.y + point.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, (Range / 2));
            foreach (Collider2D hit in colliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    LockedEnemy = hit.transform;
                    break;
                }
            }
        }

        if (LockedEnemy != null)
        {
            Vector3 targetDirection = LockedEnemy.transform.position - transform.position;
            transform.position += (targetDirection.normalized * Velocity * Time.deltaTime);
        }
        else
        {
            base.Update();
        }
    }
}
