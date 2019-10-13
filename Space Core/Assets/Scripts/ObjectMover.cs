using System.Collections;
using System.Collections.Generic;
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
        if (moveCompleted = moveRemaining.magnitude < Physics2D.defaultContactOffset)
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
    public Vector2 point;
    public Vector2 normal;

    public ContactData(bool _wasHit, Vector2 _point, Vector2 _normal)
    {
        wasHit = _wasHit;
        point = _point;
        normal = _normal;
    }
}

public class ObjectMover : MonoBehaviour
{

    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    Rigidbody2D rb;
    Collider2D col;

    private void OnValidate()
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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private ContactData[] HandleDiscreteCollision(Vector2 moveDirection, out int contactCount)
    {

        bool stuck;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ContactData[] contacts = new ContactData[maxSize];

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
                    if (dist.distance < -Physics2D.defaultContactOffset || dist.distance == 0)
                    {
                        stuck = true;
                        wasFixed = true;

                        float angle = Vector2.Angle(-moveDirection, -dist.normal);
                        Vector3 fix = (Vector3)((dist.distance/Mathf.Cos(angle*Mathf.Deg2Rad)  + 0.001f) * moveDirection);

                        transform.position += fix;
                    }
                    else if (dist.distance == 0)
                    {
                        stuck = true;
                        wasFixed = true;

                        Vector3 fix = (Vector3)(0.0035f * moveDirection);

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

        } while (stuck);

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

    private ContactData[] HandleContinuousCollision(Vector2 moveDirection, out int contactCount)
    {

        bool stuck;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ContactData[] contacts = new ContactData[maxSize];

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
                    contacts[i] = new ContactData(Vector2.Dot(-dist.normal, moveDirection) < 0, dist.pointB, -dist.normal);

                }
                else if (dist.distance > 0)
                {
                    contacts[i] = new ContactData(Vector2.Dot(dist.normal, moveDirection) < 0, dist.pointB, dist.normal);
                }
            }
        }

        return contacts;

    }

    private MoveData MoveContinuous(Vector2 moveBy)
    {

        Vector2 moveMade;

        int maxSize = 5;

        bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[maxSize];
        int numHits;
        if ((numHits = col.Cast(moveBy.normalized, hits, moveBy.magnitude)) > 0)
        {

            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

            if (numHits > maxSize)
                numHits = maxSize;

            float distance = moveBy.magnitude;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.distance < distance)
                    distance = hit.distance;
            }

            moveMade = moveBy.normalized * (distance - Physics2D.defaultContactOffset);

        }
        else
        {
            moveMade = moveBy.normalized * (moveBy.magnitude - Physics2D.defaultContactOffset);
        }

        Vector3 prevPos = transform.position;

        if (moveMade.normalized != moveBy.normalized)
        {
            return MoveData.invalid;
        }

        transform.position += (Vector3)moveMade;
        Physics2D.SyncTransforms();

        int contactCount;
        ContactData[] contacts = HandleContinuousCollision(moveBy.normalized, out contactCount);

        if (moveMade.magnitude > moveBy.magnitude)
            moveMade = moveBy;

        return new MoveData(moveBy, moveMade, contacts, contactCount);

    }

    public MoveData Move(Vector2 moveBy)
    {
        if (moveBy == Vector2.zero)
            return MoveData.invalid;

        if (rb.collisionDetectionMode == CollisionDetectionMode2D.Continuous)
        {
            // Buggy, requires fixing using discrete for now
            return MoveDiscrete(moveBy);//MoveContinuous(moveBy);
            
        }
        else
            return MoveDiscrete(moveBy);

    }

    public MoveData[] MoveMax(Vector2 moveBy, out int size)
    {
        if (moveBy == Vector2.zero)
        {
            size = 1;
            MoveData[] md = new MoveData[1];
            md[0] = MoveData.invalid;
            return md;
        }

        int maxSize = 10;
        Vector2 newMoveBy = moveBy;
        MoveData[] moveDatas = new MoveData[maxSize];
        List<Vector2> directions = new List<Vector2>();
        directions.Add(newMoveBy.normalized);
        int loops = 0;
        size = 1;
        while (!(moveDatas[size-1] = Move(newMoveBy)).moveCompleted)
        {
            Vector2 averageHitNormal = Vector2.zero;
            for (int i = 0; i < moveDatas[size-1].contactCount; i++)
            {
                if (moveDatas[size - 1].contacts[i].wasHit)
                    averageHitNormal += moveDatas[size - 1].contacts[i].normal;
                if (moveDatas[size - 1].contacts[i].wasHit)
                    Debug.Log("Fuck");
            }
            if (averageHitNormal != Vector2.zero)
                averageHitNormal.Normalize();

            //Debug.Log(averageHitNormal);

            directions.Add(newMoveBy);

            newMoveBy = Vector3.Project(Vector3.Project(moveDatas[size-1].moveRemaining, moveBy),
                                        slopeFromNormal(averageHitNormal));

            if (Vector2.Dot(newMoveBy, moveBy) <= 0 || directions.Contains(newMoveBy.normalized))
                break;

            directions.Add(newMoveBy.normalized);

            loops++;
            if (loops > maxSize)
            {
                Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }
            size++;
        }
        return moveDatas;

    }

    public float speed;
    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
            direction += Vector2.right;
        if (Input.GetKey(KeyCode.A))
            direction += Vector2.left;
        if (Input.GetKey(KeyCode.W))
            direction += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector2.down;

        int size;
        MoveMax(direction * speed * Time.fixedDeltaTime, out size);
    }
}
