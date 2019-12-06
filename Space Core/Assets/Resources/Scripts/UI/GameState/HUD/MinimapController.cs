using UnityEngine;
using TMPro;

public class MinimapController : MonoBehaviour
{
    public TextMeshProUGUI SectorName;
    public TextMeshProUGUI RoomName;
    
    /// <summary>
    /// Updates text elements on minimap
    /// An area is a subSection
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="area"></param>
    public void UpdateLocation(string sector, string room)
    {
        SectorName.text = sector;
        RoomName.text = room;
    }
}
