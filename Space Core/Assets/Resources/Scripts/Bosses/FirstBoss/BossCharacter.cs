using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCharacter : Character
{
    public Image UIBar;
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

        Health = 1750;
        MaxHealth = 1750;
    }

    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        base.ReceiveAttack(attackInfo);
        UIBar.fillAmount = Health / MaxHealth;
    }
}
