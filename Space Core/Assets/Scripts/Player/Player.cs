using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{

    private PlayerMovement movement;

    private void OnValidate()
    {
        //Const Values

       if(!(movement = GetComponent<PlayerMovement>()))
            movement = gameObject.AddComponent<PlayerMovement>();

    }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
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
