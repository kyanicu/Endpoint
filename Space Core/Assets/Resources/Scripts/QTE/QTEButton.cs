using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEButton : MonoBehaviour
{
    //QTE button's current status
    public bool Active;

    //The QTE button's attached arrow image
    public GameObject ArrowImage;

    //Enums for possible QTE button presses
    public enum KeyNames { up, down, left, right };

    //This button's needed QTE key
    public KeyNames keyName { get; private set; }

    /// <summary>
    /// Rotation to be applied to arrow image.
    /// Arrow starts facing downward and items line up with 
    /// items in KeyNames which explains the array's item order
    /// </summary>
    private int[] arrowRotation = { 180, 0, 270, 90 };

    //The sprite Component of the QTE button
    private Image button;

    /// <summary>
    /// function used to change the background color of a button.
    /// Called when initialized, activated, deactivated, and on misinput
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        button.color = color;
    }



    // Start is called before the first frame update
    public void Randomize()
    {
        //Get the rotation for our arrow
        Vector3 rotValue = new Vector3(0, 0, arrowRotation[(int)keyName]);

        //Apply that rotation
        ArrowImage.transform.Rotate(-rotValue);

        Initialize();
    }

    // Start is called before the first frame update
    public void Initialize()
    { 
        transform.rotation = Quaternion.Euler(Vector3.zero);

        //Rng between 0 and 3 (inclusive)
        int rand = Random.Range(0, 4);

        //Get the corresponding key name
        keyName = (KeyNames)rand;

        //Get the rotation for our arrow
        Vector3 rotValue = new Vector3(0, 0, arrowRotation[rand]);

        //Apply that rotation
        ArrowImage.transform.Rotate(rotValue);

        //Button set to inactivate on start
        Active = false;

        //Retrieve QTE button's sprite renderer component
        button = GetComponent<Image>();

        //Set it's color to inactive color (gray)
        SetColor(Color.gray);
    }
}
