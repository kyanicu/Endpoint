using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    float timer;

    public ObjectMover mover;
    public Collider2D col;

    List<Character> groundedCharacters = new List<Character>();

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
    }


    // Start is called before the first frame update
    void Start()
    {
        
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
    public Vector2 direction = Vector2.right;

    // Update is called once per frame
    void FixedUpdate()
    {

        timer += Time.fixedDeltaTime;
        if (timer >= change)
        {
            direction = -direction;
            timer = 0f;
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
                Debug.Log("hit");
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
