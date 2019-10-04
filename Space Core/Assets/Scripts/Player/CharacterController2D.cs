using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorLibrary;

public static class VectorLibrary
{

    public static bool areNearlyEquivalent(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b) < 0.1f;
    }

    public static float findSmallestAngledVector(Vector2[] angledVectors, Vector2 normal)
    {
        float smallest = 180;

        foreach (Vector2 vector in angledVectors)
        {
            float angle = Vector2.Angle(normal, vector);
            if (angle < smallest)
                smallest = angle;
        }
        return smallest;
    }

    public static Vector2 slopeFromNormal(Vector2 normal)
    {
        return rotateVector(normal, -90);
    }

    public static Vector2 normalFromSlope(Vector2 slope)
    {
        return rotateVector(slope, 90);
    }

    public static Vector2 rotateVector(Vector2 vector, float angle)
    {

        Vector2 rotatedVector = Quaternion.Euler(0, 0, angle) * vector;

        return rotatedVector;
    }

    public static Vector2 vectorFromAngle(float angle)
    {
        return Quaternion.Euler(0, 0, angle) * Vector2.right;
    }

    public static float getAngleFromHorizon(Vector2 slope)
    {
        float fromRight = Vector2.Angle(Vector2.right, slope);
        float fromLeft = Vector2.Angle(Vector2.left, slope);

        return ((fromRight < fromLeft) ? fromRight : fromLeft);

    }

    public static Vector2 averageVector(Vector2[] vectors)
    {

        Vector2 average = Vector2.zero;

        foreach (Vector2 vector in vectors)
        {
            average += vector;
        }

        average /= 3;

        return average;
    }
}

public class CharacterController2D : MonoBehaviour
{

    private struct MoveData
    {

        public Vector2 moveInitial;
        public Vector2 moveDone;
        public Vector2 moveLeft { get { return moveInitial - moveDone; } }
        public Vector2 normal;
        public RaycastHit2D[] hits;
        public int hitSize;

        public bool wasContinous;

        public bool moveCompleted { get { return moveLeft.magnitude <= CharacterController2D.totalCollisionOffset; } }

        public MoveData(Vector2 init, Vector2 done, Vector2 _normal, RaycastHit2D[] _hits, int _hitSize, bool _wasContinous)
        {
            moveInitial = init;
            moveDone = done;
            normal = _normal;
            hits = _hits;
            hitSize = _hitSize;
            wasContinous = _wasContinous;
        }

    }

    private const float extraCollisionOffset = 0.0035f;///0.0001f;
    static public float totalCollisionOffset { get { return Physics2D.defaultContactOffset + extraCollisionOffset; } }
    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    private Vector2 _currentSlope;
    public Vector2 currentSlope { get { return _currentSlope; } private set { _currentSlope = value; } }

    private bool _isGrounded;
    public bool isGrounded { get { return _isGrounded; } private set { _isGrounded = value; } }


    [SerializeField] private float slopeMax = 45;
    [SerializeField] private float stepMax = 0.5f;

    public Rigidbody2D rb;
    public CapsuleCollider2D capCol;

    private void OnValidate()
    {
        //Const Values

        if(!GetComponent<Rigidbody2D>())
            rb = gameObject.AddComponent<Rigidbody2D>();

        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;
        if (!rb.simulated)
            rb.simulated = true;
        if (rb.useFullKinematicContacts)
            rb.useFullKinematicContacts = false;

        if (!GetComponent<CapsuleCollider2D>())
            capCol = gameObject.AddComponent<CapsuleCollider2D>();

        if (!capCol.isTrigger)
            capCol.isTrigger = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capCol = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Move(Vector2 moveBy, bool forceUnground = false)
    {
        if (forceUnground)
            Unground(true);

        //if (rb.collisionDetectionMode == CollisionDetectionMode2D.Discrete)
            MoveDiscrete(moveBy);
        //else
            //MoveContinous(moveBy);

    }

    private void MoveDiscrete(Vector2 moveBy)
    {
        if (!isGrounded)
            translate(moveBy);
        else
            translate(Vector3.Project(moveBy, currentSlope));
    }

    private void translate(Vector2 moveBy)
    {
        transform.position += (Vector3)moveBy;
        //rb.MovePosition(transform.position + (Vector3)moveBy);
    }

    /*
    private void MoveContinous(Vector2 moveBy)
    {
        if (isGrounded)
            MoveAlongGround(moveBy);
        else
            MoveMax(moveBy);
    }
    private MoveData moveUntilCollision(Vector2 moveBy)
    {

        Vector2 wallNormalForFixing = Vector2.zero;

        bool prevGrounded = isGrounded;
        Vector2 initMoveBy = moveBy;
        Vector2 normal = Vector2.zero;
        int maxSize = 5;

        bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[maxSize];
        int numHits;
        if ((numHits = capCol.Cast(moveBy.normalized, hits, moveBy.magnitude)) > 0)
        {
            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

            int collidersHit = 0;
            RaycastHit2D[] colliderHits = new RaycastHit2D[numHits];

            if (numHits > maxSize)
                numHits = maxSize;


            float distance = moveBy.magnitude;
            for (int i = 0; i < numHits; i++)
            {
                if (hits[i].distance < distance)
                    distance = hits[i].distance;
            }

            bool projTest = false;
            for (int i = 0; i < numHits; i++)
            {

                if (hits[i].distance > distance)
                    continue;

                colliderHits[collidersHit] = hits[i];
                collidersHit++;

                normal += hits[i].normal;
                if (Vector3.Project(moveBy.normalized * hits[i].distance, hits[i].normal).magnitude <= totalCollisionOffset)
                {
                    projTest = true;
                    break;
                }

            }
            if (collidersHit == 0)
            {

                for (int i = 0; i < numHits; i++)
                {
                    hits[i] = new RaycastHit2D();
                }
                numHits = 0;

                Physics2D.queriesHitTriggers = false;

                if (moveBy.magnitude <= totalCollisionOffset)
                    return new MoveData(initMoveBy, Vector2.zero, normal, hits, numHits, true);

            }
            else
            {

                hits = colliderHits;
                numHits = collidersHit;

                normal.Normalize();

                if (distance < 0)
                    distance = 0;

                moveBy = (moveBy.normalized * (distance - totalCollisionOffset));

                if (Vector2.Dot(moveBy, initMoveBy) <= 0)
                    moveBy = initMoveBy.normalized * totalCollisionOffset / 2;

                if (projTest || distance <= totalCollisionOffset)
                    return new MoveData(initMoveBy, Vector2.zero, normal, hits, numHits, true);
            }

        }
        else
        {
            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

            if (moveBy.magnitude <= totalCollisionOffset)
            {
                return new MoveData(initMoveBy, Vector2.zero, normal, hits, numHits, true);
            }
        }

        translate(moveBy);

        return new MoveData(initMoveBy, moveBy, normal, hits, numHits, true);

    }

    private void MoveMax(Vector2 moveBy)
    {

        bool prevGrounded = isGrounded;

        Vector2 newMoveBy = moveBy;
        MoveData moveData;
        List<Vector2> directions = new List<Vector2>();
        directions.Add(newMoveBy.normalized);
        int loops = 0;
        while (!(moveData = moveUntilCollision(newMoveBy)).moveCompleted)
        {
            if (!prevGrounded && isGrounded)
                break;

            prevGrounded = isGrounded;

            directions.Add(newMoveBy);

            newMoveBy = Vector3.Project(Vector3.Project(moveData.moveLeft, moveBy),
                                        slopeFromNormal(moveData.normal));

            if (Vector2.Dot(newMoveBy, moveBy) <= 0 || directions.Contains(newMoveBy.normalized))
                break;

            directions.Add(newMoveBy.normalized);

            loops++;
            if (loops > 10)
            {
                Debug.LogError("moveMaxPossibleXYDistance(): Possible Infinite Loop. Exiting");
                break;
            }
        }
    }

    private void MoveAlongGround(Vector2 moveBy)
    {

        Vector2 newMoveBy = moveBy;
        MoveData moveData;
        int loops = 0;
        while (!(moveData = moveUntilCollision(newMoveBy)).moveCompleted)
        {

            if (!isGrounded)
            {
                MoveMax(moveData.moveLeft);
                break;
            }

            newMoveBy = Mathf.Sign(Vector2.Dot(moveData.moveLeft, currentSlope))
                * moveData.moveLeft.magnitude * currentSlope;

            loops++;
            if (loops > 10)
            {
                //Debug.Break();
                Debug.LogError("moveAlongGround(): Possible Infinite Loop. Exiting");

                break;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            FixDiscreteCollision(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            FixDiscreteCollision(collider);
        }
    }

    // Update is called once per frame
    void FixDiscreteCollision(Collider2D collider)
    {
        ColliderDistance2D dist = capCol.Distance(collider);

        if (Vector2.Angle(Vector2.up, dist.normal) <= slopeMax)
        {
            currentSlope = slopeFromNormal(-dist.normal);
            isGrounded = true;
        }

        if (dist.distance < -totalCollisionOffset)
        {
            translate(dist.distance * dist.normal);
        }     
    void HandleContinousCollision(MoveData moveData)
    {

    }
    */

    private void SetSlope(Vector2 newSlope)
    {
        currentSlope = newSlope;
    }

    private void Ground(Vector2 newSlope)
    {
        isGrounded = true;
        SetSlope(newSlope);
    }

    private bool AttemptReground()
    {

        int maxSize = 5;

        bool prevQueriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[maxSize];
        int numHits;
        if ((numHits = capCol.Cast(Vector2.down, hits, stepMax)) > 0)
        {
            Physics2D.queriesHitTriggers = prevQueriesHitTriggers;

            if (numHits > maxSize)
                numHits = maxSize;

            float distance = stepMax;
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.distance < distance)
                    distance = hit.distance;
            }

            Vector2 slopeNormal = Vector2.zero;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.distance > distance)
                    break;

                if (Vector2.Angle(Vector2.up, hit.normal) <= slopeMax)
                {
                    slopeNormal += hit.normal;
                }
            }
            slopeNormal.Normalize();

            if (slopeNormal != Vector2.zero)
            {
                translate((distance - totalCollisionOffset) * Vector2.down);
                Ground(slopeFromNormal(slopeNormal));
                return true;
            }
            else
                return false;

        }
        else
            return false;

    }

    public void Unground(bool forced = false)
    {
        isGrounded = false;

        if (forced)
            translate(Vector2.up * totalCollisionOffset);
    }

    private void UpdateState(ColliderDistance2D[] dists, int numHits)
    {

        if (numHits > 0)
        {

            Vector2 slopeNormal = Vector2.zero;
            foreach (ColliderDistance2D dist in dists)
            {
                if (Vector2.Angle(Vector2.up, -dist.normal) <= slopeMax)
                {
                    slopeNormal += -dist.normal;
                }
            }
            slopeNormal.Normalize();
            
            if(slopeNormal != Vector2.zero)
            {
                if (!isGrounded)
                    Ground(slopeFromNormal(slopeNormal));
            }
            else if(isGrounded)
            {
                if (!AttemptReground())
                    Unground();
            }

        }
        else
        {
            if(isGrounded)
            {
                if (!AttemptReground())
                    Unground();
            }
        } 
    }

    private void CheckForAndHandleCollisions(bool movingObjectCollision = false)
    {

        bool stuck = true;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useTriggers = false;

        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        ColliderDistance2D[] dists = new ColliderDistance2D[maxSize];
        int numHits = 0;

        int loops = 0;
        while (stuck)
        {

            stuck = false;
            if ((numHits = capCol.OverlapCollider(filter, colliders)) > 0)
            {
                if (numHits > maxSize)
                    numHits = maxSize;

                for (int i = 0; i < numHits; i++)
                {
                    dists[i] = capCol.Distance(colliders[i]);

                    if (dists[i].distance < 0)
                    {
                        stuck = true;

                        translate((dists[i].distance) * dists[i].normal);
                    }

                }
            }

            loops++;
            if (loops > maxSize)
            {
                Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }
        }

        UpdateState(dists, numHits);

    }


    private void FixedUpdate()
    {
        CheckForAndHandleCollisions();
    }
}
