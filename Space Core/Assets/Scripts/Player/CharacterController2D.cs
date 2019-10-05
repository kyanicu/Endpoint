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

        if (rb.bodyType != RigidbodyType2D.Dynamic)
            rb.bodyType = RigidbodyType2D.Dynamic;
        if (!rb.simulated)
            rb.simulated = true;
        if (!rb.freezeRotation)
            rb.freezeRotation = true;

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

        rb.MovePosition(moveBy);

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
                rb.MovePosition((distance) * Vector2.down);
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
            rb.MovePosition (Vector2.up * Physics2D.defaultContactOffset);
    }

    private void UpdateState()
    {
        int maxSize = 10;
        ContactPoint2D[] contacts = new ContactPoint2D[maxSize];
        int size = rb.GetContacts(contacts);
        if (size > 0)
        {
            Vector2 slopeNormal = Vector2.zero;
            for (int i = 0; i < size; i++)
            {
                if (Vector2.Angle(Vector2.up, contacts[i].normal) <= slopeMax)
                    slopeNormal += contacts[i].normal;
            }
            slopeNormal.Normalize();

            if (slopeNormal != Vector2.zero)
            {
                if (!isGrounded)
                    Ground(slopeFromNormal(slopeNormal));
            }
            else if (isGrounded)
            {
                if (!AttemptReground())
                    Unground();
            }

        }
        else
        {
            if (isGrounded)
            {
                if (!AttemptReground())
                    Unground();
            }
        }
    }
    private void FixedUpdate()
    {

        rb.velocity = Vector2.zero;

        UpdateState();
    }
}
