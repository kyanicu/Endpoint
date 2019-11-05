using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    CharacterController2D charCont;

    private Vector2 _velocity;
    public Vector2 velocity { get { return _velocity; } private set { _velocity = value;  } }

    private bool _forceUnground;
    public bool forceUnground { private get { return _forceUnground; } set { _forceUnground = value; } }

    [SerializeField]
    private float runMax = 7, runAccel = 40, runDecel = 40,
        jumpVelocity = 15, gravityScale = 1, jumpCancelMinVel = 12, jumpCancelVel = 2,
        airAccel = 50, airDecel = 25, airMax = 9,
        pushForce = 14;

    private bool isJumping, isJumpCanceling;

    private float pushingDirection;

    private void OnValidate()
    {
        //Const Values

        if(!GetComponent<CharacterController2D>())
            gameObject.AddComponent<CharacterController2D>();

    }

    private void Awake()
    {
        charCont = GetComponent<CharacterController2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController2D>();
    }

    public void Run(float direction)
    {

        if (charCont.isTouchingRightWall && direction == +1)
        {
            if (charCont.isGrounded)
                pushingDirection = +1;
            //return;
        }
        else if (charCont.isTouchingLeftWall && direction == -1)
        {
            if (charCont.isGrounded)
                pushingDirection = -1;
            //return;
        }

        if (!charCont.isGrounded || forceUnground)
        {

            if (direction == 0)
            {
                int sign = (int)Mathf.Sign(velocity.x);
                velocity += Vector2.right * -Mathf.Sign(velocity.x) * airDecel * Time.fixedDeltaTime;
                if (Mathf.Sign(velocity.x) != sign)
                    velocity = new Vector2(0, velocity.y);
            }
            else
            {
                if (Mathf.Abs(velocity.x) < airMax)
                {
                    velocity += Vector2.right * direction * airAccel * Time.fixedDeltaTime;

                    if (Mathf.Abs(velocity.x) > airMax)
                        velocity = new Vector2(direction * airMax, velocity.y);
                }
            }

            if (Mathf.Abs(velocity.x) >= airMax)
                velocity += Vector2.right * -Mathf.Sign(velocity.x) * airDecel * Time.fixedDeltaTime;
        }
        else
        {

            float velSign = Mathf.Sign(Vector2.Dot(charCont.currentSlope, velocity));

            if (direction == 0 || velocity.magnitude > runMax)
            {
                if (velocity.magnitude - runDecel * Time.fixedDeltaTime <= 0)
                    velocity = Vector2.zero;
                else
                    velocity += charCont.currentSlope * runDecel * -velSign * Time.fixedDeltaTime;
            }
            else
            {
                if (Mathf.Abs((velocity.magnitude * velSign) + (runAccel * direction * Time.fixedDeltaTime)) >= runMax)
                    velocity = charCont.currentSlope * runMax * direction;
                else
                    velocity += charCont.currentSlope * runAccel * Time.fixedDeltaTime * direction;
            }
        }
    }

    public void Jump()
    {
        if (charCont.isTouchingCeiling)
            return;

        if (charCont.isGrounded)
        {
            velocity = new Vector2(velocity.x, jumpVelocity);
            forceUnground = true;
            isJumping = true;
        }
    }

    public void JumpCancel()
    {

        if (isJumping)
        {
            if (velocity.y <= jumpCancelMinVel)
            {
                isJumping = false;
                velocity = new Vector2(velocity.x, jumpCancelVel);
            }
            else if (velocity.y >= 0 || !charCont.isGrounded)
                isJumpCanceling = true;
        }

    }

    public void CancelDirectionalVelocity(Vector2 direction)
    {

        Vector2 proj = Vector3.Project(velocity, direction);

        velocity -= proj;


        if (direction.normalized != proj.normalized)
            return;

        //velocity -= proj;

    }

    private void HandleContacts(ContactData[] contacts, int contactCount)
    {
        if (!charCont.isGrounded)
        {

            if (charCont.isTouchingCeiling)
                CancelDirectionalVelocity(Vector2.up);

            if (charCont.isTouchingRightWall)
                CancelDirectionalVelocity(Vector2.right);
            else if (charCont.isTouchingLeftWall)
                CancelDirectionalVelocity(Vector2.left);

            // Need to figure out why this acts weird, for now use the less accurate code up top
            /* 
            for (int i = 0; i < contactCount; i++)
            {
                if (contacts[i].wasHit)
                { 
                    CancelDirectionalVelocity(-contacts[i].normal);
                }
            }
            */
        }
        else
        {
            bool pushing = false;
            for(int i = 0; i < contactCount; i++)
            {
                if (contacts[i].collider.tag == "PushableObject")
                    Debug.Log(pushingDirection);
                if (pushingDirection == -1 && contacts[i].normal.x > 0 && contacts[i].collider.tag == "PushableObject")
                {
                    pushing = true;
                    Push(contacts[i]);
                    break;
                }
                else if(pushingDirection == +1 && contacts[i].normal.x < 0 && contacts[i].collider.tag == "PushableObject")
                {
                    pushing = true;
                    Push(contacts[i]);
                    break;
                }
            }

            if (charCont.isTouchingRightWall && !(pushing && pushingDirection == +1))
                CancelDirectionalVelocity(Vector2.right);
            else if (charCont.isTouchingLeftWall && !(pushing && pushingDirection == -1))
                CancelDirectionalVelocity(Vector2.left);
        }
    }

    void Push(ContactData contact)
    {
        contact.collider.attachedRigidbody.AddForceAtPosition(pushForce * Vector2.right * pushingDirection, contact.point);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        bool prevGroundedState = charCont.isGrounded;

        if (isJumpCanceling)
        {
            if (velocity.y <= jumpCancelMinVel)
            {
                isJumpCanceling = false;
                JumpCancel();
            }
            else if (charCont.isGrounded && !forceUnground)
                isJumpCanceling = false;
        }

        if (isJumping && (velocity.y <= 0 || (charCont.isGrounded && !forceUnground)))
        {
            isJumping = false;
        }

        if (!charCont.isGrounded)
            velocity += Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

        if (velocity != Vector2.zero)
        {
            int moveCount;
            MoveData[] moveDatas = charCont.Move(velocity * Time.fixedDeltaTime, out moveCount, forceUnground);
            
            for (int i = 0; i < moveCount; i++)
                HandleContacts(moveDatas[i].contacts, moveDatas[i].contactCount);
            if (charCont.isGrounded)
            {
                if (prevGroundedState != charCont.isGrounded)
                    velocity = Vector3.Project(velocity, charCont.currentSlope);
                else
                    velocity = charCont.currentSlope * velocity.magnitude * Mathf.Sign(Vector2.Dot(charCont.currentSlope, velocity));
            }
        }

        forceUnground = false;
        pushingDirection = 0;
    }
}
