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

        average /= vectors.Length;
        average /= vectors.Length;

        return average;
    }
}

public class CharacterController2D : MonoBehaviour
{
    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    private Vector2 _currentSlope;
    public Vector2 currentSlope { get { return _currentSlope; } private set { _currentSlope = value; } }

    private bool _isGrounded;
    public bool isGrounded { get { return _isGrounded; } private set { _isGrounded = value; } }

    private bool _isTouchingCeiling;
    public bool isTouchingCeiling { get { return _isTouchingCeiling; } private set { _isTouchingCeiling = value; } }

    private bool _isTouchingLeftWall;
    public bool isTouchingLeftWall { get { return _isTouchingLeftWall; } private set { _isTouchingLeftWall = value; } }

    private bool _isTouchingRightWall;
    public bool isTouchingRightWall { get { return _isTouchingRightWall; } private set { _isTouchingRightWall = value; } }

    [SerializeField] private float slopeMax = 45;
    [SerializeField] private float stepMax = 0.5f;

    private Vector2 bottomPoint { get { return (Vector2)(capCol.bounds.center + (-transform.up * capCol.size.y / 2)); } }

    public ObjectMover mover;
    public CapsuleCollider2D capCol;

    private void Reset()
    {
        //Const Values

        if (!(capCol = GetComponent<CapsuleCollider2D>()))
            capCol = gameObject.AddComponent<CapsuleCollider2D>();
        if (!(mover = GetComponent<ObjectMover>()))
            mover = gameObject.AddComponent<ObjectMover>();

        capCol.isTrigger = true;

    }

    private void Awake()
    {
        if (!(capCol = GetComponent<CapsuleCollider2D>()))
            capCol = gameObject.AddComponent<CapsuleCollider2D>();
        else
            mover = GetComponent<ObjectMover>();

        if (!(mover = GetComponent<ObjectMover>()))
            mover = gameObject.AddComponent<ObjectMover>();
        else
            capCol = GetComponent<CapsuleCollider2D>();

        capCol.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool CheckStep(ContactData corner)
    {
        Debug.Log("Hit");
        if (corner.point.y <= bottomPoint.y)
            return false;
        else if (corner.point.y - bottomPoint.y <= stepMax)
            return true;
        else
            return false;
    }

    private void HandleStep(ContactData corner)
    {
        transform.position += (Vector3)(corner.point - bottomPoint);
    }

    public MoveData[] MoveAlongGround(Vector2 moveBy, out int moveCount)
    {
        isTouchingCeiling = false;
        isTouchingRightWall = false;
        isTouchingLeftWall = false;

        if (moveBy == Vector2.zero)
        {
            moveCount = 0;
            return new MoveData[0];
        }
        int maxSize = 10;
        MoveData[] moveDatas = new MoveData[maxSize];
        Vector2 newMoveBy = moveBy;
        int loops = 0;
        moveCount = 1;
        bool completed = false;
        while (true)
        {
            completed = (moveDatas[moveCount - 1] = mover.Move(newMoveBy)).moveCompleted;

            isTouchingCeiling = false;
            isTouchingRightWall = false;
            isTouchingLeftWall = false;
            
            bool hitWall = false;
            Vector2 slopeNormal = Vector2.zero;
            int size = moveDatas[moveCount - 1].contactCount;
            bool stepHit = false;
            for (int i = 0; i < size; i++)
            {

                ContactData contact = moveDatas[moveCount - 1].contacts[i];

                if (Vector2.Angle(contact.normal, Vector2.up) < slopeMax && !stepHit)
                    slopeNormal += contact.normal;
                else
                {
                    if (contact.wasHit)
                        hitWall = true;
                    if (contact.isCorner && CheckStep(contact))
                    {
                        slopeNormal = contact.normal;//HandleStep(contact);
                        stepHit = true;
                    }
                    else
                    {
                        if (Vector2.Dot(contact.normal, Vector2.down) > 0.000001f)
                            isTouchingCeiling = true;

                        float dotLeft = Vector2.Dot(contact.normal, Vector2.left);
                        if (dotLeft > Mathf.Epsilon)
                            isTouchingRightWall = true;
                        else if (dotLeft < -Mathf.Epsilon)
                            isTouchingLeftWall = true;
                    }
                }
            }
            if (slopeNormal != Vector2.zero)
            {
                SetSlope(slopeFromNormal(slopeNormal.normalized));
            }
            else
            {
                if (!AttemptReground())
                {
                    Unground();
                    break;
                }
                 
            }
            if (hitWall || isGrounded)
                break;


            newMoveBy = Mathf.Sign(Vector2.Dot(moveDatas[moveCount - 1].moveRemaining, currentSlope))
                * moveDatas[moveCount - 1].moveRemaining.magnitude * currentSlope;

            moveCount++;

            loops++;
            if (loops > maxSize)
            {
                Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }
        }

        return moveDatas;

    }

    public MoveData[] Move(Vector2 moveBy, out int moveCount, bool forceUnground = false)
    {
        if (moveBy == Vector2.zero)
        {
            moveCount = 0;
            return new MoveData[0];
        }

        if (forceUnground)
            Unground();
        else if (isGrounded)
            moveBy = Vector3.Project(moveBy, currentSlope);

        MoveData[] moveDatas;
        if (!isGrounded)
        {
            moveDatas = mover.MoveMax(moveBy, out moveCount);
            HandleUngroundedMove(moveDatas, moveCount);
        }
        else
        {
            moveDatas = MoveAlongGround(moveBy, out moveCount);
        }

        return moveDatas;
    }

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
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.distance < distance && hit.distance != 0)
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
                HandleReground(mover.Move(distance * Vector2.down), slopeFromNormal(slopeNormal));
                
                return true;
            }
            else
                return false;
        }
        else
            return false;

    }

    private void HandleReground(MoveData moveData, Vector2 slope)
    {

        for (int i = 0; i < moveData.contactCount; i++)
        {
            ContactData contact = moveData.contacts[i];

            if (Vector2.Angle(moveData.contacts[i].normal, Vector2.up) >= slopeMax)
            {
                if (Vector2.Dot(contact.normal, Vector2.down) > 0.000001f)
                    isTouchingCeiling = true;

                float dotLeft = Vector2.Dot(contact.normal, Vector2.left);
                if (dotLeft > Mathf.Epsilon)
                    isTouchingRightWall = true;
                else if (dotLeft < -Mathf.Epsilon)
                    isTouchingLeftWall = true;
            }
        }

        Ground(slope);
    }

    public void Unground(bool forced = false)
    {
        isGrounded = false;

        if (forced)
            mover.Move(normalFromSlope(currentSlope) * Physics2D.defaultContactOffset);
    }

    private void HandleUngroundedMove(MoveData[] moveDatas, int moveCount)
    {
        if (moveCount == 0)
            return;

        isTouchingCeiling = false;
        isTouchingLeftWall = false;
        isTouchingRightWall = false;

        MoveData moveData = moveDatas[moveCount - 1];
        if (moveData.contactCount > 0)
        {
            Vector2 slopeNormal = Vector2.zero;
            for (int i = 0; i < moveData.contactCount; i++)
            {

                ContactData contact = moveDatas[moveCount - 1].contacts[i];

                if (Vector2.Angle(moveData.contacts[i].normal, Vector2.up) < slopeMax)
                    slopeNormal += moveData.contacts[i].normal;
                else
                {

                    if (Vector2.Dot(contact.normal, Vector2.down) > 0.000001f)
                        isTouchingCeiling = true;

                    float dotLeft = Vector2.Dot(contact.normal, Vector2.left);
                    if (dotLeft > Mathf.Epsilon)
                        isTouchingRightWall = true;
                    else if (dotLeft < -Mathf.Epsilon)
                        isTouchingLeftWall = true;
                }
            }
            if (slopeNormal != Vector2.zero)
            {
                slopeNormal = slopeFromNormal(slopeNormal.normalized);

                Ground(slopeNormal);
            }
        }
    }

    private void FixedUpdate()
    {

    }
}
