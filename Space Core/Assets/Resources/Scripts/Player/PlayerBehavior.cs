using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public GameObject Enemy;
    private GameObject HackArea;

    /// <summary>
    /// Allows Player to select Enemy they want to hack
    /// Uses Raycast, currently only allows for raycast to the right
    /// </summary>
    void HackSelector()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.right, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Enemy = hit.collider.gameObject;
                HackArea = hit.collider.gameObject.transform.Find("HackArea").gameObject;
                HackArea.SetActive(true);
            }
        }
    }
    
    /// <summary>
    /// Switches the player control to a hacked enemy
    /// Destroys the player's old body and makes the Enemy the Player
    /// </summary>
    public void Switch()
    {
        Destroy(HackArea);
        Enemy.AddComponent<PlayerBehavior>();
        Enemy.AddComponent<BasicMovement>();
        Enemy.tag = "Player";
        Destroy(gameObject);
    }

    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            HackSelector();
        }
    }
}
