using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBullet : MonoBehaviour
{
    private float Speed = 45f;

    public void Start()
    {
        Destroy(gameObject, 5f);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Player.instance.Enemy = col.gameObject.GetComponent<Enemy>();
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        transform.position += transform.right * Speed * Time.deltaTime;
    }
}
