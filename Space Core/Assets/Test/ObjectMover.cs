using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    public float speed;

    Rigidbody2D rb;
    Collider2D col;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
    }

    private void MoveDiscrete(Vector2 moveBy)
    {
        transform.position += (Vector3)moveBy;
        Physics2D.SyncTransforms();

        bool stuck = false;
        bool wasFixed = false;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        filter.useTriggers = false;
        int maxSize = 5;
        Collider2D[] colliders = new Collider2D[maxSize];
        Vector2[] points = new Vector2[maxSize];
        Vector2[] normals = new Vector2[maxSize];
        int numHits;

        int loops = 0;
        do
        {
            stuck = false;
            if ((numHits = col.OverlapCollider(filter, colliders)) > 0)
            {

                if (numHits > maxSize)
                    numHits = maxSize;

                for (int i = 0; i < numHits; i++)
                {

                    ColliderDistance2D dist = col.Distance(colliders[i]);

                    if (!wasFixed)
                    {
                        points[i] = dist.pointA;
                        normals[i] = -dist.normal;
                    }

                    if (dist.distance < -Physics2D.defaultContactOffset)
                    {

                        if (!wasFixed)
                        {
                            points = new Vector2[maxSize];
                            normals = new Vector2[maxSize];
                        }

                        stuck = true;
                        wasFixed = true;

                        transform.position += (Vector3)(dist.pointB - dist.pointA + (Physics2D.defaultContactOffset * dist.normal));

                        Physics2D.SyncTransforms();
                    }

                }
            }

            loops++;
            if (loops > 3)
            {
                Debug.LogError("Possible Infinite Loop. Exiting");
                break;
            }

        } while (stuck);


    }
    
    private void MoveContinuous(Vector2 moveBy)
    {
        // TODO
        // Use Discrete for now
        MoveDiscrete(moveBy);

    }

    public void Move(Vector2 moveBy)
    {
        if (rb.collisionDetectionMode == CollisionDetectionMode2D.Continuous)
            MoveContinuous(moveBy);
        else
            MoveDiscrete(moveBy);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
            inputDirection += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            inputDirection += Vector2.right;
        if (Input.GetKey(KeyCode.W))
            inputDirection += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            inputDirection += Vector2.down;

        inputDirection.Normalize();

        Move(inputDirection * speed * Time.fixedDeltaTime);



    }
}
