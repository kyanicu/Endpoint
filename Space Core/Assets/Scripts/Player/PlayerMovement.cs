using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D col;

    //private Vector2 _velocity;
    public Vector2 velocity { get { return rb.velocity; } private set { rb.velocity = value;  } }

    private float runSpeed, jumpVelocity;
    private bool isGrounded;

    private Vector2 currentSlope;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        rb.gravityScale = 1;
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
        if (!isGrounded)
        {
            velocity = new Vector2(runSpeed * direction, velocity.y);
        }
        else
        {
            velocity = currentSlope * runSpeed * direction;
        }
    }

    public void Jump ()
    {
        if (isGrounded)
        {
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

        int hitCount = col.Cast(Vector2.down, hits, 0.05f);

        isGrounded = false;

        Vector2 normal = Vector2.zero;
        for (int i = 0; i < hitCount; i++)
        {
            if (Vector2.Angle(hits[i].normal, Vector2.up) <= 45)
            {
                normal += hits[i].normal;
            }
        }
        if (normal != Vector2.zero)
        {
            normal.Normalize();
            isGrounded = true;

            currentSlope = (Quaternion.Euler(0, 0, -90) * normal).normalized;

            Debug.Log(currentSlope);
        }
        else
        {
            currentSlope = Vector2.right;
        }

        if (isGrounded && rb.gravityScale != 0) 
        {
            rb.gravityScale = 0;
        }
        else if (rb.gravityScale == 0)
        {
            rb.gravityScale = 1;
        }
    }
}
