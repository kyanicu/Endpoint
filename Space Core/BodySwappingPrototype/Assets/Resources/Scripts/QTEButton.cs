using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEButton : MonoBehaviour
{
    public enum KeyNames
    {
        up,
        down,
        left,
        right
    };
    public KeyNames keyName { get; private set; }
    private int[] arrowRotation =
    {
        180,
        0,
        270,
        90
    };
    public bool active;
    public GameObject arrowImage;
    private Image button;

    public void setColor(Color color)
    {
        button.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 3);
        keyName = (KeyNames)rand;
        Vector3 rotValue = new Vector3(0, 0, arrowRotation[rand]);
        arrowImage.transform.Rotate(rotValue);
        active = false;
        button = GetComponent<Image>();
        setColor(Color.gray);
    }
}
