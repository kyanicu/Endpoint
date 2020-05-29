using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float velocity;
    public float ActiveTime;
    public GameObject ExplosionRadius;
    public MeshRenderer meshRenderer;
    public float FinishPosition;
    private Vector2 startPosition;
    private bool hit;
    private Collider2D collider;
    private bool movingUp;
    private float colliderInactiveTimer;
    private float activeTimer;

    private const float colliderInactiveTime = 0.2f;

    private void OnEnable()
    {
        collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        colliderInactiveTimer = colliderInactiveTime;
    }

    private void Update()
    {
        if (hit)
        {
            return;
        }

        if (colliderInactiveTimer <= 0)
        {
            collider.enabled = true;
        }
        else
        {
            colliderInactiveTimer -= Time.deltaTime;
        }

        if (movingUp && startPosition.y + FinishPosition > transform.position.y)
        {
            transform.position += Time.deltaTime * velocity * Vector3.up;
            transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }
        else
        {
            movingUp = false;
            float step = Time.deltaTime * velocity;
            transform.position = Vector2.MoveTowards(
                                    transform.position,
                                    PlayerController.instance.Character.transform.position,
                                    step);
            Vector3 lookPos = PlayerController.instance.Character.transform.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        activeTimer -= Time.deltaTime;

        if (activeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        meshRenderer.enabled = true;
        startPosition = this.transform.position;
        movingUp = true;
        activeTimer = ActiveTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Terrain"))
        {
            hit = true;
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        meshRenderer.enabled = false;
        ExplosionRadius.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
