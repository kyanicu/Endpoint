using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSwitch : InteractableEnv
{
    //list of shock floors from the scene. Added manually.
    public ShockFloor[] shockFloors;

    private void Awake()
    {
        functionalityText = "disable nearby Shock Floors";
    }

    public override void ActivateFunctionality()
    {
        foreach (ShockFloor shockFloor in shockFloors)
        {
            shockFloor.GetComponent<ShockFloor>().enabled = false;
        }
        //Disables self so that it can't be activated again.
        this.enabled = false;
    }
}
