using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
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

    public void CancelDirectionalVelocity(Vector2 direction)
    {
        
        Vector2 proj = Vector3.Project(velocity, direction);

        if (direction.normalized != proj.normalized)
            return;

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
            velocity += charCont.currentSlope * runSpeed * direction;
        }
    }

    public void Jump()
    {
        if (charCont.isGrounded)
        {
            velocity = new Vector2(velocity.x, jumpVelocity);
            forceUnground = true;
        }
    }

    private void HandleContacts(ContactData[] contacts, int contactCount)
    {
        if (!charCont.isGrounded)
        {
            for (int i = 0; i < contactCount; i++)
            {
                if (contacts[i].wasHit)
                    CancelDirectionalVelocity(-contacts[i].normal);
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {


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
