using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : Character
{
    /// <summary>
    /// Set base objects of Boss
    /// </summary>
    private void Awake()
    {
        if (!(movement = GetComponent<Movement>()))
            movement = gameObject.AddComponent<BasicMovement>();
        else
            movement = GetComponent<Movement>();

        //if (MinimapIcon == null)
        //{
        //    MinimapIcon = transform.Find("MinimapIcon").gameObject;
        //    MinimapIcon.layer = LayerMask.NameToLayer("Minimap/Mapscreen");
        //}
        animator = transform.Find("BossBotAnimated").gameObject.GetComponent<Animator>();

        Health = 500;
        MaxHealth = 500;
    }
}
