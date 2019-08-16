using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{

    private PlayerMovement movement;

    [SerializeField]
    private float runSpeed, jumpVelocity;

    void Awake()
    {
        movement = gameObject.AddComponent<PlayerMovement>();
        movement.Initialize(runSpeed, jumpVelocity);
    }

    public void Run(float direction)
    {
        movement.Run(direction);
    }

    public void Jump()
    {
        movement.Jump();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
