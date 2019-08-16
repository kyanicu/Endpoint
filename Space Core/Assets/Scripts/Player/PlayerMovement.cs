using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D col;

    private Vector2 _velocity;
    public Vector2 velocity { get { return _velocity; } private set { _velocity = value; rb.velocity = value; } }

    private float runSpeed, jumpVelocity;
    private bool isGrounded;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void Initialize(float _runSpeed, float _jumpVelocity)
    {
        runSpeed = _runSpeed;
        jumpVelocity = _jumpVelocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Run(float direction)
    {
        velocity = new Vector2(runSpeed * direction, velocity.y);
    }

    public void Jump ()
    {
        if (isGrounded)
        {
            Debug.Log("Jump");
            velocity += jumpVelocity * Vector2.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        RaycastHit2D[] hits = new RaycastHit2D[5];

        int hitCount = col.Cast(Vector2.down, hits, 0.2f);

        isGrounded = false;

        for (int i = 0; i < hitCount; i++)
        {
            if (Vector2.Angle(hits[i].normal, Vector2.up) <= 45)
            {
                isGrounded = true;
                rb.MovePosition(Vector2.down * hits[i].normal);
                velocity = new Vector2(velocity.x, 0);
                break;
            }
        }

        if (!isGrounded)
        {
            //velocity += Physics2D.gravity * Time.fixedDeltaTime;
        }

        rb.velocity = velocity;
    }
}
