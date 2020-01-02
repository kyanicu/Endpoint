using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEButton : MonoBehaviour
{
    //QTE button's current status
    public bool Active;

    //The UI controller buttons to be loaded onto QTE button
    public Sprite[] ABXYButtons;

    //The sprite Renderer for the attached image, displays ABXY
    public Image ButtonImage;

    //Enums for possible QTE button presses
    public enum KeyNames { A, B, X, Y };

    //This button's needed QTE key
    public KeyNames keyName { get; private set; }

    //The sprite Component of the QTE button
    private SpriteRenderer button;

    /// <summary>
    /// function used to change the background color of a button.
    /// Called when initialized, activated, deactivated, and on misinput
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        button.color = color;
    }

    public void Randomize()
    {
        //Rng between 0 and 3 (inclusive)
        int rand = Random.Range(0, 4);

        //Attach new ui sprite to button
        ButtonImage.sprite = ABXYButtons[rand];

        //Get the corresponding key name
        keyName = (KeyNames)rand;
    }

    // Start is called before the first frame update
    public void Initialize()
    { 
        transform.rotation = Quaternion.Euler(Vector3.zero);

        Randomize();

        //Button set to inactivate on start
        Active = false;

        //Retrieve QTE button's sprite renderer component
        button = GetComponent<SpriteRenderer>();

        //Set it's color to inactive color (gray)
        SetColor(Color.gray);
    }
}
