using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    float timer;

    public ObjectMover mover;
    public Collider2D col;

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
    }
}
