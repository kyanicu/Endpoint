using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterController2D : MonoBehaviour
{

    public void SlopeFromNormal(Vector2 slope)
    {

    }

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


    [SerializeField] private float slopeChangeThreshold = 45;
    [SerializeField] private float stepHeightThreshold = 0.5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D capCol;

    private bool initialValidation = true;
    private void OnValidate() {
        if (initialValidation) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            capCol = gameObject.AddComponent<CapsuleCollider2D>();

            // Default Values

            initialValidation = false;
        }

        //Const Values
        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;
        if (!rb.simulated)
            rb.simulated = true;
        if (rb.useFullKinematicContacts)
            rb.useFullKinematicContacts = false;

        if (!capCol.isTrigger)
            capCol.isTrigger = true;
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Move(Vector2 moveBy)
    {

        if (rb.collisionDetectionMode == CollisionDetectionMode2D.Discrete)
            MoveDiscrete(moveBy);
        else
            MoveContinous(moveBy);

    }

    private void MoveDiscrete(Vector2 moveBy)
    {
        if (isGrounded)
            translate(moveBy);
        else
            translate(Vector3.Project(moveBy, currentSlope));
    }

    private void translate(Vector2 moveBy)
    {
        transform.position += (Vector3)moveBy;
    }

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
        while (!moveData = moveUntilCollision(newMoveBy)).moveCompleted)
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

        if (!isAttachedToGround && overLedge != 0)
            moveBy = Vector3.Project(moveBy, Vector2.right);

        Vector2 newMoveBy = moveBy;
        MoveData moveData;
        int loops = 0;
        while (!moveCompleted(moveData = moveUntilCollision(newMoveBy)) && velocity != Vector2.zero)
        {

            if (!isGrounded)
            {
                moveMaxPossibleXYDistance(moveData.moveLeft);
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
            FixCollision(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            FixCollision(collider);
        }
    }

    // Update is called once per frame
    void FixCollision(Collider2D collider)
    {
        ColliderDistance2D dist = capCol.Distance(collider);

        if (dist.distance < -totalCollisionOffset)
        {
            translate(dist.distance * dist.normal);
        }


    }
}
