using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController2D charCont;

    private Vector2 _velocity;
    public Vector2 velocity { get { return _velocity; } private set { _velocity = value;  } }

    private bool _forceUnground;
    public bool forceUnground { private get { return _forceUnground; } set { if (value) _forceUnground = value; } }

    [SerializeField]
    private float runSpeed = 5, jumpVelocity = 10, gravityScale = 1;

    private void OnValidate()
    {
        //Const Values

        if(!GetComponent<CharacterController2D>())
            charCont = gameObject.AddComponent<CharacterController2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            CancelDirectionalVelocity(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            CancelDirectionalVelocity(collider);
        }
    }

    public void CancelDirectionalVelocity(Collider2D collider)
    {
        ColliderDistance2D dist = charCont.capCol.Distance(collider);

        Vector2 proj = Vector3.Project(velocity, -dist.normal);

        if (Vector2.Dot(-dist.normal, velocity) > 0)
        {
            velocity -= proj;
        }
    }

    public void Run(float direction)
    {
        if (!charCont.isGrounded)
        {
            velocity = new Vector2(runSpeed * direction, velocity.y);
        }
        else
        {
            velocity -= (Vector2) Vector3.Project(velocity, charCont.currentSlope);
            velocity += charCont.currentSlope * runSpeed * direction;
        }
    }

    public void Jump ()
    {
        if (charCont.isGrounded)
        {
            velocity += jumpVelocity * Vector2.up;
            forceUnground = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.Log(charCont.isGrounded);

        if (!charCont.isGrounded)
          velocity += Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

        charCont.Move(velocity * Time.fixedDeltaTime, forceUnground);
        forceUnground = false;

    }
}
