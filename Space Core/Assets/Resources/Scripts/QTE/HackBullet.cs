using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBullet : MonoBehaviour
{
    private float Speed = 45f;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Player.instance.Enemy = col.gameObject.GetComponent<Enemy>();
            gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        transform.position += transform.right * Speed * Time.deltaTime;
    }
}
