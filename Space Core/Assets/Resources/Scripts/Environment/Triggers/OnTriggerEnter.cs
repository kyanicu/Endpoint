using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnter : AbsTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            handleResult();
        }
    }
}
