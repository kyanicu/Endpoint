using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationChanger : MonoBehaviour
{
    [Header("Position A")]
    [Space]
    [Header("Leave Section empty if not changing!")]
    public Transform PositionA;
    public string SectionA;
    public string AreaA;

    [Header("Position B")]
    public Transform PositionB;
    public string SectionB;
    public string AreaB;

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            float playerDistToA = Vector2.Distance(col.gameObject.transform.position, PositionA.position);
            float playerDistToB = Vector2.Distance(col.gameObject.transform.position, PositionB.position);

            //If player is closer to position A than B when exiting trigger
            if (playerDistToA < playerDistToB)
            {
                //Update minimap position with Position A info
                HUDController.instance.UpdateMinimap(SectionA, AreaA);
            }
            else
            {
                //Else update minimap position with Position B info
                HUDController.instance.UpdateMinimap(SectionB, AreaB);
            }
        }
    }
}
