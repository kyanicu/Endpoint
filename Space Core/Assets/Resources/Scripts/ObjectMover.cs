using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static VectorLibrary;

public struct MoveData
{
    private static MoveData _invalid { get { MoveData m = new MoveData(); m.isValid = false; return m; } }
    public static MoveData invalid { get { return _invalid; } }

    public bool isValid;

    public Vector2 moveInit;
    public Vector2 moveMade;
    public Vector3 moveRemaining;
    public bool moveCompleted;
    public ContactData[] contacts;
    public int contactCount;

    public MoveData(Vector2 _moveInit, Vector2 _moveMade, ContactData[] _contacts, int _contactCount)
    {
        isValid = true;

        moveInit = _moveInit;
        moveMade = _moveMade;
        moveRemaining = moveInit - moveMade;
        if (moveCompleted = moveRemaining.magnitude < Physics2D.defaultContactOffset + 0.001)
        {
            moveMade = moveInit;
            moveRemaining = Vector2.zero;
        }

        contacts = _contacts;
        contactCount = _contactCount;
    }

}

public struct ContactData
{
    public bool wasHit;
    public bool isCorner;
    public Vector2 point;
    public Vector2 normal;
    public Collider2D collider;

    public ContactData(bool _wasHit, Vector2 _point, Vector2 _normal, Collider2D _collider, LayerMask layerMask)
    {
        wasHit = _wasHit;
        point = _point;
        normal = _normal;
        collider = _collider;
        isCorner = false;
        isCorner = CheckIfCorner(layerMask);
    }

    private bool CheckIfCorner(LayerMask layerMask)
    {

        Vector2 rightDirection = slopeFromNormal(normal);

        //float distance = hit.distance + Physics2D.defaultContactOffset;

        //Vector2 hitStartPoint = point + (normal * distance);
        Vector2 leftPoint = point - (rightDirection * Physics2D.defaultContactOffset);
        Vector2 rightPoint = point + (rightDirection * Physics2D.defaultContactOffset);

        RaycastHit2D leftHit = Physics2D.Raycast(leftPoint, -normal, Physics2D.defaultContactOffset*2, layerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(rightPoint, -normal, Physics2D.defaultContactOffset*2, layerMask);

        bool right = rightHit.collider != null;
        bool left = leftHit.collider != null;

        float angleRightToLeft = Vector2.SignedAngle(rightHit.normal, leftHit.normal);

        //if (!(right && left))
        //Debug.Break();

        //vertexData = new VertexData(hit, leftHit, rightHit, angleRightToLeft);

        //if (check if landed on a curved surface)
            //return false;
        if (!(right && left))
            return true;
        else if (angleRightToLeft > 0)
            return true;
        else
            return false;

    }
}

public class ObjectMover : MonoBehaviour
{
    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    Rigidbody2D rb;
    Collider2D col;

    private void Reset()
    {
        //Const Values

        if (!(rb = GetComponent<Rigidbody2D>()))
            rb = gameObject.AddComponent<Rigidbody2D>();

        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;
        if (!rb.simulated)
            rb.simulated = true;

    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!(rb = GetComponent<Rigidbody2D>()))
            rb = gameObject.AddComponent<Rigidbody2D>();
        else
            rb = GetComponent<Rigidbody2D>();

        col = GetComponent<Collider2D>();

        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;
        if (!rb.simulated)
            rb.simulated = true;

    }

    /*private ContactData[] HandleDiscreteCollision(Vector2 moveDirection, out int contactCount)
    {

        bool stuck;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useLayerMask = true;
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ContactData[] contacts = new ContactData[maxSize];
        int prevCount = 0;
        int loops = 0;
        do
        {
            stuck = false;
            if ((contactCount = col.OverlapCollider(filter, colliders)) > 0)
            {

                if (contactCount > maxSize)
                    contactCount = maxSize;

                for (int i = 0; i < contactCount; i++)
                {

                    ColliderDistance2D dist = col.Distance(colliders[i]);

                    if (dist.distance < 0)
                    {
                        contacts[i] = new ContactData(Vector2.Dot(-dist.normal, moveDirection) < -0.001, dist.pointB, -dist.normal);

                    }
                    else if (dist.distance > 0)
                    {
                        contacts[i] = new ContactData(Vector2.Dot(dist.normal, moveDirection) < -0.001, dist.pointB, dist.normal);
                    }

                    //Debug.Log( "Dist: " + dist.distance + "Normal: " + contacts[i].normal + ", Move: " + moveDirection + ", Dot: " + Vector2.Dot(contacts[i].normal, moveDirection));
                    //Debug.Log(contacts[i].wasHit);

                    bool wasFixed = false;
                    if (dist.distance < -Physics2D.defaultContactOffset)
                    {

                        float angle = Vector2.Angle(-moveDirection, -dist.normal);
                        Vector3 fix = (Vector3)((dist.distance / Mathf.Cos(angle * Mathf.Deg2Rad) + 0.001f) * moveDirection);

                        if (fix.magnitude > -dist.distance * 3)
                            fix = (dist.distance) * dist.normal;

                        stuck = true;
                        wasFixed = true;

                        //Debug.Log("Angle: " + angle + " Fix: " + fix.magnitude + " Dist: " + dist.distance);

                        transform.position += fix;
                    }
                    else if (dist.distance == 0)
                    {
                        stuck = true;
                        wasFixed = true;
                        Vector3 fix = (Vector3)(0.001f * moveDirection);
                        transform.position += fix;
                    }


                    if(wasFixed)
                        Physics2D.SyncTransforms();
                }
            }

            loops++;
            if (loops > maxSize)
            {
                Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }

            if (loops > 1 && contactCount == 0 && !stuck)
                contactCount = prevCount;
            else 
                prevCount = contactCount;

        } while (stuck);

        col.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Physics2D.autoSimulation = false;
        Physics2D.Simulate(0);
        Physics2D.autoSimulation = true;
        ContactPoint2D[] contactPoints = new ContactPoint2D[5];
        Debug.Log(rb.GetContacts(contactPoints));
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = true;
        return contacts;

    }*/

    private ContactData[] HandleDiscreteCollision(Vector2 moveDirection, out int contactCount)
    {
        // Check for and handle any overlap

        bool stuck;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useLayerMask = true;
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ColliderDistance2D[] dists = new ColliderDistance2D[maxSize];
        int prevCount = 0;
        int distCount = 0;
        int loops = 0;
        do
        {
            stuck = false;
            if ((distCount = contactCount = col.OverlapCollider(filter, colliders)) > 0)
            {

                if (contactCount > maxSize)
                    contactCount = maxSize;

                for (int i = 0; i < contactCount; i++)
                {

                    dists[i] = col.Distance(colliders[i]);
                    
                    bool wasFixed = false;
                    if (dists[i].distance < -Physics2D.defaultContactOffset)
                    {

                        float angle = Vector2.Angle(-moveDirection, -dists[i].normal);
                        Vector3 fix = (Vector3)((dists[i].distance / Mathf.Cos(angle * Mathf.Deg2Rad) + 0.001f) * moveDirection);

                        if (fix.magnitude > -dists[i].distance * 3)
                            fix = (dists[i].distance) * dists[i].normal;

                        stuck = true;
                        wasFixed = true;

                        transform.position += fix;
                    }
                    else if (dists[i].distance == 0)
                    {
                        stuck = true;
                        wasFixed = true;
                        Vector3 fix = (Vector3)(0.001f * moveDirection);
                        transform.position += fix;
                    }

                    if (wasFixed)
                        Physics2D.SyncTransforms();
                }

                prevCount = contactCount;

            }

            loops++;
            if (loops > maxSize && stuck)
            {
                //Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }
            
        } while (stuck);

        if (distCount == 0)
            distCount = prevCount;
        

        // Gather contact points
        ContactData[] contacts = new ContactData[(maxSize*4)];
        contactCount = 0;

        bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[maxSize];
        int numHits;
        if ((numHits = col.Cast(moveDirection, hits, Physics2D.defaultContactOffset)) > 0)
        {

            if (numHits > maxSize)
                numHits = maxSize;

            //Debug.Log("Hit");
            for (int i = 0; i < numHits; i++)
            {
                contacts[contactCount] = new ContactData(true, hits[i].point, hits[i].normal, hits[i].collider, layerMask);
                contactCount++;

                //Debug.Log(contacts[i].normal);

            }

        }
        if ((numHits = col.Cast(rotateVector(moveDirection, 90), hits, Physics2D.defaultContactOffset)) > 0)
        {
            for (int i = 0; i < numHits; i++)
            {
                //if ((rotateVector(moveDirection, -90) == -hits[i].normal))
                    //Debug.Log("Side+");
                //else
                    //Debug.Log("Hit");

                bool alreadyIn = false;
                for (int j = 0; j < contactCount; j++)
                {
                    if (hits[i].normal == contacts[j].normal)
                        alreadyIn = true;
                }
                if (alreadyIn)
                    continue;

                contacts[contactCount] = new ContactData((rotateVector(moveDirection, 90) != -hits[i].normal), hits[0].point, hits[i].normal, hits[i].collider, layerMask);
                contactCount++;

                //Debug.Log(contacts[i].normal);

            }
        }
        if ((numHits = col.Cast(rotateVector(moveDirection, -90), hits, Physics2D.defaultContactOffset)) > 0)
        {
            for (int i = 0; i < numHits; i++)
            {
                //if (rotateVector(moveDirection, -90) == -hits[i].normal)
                    //Debug.Log("Side-");
                //else
                    //Debug.Log("Hit");

                bool alreadyIn = false;
                for (int j = 0; j < contactCount; j++)
                {
                    if (hits[i].normal == contacts[j].normal)
                        alreadyIn = true;
                }
                if (alreadyIn)
                    continue;

                contacts[contactCount] = new ContactData(rotateVector(moveDirection, -90) != -hits[i].normal, hits[0].point, hits[i].normal, hits[i].collider, layerMask);
                contactCount++;

                //Debug.Log(contacts[i].normal);

            }
        }
        //Debug.Log(distCount);
        //Debug.Log(contactCount);

        if (distCount > contactCount)
        {
            contactCount = distCount;
            for (int i = 0; i < distCount; i++)
            {
                if (dists[i].distance < -Mathf.Epsilon)
                {

                    contacts[i] = new ContactData(Vector2.Dot(-dists[i].normal, moveDirection) < -Mathf.Epsilon, dists[i].pointB, -dists[i].normal, hits[i].collider, layerMask);

                    //Debug.Log(contacts[i].normal);
                }
                else if (dists[i].distance > Mathf.Epsilon)
                {
                    contacts[i] = new ContactData(Vector2.Dot(dists[i].normal, moveDirection) < -Mathf.Epsilon, dists[i].pointB, dists[i].normal, hits[i].collider, layerMask);
                    //Debug.Log(contacts[i].normal);

                }
                else
                {
                    contacts[i] = new ContactData(true, dists[i].pointB, moveDirection, hits[i].collider, layerMask);
                    //Debug.Log(contacts[i].normal);

                }
            }
        }

        Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

        return contacts;

    }

    private MoveData MoveDiscrete(Vector2 moveBy)
    {
        Vector3 prevPos = transform.position;

        transform.position += (Vector3)moveBy;
        Physics2D.SyncTransforms();

        int contactCount;
        ContactData[] contacts = HandleDiscreteCollision(moveBy.normalized, out contactCount);

        Vector2 moveMade = Vector3.Project(transform.position - prevPos, moveBy);
        if (moveMade.magnitude > moveBy.magnitude)
            moveMade = moveBy;

        return new MoveData(moveBy, moveMade, contacts, contactCount);
    }

    /*
    private ContactData[] HandleContinuousCollision(RaycastHit2D[] hits, float moveDistance, Vector2 moveDirection, int  hitCount, out int contactCount)
    {
        
        if (hitCount != 0)
        {
            int initHitCount = hitCount;
            int[] validHits = new int[initHitCount];
            hitCount = 0;
            for (int i = 0; i < initHitCount; i++)
            {
                if (hits[i].distance == moveDistance)
                {
                    Debug.Log(hits[i].normal);
                    validHits[hitCount] = i;
                    hitCount++;
                }
            }
        }

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useLayerMask = true;
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ContactData[] contacts = new ContactData[maxSize + hitCount];
        for (int i = 0; i < hitCount; i++)
        {
            contacts[i] = new ContactData(true, hits[i].point, hits[i].normal, hits[i].collider);
        }
        if ((contactCount = col.OverlapCollider(filter, colliders)) > 0)
        {

            if (contactCount > maxSize)
                contactCount = maxSize;

            int initContactCount = contactCount;
            int[] validContacts = new int[initContactCount];
            contactCount = 0;
            for (int i = 0; i < initContactCount; i++)
            {
                ColliderDistance2D dist = col.Distance(colliders[i]);

                if (Mathf.Abs(Vector2.Dot(dist.normal, moveDirection)) <= Mathf.Epsilon)
                {
                    if (dist.distance < -Mathf.Epsilon)
                    {
                        contacts[contactCount + hitCount] = new ContactData(false, dist.pointB, -dist.normal, hits[i].collider);

                    }
                    else if (dist.distance > Mathf.Epsilon)
                    {
                        contacts[contactCount + hitCount] = new ContactData(false, dist.pointB, dist.normal, hits[i].collider);
                    }
                    contactCount++;
                }
            }
        }

        contactCount += hitCount;

        return contacts;

    }

    private MoveData MoveContinuous(Vector2 moveBy)
    {

        Vector2 moveMade;

        int maxSize = 5;

        bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        float distance = moveBy.magnitude;
        RaycastHit2D[] hits = new RaycastHit2D[maxSize];
        int numHits;
        if ((numHits = col.Cast(moveBy.normalized, hits, moveBy.magnitude)) > 0)
        {

            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

            if (numHits > maxSize)
                numHits = maxSize;

            for (int i = 0; i < numHits; i++)
            {
                if ((hits[i].distance < distance && hits[i].distance != 0) || !(hits[i].distance == 0 && hits[i].normal == moveBy.normalized))
                    distance = hits[i].distance;

                Debug.Log(hits[i].distance);
            }
            moveMade = moveBy.normalized * distance;// - (Physics2D.defaultContactOffset));

        }
        else
        {
            moveMade = moveBy;// moveBy.normalized * (moveBy.magnitude - (Physics2D.defaultContactOffset));
        }    
    
        if (moveMade.normalized != moveBy.normalized)
        {
            return MoveData.invalid;
        }

        transform.position += (Vector3)moveMade;
        Physics2D.SyncTransforms();

        int contactCount;
        ContactData[] contacts = HandleContinuousCollision(hits, distance, moveBy.normalized, numHits, out contactCount);

        if (moveMade.magnitude > moveBy.magnitude)
            moveMade = moveBy;

        return new MoveData(moveBy, moveMade, contacts, contactCount);

    }
    */
    public MoveData Move(Vector2 moveBy)
    {
        if (moveBy == Vector2.zero)
            return MoveData.invalid;

        if (rb.collisionDetectionMode == CollisionDetectionMode2D.Continuous)
        {
            //Buggy, use discrete for now)
            return MoveDiscrete(moveBy); // return MoveContinuous(moveBy);
        }
        else
            return MoveDiscrete(moveBy);

    }

    public MoveData[] MoveMax(Vector2 moveBy, out int moveCount)
    {

        if (moveBy == Vector2.zero)
        {
            moveCount = 0;
            return new MoveData[0];
        }

        int maxSize = 10;
        Vector2 newMoveBy = moveBy;
        MoveData[] moveDatas = new MoveData[maxSize];
        List<Vector2> directions = new List<Vector2>();
        directions.Add(newMoveBy.normalized);
        int loops = 0;
        moveCount = 1;
        while (!(moveDatas[moveCount - 1] = Move(newMoveBy)).moveCompleted)
        {
            Vector2 averageHitNormal = Vector2.zero;
            for (int i = 0; i < moveDatas[moveCount-1].contactCount; i++)
            {
                if (moveDatas[moveCount - 1].contacts[i].wasHit)
                    averageHitNormal += moveDatas[moveCount - 1].contacts[i].normal;
            }
            if (averageHitNormal != Vector2.zero)
                averageHitNormal.Normalize();

            //Debug.Log(moveDatas[moveCount - 1].contactCount);
            directions.Add(newMoveBy);
            
            //newMoveBy = Vector3.Project(Vector3.Project(moveDatas[moveCount - 1].moveRemaining, moveBy),
            //                            slopeFromNormal(averageHitNormal));
            newMoveBy = Vector3.Project(moveDatas[moveCount - 1].moveRemaining, slopeFromNormal(averageHitNormal));

           
            if (newMoveBy == Vector2.zero || Vector2.Dot(newMoveBy, moveBy) <= 0 || directions.Contains(newMoveBy.normalized))
                break;
            //Debug.Log(newMoveBy == Vector2.zero);

            directions.Add(newMoveBy.normalized);

            loops++;
            if (loops > maxSize)
            {
                //Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }
            moveCount++;
        }
        return moveDatas;

    }

    public float speed;
    // Update is called once per frame
    void FixedUpdate()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (Input.GetKey(KeyCode.Return))
            transform.position = new Vector2(0, 2);

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
            direction += Vector2.right;
        if (Input.GetKey(KeyCode.A))
            direction += Vector2.left;
        if (Input.GetKey(KeyCode.W))
            direction += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector2.down;
        direction.Normalize();

        int size;
        MoveData[] moveData = MoveMax(direction * speed * Time.fixedDeltaTime, out size);
        
    }
}
