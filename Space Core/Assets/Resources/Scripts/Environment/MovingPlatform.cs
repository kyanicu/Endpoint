using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public ObjectMover mover;
    public Collider2D col;

    private Vector3 playerPos { get { return PlayerController.instance.Character.transform.position; } }
    private Vector3 transformOrigin { get { return transform.position + Vector3.down * 2; } }

    List<Character> groundedCharacters = new List<Character>();

    Vector3 center;

    private void Reset()
    {
        //Const Values

        col = GetComponent<Collider2D>();

        if (!(mover = GetComponent<ObjectMover>()))
            mover = gameObject.AddComponent<ObjectMover>();

        col.isTrigger = false;
        mover.stopOnHit = false;

    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        if (!(mover = GetComponent<ObjectMover>()))
            mover = gameObject.AddComponent<ObjectMover>();

        col.isTrigger = false;
        mover.stopOnHit = false;

        center = transform.position + (Vector3)(direction * speed * change) / 2;

    }


    // Start is called before the first frame update
    void Start()
    {
        defaultSpeed = speed;    
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Character character = other.GetComponent<Character>();
            int contactCount;
            ContactData[] contacts = character.movement.UpdateState(-direction, out contactCount);
            if (character.movement.charCont.isGrounded) 
            {
                for (int i = 0; i < contactCount; i++)
                {
                    if (contacts[i].collider == null)
                        continue;
                    else if (contacts[i].collider.gameObject == gameObject && character.movement.charCont.CheckGroundableSlope(contacts[i].normal))
                    {
                        groundedCharacters.Add(character);
                    }
                }
            }
        }
    }
/*
    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (direction.y < 0 && (other.tag == "Player" || other.tag == "Enemy"))
        {

            Character character = other.GetComponent<Character>();
            int contactCount;
            ContactData[] contacts = character.movement.UpdateState(direction, out contactCount);
            if (character.movement.charCont.isGrounded) 
            {
                for (int i = 0; i < contactCount; i++)
                {
                    if (contacts[i].collider == null)
                        continue;
                    else if (contacts[i].collider.gameObject == gameObject && character.movement.charCont.CheckGroundableSlope(contacts[i].normal))
                    {
                        groundedCharacters.Add(character);
                    }
                }
            }
            else 
            {
                bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
                Physics2D.queriesHitTriggers = true;

                int maxSize = 10;
                RaycastHit2D[] hits = new RaycastHit2D[maxSize];
                int numHits;
                if ((numHits = character.movement.charCont.capCol.Cast(direction, hits, Physics2D.defaultContactOffset + (speed * Time.fixedDeltaTime))) > 0)
                {
                    if(numHits > maxSize)
                        numHits = maxSize;

                    for (int i = 0; i < numHits; i++)
                    {
                        if (hits[i].collider.gameObject == gameObject && character.movement.charCont.CheckGroundableSlope(hits[i].normal))
                        {
                            groundedCharacters.Add(character);
                        }
                    }
                }
                Physics2D.queriesHitTriggers = prevQueriesHitTriggers;
            }
        }
        
    }
*/
    public float speed = 5f;
    public float change = 1f;
    private float timer;
    public Vector2 direction = Vector2.right;

    public float defaultSpeed;

    public bool entered; 
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Vector2.Distance(transform.position, playerPos) <= 5) 
        {
            if (!entered)
            {
                if (speed == 0 && Vector3.Dot(Vector2.up, playerPos - transformOrigin) >= .8f)
                {
                    speed = defaultSpeed;
                }
                else if (speed != 0 && Vector3.Dot(Vector2.down, playerPos - transformOrigin) >= .6f) 
                {
                    direction *= -1;
                    timer = change - timer;
                }
                entered = true;
            }
        }
        else
        {

            if (speed == 0 && Vector3.Dot(direction, playerPos - center) >= 0.5f)
            {
                speed = defaultSpeed;
            }
            else if (speed != 0 && Vector3.Dot(-direction, playerPos - center) >= 0.5f)
            {
                direction *= -1;
                timer = change - timer;
            }

            entered = false;
        }



        if (speed != 0) 
        {
            timer += Time.fixedDeltaTime;
        }
        if (timer >= change)
        {
            timer = 0;
            speed = 0;
            direction *= -1f;
        }
        mover.Move(speed * direction * Time.fixedDeltaTime);
        
        if(direction.y < 0)
        {

            bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = true;

            int maxSize = 10;
            RaycastHit2D[] hits = new RaycastHit2D[maxSize];
            int numHits;
            if ((numHits = col.Cast(-direction, hits, Physics2D.defaultContactOffset + (speed * Time.fixedDeltaTime))) > 0)
            {
                if(numHits > maxSize)
                    numHits = maxSize;

                for (int i = 0; i < numHits; i++)
                {

                    if (hits[i].collider.gameObject.tag == "Player" || hits[i].collider.gameObject.tag == "Enemy")
                    {
                        
                        Character character = hits[i].collider.gameObject.GetComponent<Character>();
                        int contactCount;
                        if (character.movement.charCont.isGrounded && character.movement.charCont.CheckGroundableSlope(-hits[i].normal));
                        {
                            if (!groundedCharacters.Contains(character))
                                groundedCharacters.Add(character);
                        }
                    }
                }
            }
            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;
        }

        foreach(Character character in groundedCharacters){
            int moveCount;
            MoveData[] moveData = character.movement.charCont.Move(speed * direction * Time.fixedDeltaTime, out moveCount, true);
        }
        groundedCharacters.Clear();
    }
}
