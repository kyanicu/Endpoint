using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    /// <summary>
    /// Have the default values been set?
    /// </summary>
    protected bool defaultValuesSet;

    protected CharacterController2D charCont;

    private Vector2 _velocity;
    public Vector2 velocity { get { return _velocity; } protected set { _velocity = value; } }

    /// <summary>
    /// Are we forcing the character controller off the ground this frame?
    /// </summary>
    private bool _forceUnground;
    public bool forceUnground { protected get { return _forceUnground; } set { _forceUnground = value; } }

    /// <summary>
    /// Character movement values
    /// </summary>
    [SerializeField]
    protected float runMax, runAccel, runDecel,
        jumpVelocity, gravityScale, jumpCancelMinVel, jumpCancelVel,
        airAccel, airDecel, airMax,
        pushForce;

    /// <summary>
    /// Current jump values
    /// </summary>
    protected bool isJumping, isJumpCanceling;

    /// <summary>
    /// direction the character is moving if into a wall/object,
    /// 0 == none,
    /// -1 == left,
    /// +1 == right
    /// </summary>
    protected float pushingDirection;
    
    /// <summary>
    /// Use to set default values for concrete classes
    /// </summary>
    protected abstract void SetDefaultValues();

    private void OnValidate()
    {
        //Const Values

        if (!GetComponent<CharacterController2D>())
            gameObject.AddComponent<CharacterController2D>();

    }

    /// <summary>
    /// Used in editor on component add, and when manually reset
    /// </summary>
    private void Reset()
    {
        SetDefaultValues();
        defaultValuesSet = true;
    }

    protected void Awake()
    {
        // used incase component was added outside of editor (such as via instatiate)
        if (!defaultValuesSet)
        {
            SetDefaultValues();
            defaultValuesSet = true;
        }

        charCont = GetComponent<CharacterController2D>();
    }

    /// <summary>
    /// Default run method
    /// </summary>
    /// <param name="direction">direction the character of movement,
    /// 0 == none,
    /// -1 == left,
    /// +1 == right</param>
    public virtual void Run(float direction)
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

    /// <summary>
    /// Default jump method
    /// </summary>
    public virtual void Jump()
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

    /// <summary>
    /// default jump cancel method
    /// </summary>
    public virtual void JumpCancel()
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

    /// <summary>
    /// Default contact handling method, called every frame where character is touching a collider
    /// </summary>
    /// <param name="contacts">Contact data</param>
    /// <param name="contactCount">size of contact data</param>
    protected virtual void HandleContacts(ContactData[] contacts, int contactCount)
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
            /*
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
                */
        }
    }

    /// <summary>
    /// Default push method when running into a wall
    /// </summary>
    /// <param name="contact">point of contact where pushing is occuring</param>
    protected virtual void Push(ContactData contact)
    {
        contact.collider.attachedRigidbody.AddForceAtPosition(pushForce * Vector2.right * pushingDirection, contact.point);
    }

    /// <summary>
    /// Zeroes out velocity along given direction
    /// </summary>
    /// <param name="direction">direction of velocity to zero out</param>
    public void CancelDirectionalVelocity(Vector2 direction)
    {

        Vector2 proj = Vector3.Project(velocity, direction);

        velocity -= proj;


        if (direction.normalized != proj.normalized)
            return;

        //velocity -= proj;

    }

    /// <summary>
    /// Main function where velocity is handled appropriately
    /// Virtual incase child class wishes to handle velocity differently
    /// </summary>
    protected virtual void FixedUpdate()
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
