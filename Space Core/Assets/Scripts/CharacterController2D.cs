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


    [SerializeField] private float slopeMax = 45;
    [SerializeField] private float stepMax = 0.5f;

    public ObjectMover mover;
    public CapsuleCollider2D capCol;

    private void OnValidate()
    {
        //Const Values

        if (!(capCol = GetComponent<CapsuleCollider2D>()))
            capCol = gameObject.AddComponent<CapsuleCollider2D>();
        if (!(mover = GetComponent<ObjectMover>()))
            mover = gameObject.AddComponent<ObjectMover>();


        if (!capCol.isTrigger)
            capCol.isTrigger = true;
    }

    private void Awake()
    {
        mover = GetComponent<ObjectMover>();
        capCol = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public MoveData Move(Vector2 moveBy, bool forceUnground = false)
    {
        if (moveBy == Vector2.zero)
            return MoveData.invalid;

        if (forceUnground)
            Unground();
        else if (isGrounded)
            moveBy = Vector3.Project(moveBy, currentSlope);

        MoveData moveData = mover.Move(moveBy);

        HandleMove(moveData);

        return moveData;
    }

    private void SetSlope(Vector2 newSlope)
    {
        currentSlope = newSlope;
    }

    private void Ground(Vector2 newSlope)
    {
        Debug.Log("Hit");
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
                mover.Move((distance - Physics2D.defaultContactOffset) * Vector2.down);
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
            mover.Move(normalFromSlope(currentSlope) * Physics2D.defaultContactOffset);
    }

    private void HandleMove(MoveData moveData)
    {

        if (moveData.contactCount > 0)
        {
            Vector2 slopeNormal = Vector2.zero;
            for (int i = 0; i < moveData.contactCount; i++)
            {
                if (Vector2.Angle(moveData.contacts[i].normal, Vector2.up) < slopeMax)
                    slopeNormal += moveData.contacts[i].normal;
            }
            if (slopeNormal != Vector2.zero)
            {
                slopeNormal = slopeFromNormal(slopeNormal.normalized);

                if (isGrounded)
                {
                    SetSlope(slopeNormal);
                }
                else
                {
                    Ground(slopeNormal);
                }
            }
            else
            {
                if (!AttemptReground())
                    Unground();
            }
        }
        else if (isGrounded)
        {
            if (!AttemptReground())
                Unground();
        }
    }

    private void FixedUpdate()
    { 

    }
}
