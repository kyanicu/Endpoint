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

    public void CancelDirectionalVelocity(Collider2D collider, Vector2 direction)
    {

        Vector2 proj = Vector3.Project(velocity, direction);

        velocity -= proj;
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
            velocity += charCont. * runSpeed * direction;
        }
    }

    public void onControllerCollidedEvent(RaycastHit2D hit)
    {

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

        charCont.move(velocity * Time.fixedDeltaTime);
        forceUnground = false;

    }
}
