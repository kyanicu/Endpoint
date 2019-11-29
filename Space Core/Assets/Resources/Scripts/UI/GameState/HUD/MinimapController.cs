using UnityEngine;
using TMPro;

public class MinimapController : MonoBehaviour
{
    public TextMeshProUGUI SectionName;
    public TextMeshProUGUI AreaName;
    
    /// <summary>
    /// Updates text elements on minimap
    /// An area is a subsection
    /// </summary>
    /// <param name="section"></param>
    /// <param name="area"></param>
    public void UpdateLocation(string section, string area)
    {
        SectionName.text = section;
        AreaName.text = area;
    }
}
