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
    private float runSpeed = 5, jumpVelocity = 10, gravityScale = 1, jumpCancelMinVel = 8, jumpCancelVel = 2, airAccel = 50, airDecel = 25, airMax = 5 ;

    private bool isJumping, isJumpCanceling;

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
            direction = 0;
        else if (charCont.isTouchingLeftWall && direction == -1)
            direction = 0;

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
            if (direction == 0)
                velocity = Vector2.zero;
            else
            {
                velocity -= (Vector2)Vector3.Project(velocity, charCont.currentSlope);
                velocity += charCont.currentSlope * runSpeed * direction;
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

            if (charCont.isTouchingRightWall)
                CancelDirectionalVelocity(Vector2.right);
            else if (charCont.isTouchingLeftWall)
                CancelDirectionalVelocity(Vector2.left);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

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
        }

        forceUnground = false;
    }
}
