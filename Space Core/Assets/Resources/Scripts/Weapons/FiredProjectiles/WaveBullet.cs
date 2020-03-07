using System;
using UnityEngine;

public class WaveBullet : Bullet
{
    private float growthSpeed = 4f;
    private int maxPiercing = 1;
    private int numPierced;

    public override void Activate()
    {
        numPierced = 0;
        base.Activate();
    }

    // Update is called once per frame
    public override void Update()
    {
        var newScale = new Vector3(1, 1, 1) * Time.deltaTime * growthSpeed;
        transform.localScale += newScale;
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                DealDamage(collision);
                if (numPierced > maxPiercing)
                {
                    gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
                numPierced++;
            }
        }
    }
}
