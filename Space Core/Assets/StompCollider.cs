using System.Collections;
using UnityEngine;

public class StompCollider : MonoBehaviour
{
    public FirstBossController Boss;
    private bool detectPlayer;
    private bool harmPlayer;
    private const int DAMAGE = 15;
    private const float HURT_BOX_DELAY = 0.78f;
    private const float HURT_BOX_ACTIVE = 0.1f;
    private const float COOLDOWN = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        detectPlayer = true;
        harmPlayer = false;
    }

    /// <summary>
    /// Triggers the stomp attack when Boss collides with player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") 
            && detectPlayer
            && Boss.Character.animationState != Character.AnimationState.runextended)
        {
            Boss.Stomp();
            detectPlayer = false;
            StartCoroutine(HurtBoxCoroutine());
        }
        else if (other.CompareTag("Player") && harmPlayer)
        {
            PlayerController.instance.ReceiveAttack(new AttackInfo(DAMAGE, Vector2.zero, 0, 0, DamageSource.Enemy));
            harmPlayer = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")
            && detectPlayer 
            && Boss.Character.animationState != Character.AnimationState.runextended)
        {
            Boss.Stomp();
            detectPlayer = false;
            StartCoroutine(HurtBoxCoroutine());
        }
        else if (collision.CompareTag("Player") && harmPlayer)
        {
            PlayerController.instance.ReceiveAttack(new AttackInfo(DAMAGE, Vector2.zero, 0, 0, DamageSource.Enemy));
            harmPlayer = false;
        }
    }

    private IEnumerator HurtBoxCoroutine()
    {
        yield return new WaitForSeconds(HURT_BOX_DELAY);
        harmPlayer = true;
        yield return new WaitForSeconds(HURT_BOX_ACTIVE);
        harmPlayer = false;
        yield return new WaitForSeconds(COOLDOWN);
        detectPlayer = true;

        if (Boss.Character.animationState == Character.AnimationState.special)
        {
            Boss.Character.SetAnimationState(Character.AnimationState.idle);
        }
    }
}
