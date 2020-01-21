using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleCollider : MonoBehaviour
{
    public UpgradesOverlayManager UpgradeManager;
    private int lastIconChecked = -1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        string iconName = col.name;

        //Start at end of string just before number
        int startPoint = 4;
        int id = int.Parse(iconName.Substring(startPoint));

        if (id != lastIconChecked)
        {
            lastIconChecked = id;
            UpgradeManager.SnapReticle(id);
        }
    }
}
