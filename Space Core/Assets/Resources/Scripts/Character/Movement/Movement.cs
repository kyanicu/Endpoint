﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    protected Character owner;

    /// <summary>
    /// Have the default values been set?
    /// </summary>
    protected bool defaultValuesSet;

    public CharacterController2D charCont;

    private Vector2 _velocity;
    public Vector2 velocity { get { return _velocity; } set { _velocity = value; } }

    /// <summary>
    /// Max Velocity
    /// </summary> 
    private float terminalVelocity = 50;

    protected Vector2 movingDirection = Vector2.zero;

    /// <summary>
    /// Are we forcing the character controller off the ground this frame?
    /// </summary>
    private bool _forceUnground;
    public bool forceUnground { protected get { return _forceUnground; } set { _forceUnground = value; } }

    /// <summary>
    /// Are we preventing Run() from being called?
    /// </summary>
    public bool freezeRun;

    /// <summary>
    /// Does the character collide with other characters (not pass through)?
    /// </summary>
    public bool collideWithCharacters = true;


    /// <summary>
    /// Character movement values
    /// </summary>
    [SerializeField]
    private float _runMax, _runAccel, _runDecel, _runBreak,
        _jumpVelocity, _gravityScale, _jumpCancelMinVel, _jumpCancelVel,
        _airAccel, _airDecel, _airMax,
        _pushForce,
        _mass;

    /// <summary>
    /// Used to temporarily modify movement values,
    /// stackable,
    /// use * to add modification (mod *= 2)
    /// use / to remove modification (mod /= 2)
    /// </summary>
    public float mod = 1;

    // The following values return the encapsulated value with the current set modifier  

    public float runMax { get { return _runMax * mod; } set { _runMax = value; } }
    public float runAccel { get { return _runAccel * mod; } protected set { _runAccel = value; } }
    public float runDecel { get { return _runDecel * mod; } protected set { _runDecel = value; } }
    public float runBreak { get { return _runBreak * mod; } protected set { _runBreak = value; } }
    public float jumpVelocity { get { return _jumpVelocity * mod; } protected set { _jumpVelocity = value; } }
    public float gravityScale { get { return _gravityScale * mod; } protected set { _gravityScale = value; } }
    public float jumpCancelMinVel { get { return _jumpCancelMinVel * mod; } protected set { _jumpCancelMinVel = value; } }
    public float jumpCancelVel { get { return _jumpCancelVel / mod; } protected set { _jumpCancelVel = value; } }
    public float airAccel { get { return _airAccel * mod; } protected set { _airAccel = value; } }
    public float airDecel { get { return _airDecel * mod; } protected set { _airDecel = value; } }
    public float airMax { get { return _airMax * mod; } protected set { _airMax = value; } }
    public float pushForce { get { return _pushForce * mod; } protected set { _pushForce = value; } }
    public float mass { get { return _mass * mod; } protected set { _mass = value; } }
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

    /// <summary>
    /// Used in editor on component add, and when manually reset
    /// </summary>
    private void Reset()
    {
        SetDefaultValues();
        defaultValuesSet = true;
        if (!(charCont = GetComponent<CharacterController2D>()))
            charCont = gameObject.AddComponent<CharacterController2D>();
    }

    protected virtual void Awake()
    {
        owner = GetComponent<Character>();

        // used incase component was added outside of editor (such as via instatiate)
        if (!defaultValuesSet)
        {
            SetDefaultValues();
            defaultValuesSet = true;
        }

        if (!(charCont = GetComponent<CharacterController2D>()))
            charCont = gameObject.AddComponent<CharacterController2D>();
        else
            charCont = GetComponent<CharacterController2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collideWithCharacters && (collision.tag == "Enemy" || collision.tag == "Player" || (collision.tag == "Weapon" && collision.GetComponent<Weapon>().owner.gameObject != gameObject)))
        {
            Vector2 dist;
            if (collision.tag == "Weapon")
                dist = -collision.GetComponent<Weapon>().aimingDirection;
            else
                dist = -(transform.position - collision.transform.position).normalized;

            CancelDirectionalVelocity(dist.normalized);

        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        if (collideWithCharacters && (collision.tag == "Enemy" || collision.tag == "Player" || (collision.tag == "Weapon" && collision.GetComponent<Weapon>().owner.gameObject != gameObject)))
        {
            float maxPushbackVel = 5;
            float accel = (tag == "Player") ? 50 : 0;

            Vector2 dist;
            if (collision.tag == "Weapon")
                dist = collision.GetComponent<Weapon>().aimingDirection;
            else
                dist = (transform.position - collision.transform.position).normalized;

            if (Vector3.Project(velocity, dist).magnitude < maxPushbackVel)
                velocity += (Vector2)Vector3.Project(dist, Vector2.right).normalized * accel * Time.deltaTime;

        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collideWithCharacters && (collision.tag == "Enemy" || collision.tag == "Player" || (collision.tag == "Weapon" && collision.GetComponent<Weapon>().owner.gameObject != gameObject)))
        {
            Vector2 dist;
            if (collision.tag == "Weapon")
                dist = collision.GetComponent<Weapon>().aimingDirection;
            else
                dist = (transform.position - collision.transform.position).normalized;

            CancelDirectionalVelocity(dist.normalized);

        }

        if(!collision.isTrigger)
        {
            int contactCount;
            UpdateState(Vector2.up, out contactCount);
        }
    }

    /// <summary>
    /// Handles intended movement on x and y axes
    /// </summary>
    /// <param name="direction">direction of intended movement</param>
    public virtual void Move(Vector2 direction)
    {
        movingDirection = direction;
        if (direction.x == 0)
            Run(0);
        else
            Run(Mathf.Sign(direction.x));
    }

    /// <summary>
    /// Default run method
    /// </summary>
    /// <param name="direction">direction the character of movement,
    /// 0 == none,
    /// -1 == left,
    /// +1 == right</param>
    protected virtual void Run(float direction)
    {
        if (freezeRun)
            return;

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

            // Deceleration
            if (direction == 0 || velocity.magnitude > runMax)
            {
                if (velocity.magnitude - runDecel * Time.fixedDeltaTime <= 0)
                    velocity = Vector2.zero;
                else
                    velocity += charCont.currentSlope * runDecel * -velSign * Time.fixedDeltaTime;
            }
            // Breaking
            else if (velSign != direction && velocity.magnitude != 0)
            {
                if (velocity.magnitude - runDecel * Time.fixedDeltaTime <= 0)
                    velocity = Vector2.zero;
                else
                    velocity += charCont.currentSlope * runBreak * direction * Time.fixedDeltaTime;
            }
            // Acceleration
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
            ObjectPooler.instance.SpawnFromPool("JumpParticle", transform.position, Quaternion.identity);
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
    /// Virtual blank function for unique movement option for different movement classes
    /// </summary>
    public virtual void SpecialAbility() 
    {
        
    }

    /// <summary>
    /// Applies knockback velocity to the player
    /// </summary>
    public void TakeKnockback(Vector2 impulse, float time)
    {

        if (impulse == Vector2.zero || time == 0)
        {
            return;
        }

        if (impulse.y > 0.1)
            forceUnground = true;
        else
            impulse = Vector3.Project(impulse, charCont.currentSlope);

        StartCoroutine(FreezeRunFor(time));
        StartCoroutine(PassThroughCharactersFor(time));
        velocity = impulse / mass;
    }

    public IEnumerator FreezeRunFor(float time)
    {
        freezeRun = true;
        yield return new WaitForSeconds(time);
        freezeRun = false;
    }

    public IEnumerator PassThroughCharactersFor(float time)
    {
        collideWithCharacters = false;
        yield return new WaitForSeconds(time);
        collideWithCharacters = true;
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

            //if (charCont.isTouchingCeiling)
                //CancelDirectionalVelocity(Vector2.up);

            if (charCont.isTouchingRightWall)
                CancelDirectionalVelocity(Vector2.right);
            else if (charCont.isTouchingLeftWall)
                CancelDirectionalVelocity(Vector2.left);

            // Need to figure out why this acts weird, for now use the less accurate code up top
            
            for (int i = 0; i < contactCount; i++)
            {
                if (contacts[i].isCorner)
                {
                    float x;
                    float y;

                    if (Mathf.Sign(contacts[i].normal.x) != Mathf.Sign(velocity.x))
                        x = 0;
                    else
                        x = velocity.x;

                    if (Mathf.Sign(contacts[i].normal.y) != Mathf.Sign(velocity.y) && velocity.y > 0)
                        y = 0;
                    else
                        y = velocity.y;

                    velocity = new Vector2(x, y);
                    
                }
                else if (contacts[i].wasHit)
                {
                    CancelDirectionalVelocity(-contacts[i].normal);
                }
            }
            
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

        if (direction.normalized != proj.normalized)
            return;

        velocity -= proj;
        
    }

    public ContactData[] UpdateState(Vector2 simMoveDir, out int contactCount)
    {
        bool prevGroundedState = charCont.isGrounded;
       
        ContactData[] contactDatas = charCont.UpdateState(out contactCount, simMoveDir);

        HandleContacts(contactDatas, contactCount);

        if (charCont.isGrounded)
        {
            if (prevGroundedState != charCont.isGrounded)
                velocity = Vector3.Project(velocity, charCont.currentSlope);
            else
                velocity = charCont.currentSlope * velocity.magnitude * Mathf.Sign(Vector2.Dot(charCont.currentSlope, velocity));
        }

        forceUnground = false;
        pushingDirection = 0;

        return contactDatas;
    }

    /// <summary>
    /// Main function where velocity is handled appropriately
    /// Virtual incase child class wishes to handle velocity differently
    /// </summary>
    protected virtual void FixedUpdate()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (velocity.magnitude > terminalVelocity)
            velocity = velocity.normalized * terminalVelocity;

        bool prevGroundedState = charCont.isGrounded;

        if (isJumpCanceling)
        {
            if (velocity.y <= _jumpCancelMinVel)
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
            velocity += Physics2D.gravity * _gravityScale * Time.fixedDeltaTime;

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