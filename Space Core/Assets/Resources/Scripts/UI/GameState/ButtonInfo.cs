using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    [Tooltip("Highlighted (Selected) Image")]
    public Sprite Selected;
    [Tooltip("Normal (Deselected) Image")]
    public Sprite Deselected;

    private Image buttonImage;
    private Button button;
    private bool selected;

    // Start is called before the first frame update
    void Awake()
    {
        //Retrieve our button and button image component
        //so we only need to do it once
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
    }

    public void SwapSelect()
    {
        // Deselect this button
        if (!selected)
        {
            buttonImage.sprite = Selected;
        }
        // Select this button
        else if (selected)
        {
            //Invoke the button to update the main panel
            button.onClick.Invoke();
            buttonImage.sprite = Deselected;
        }

        //Swap value of selected
        selected = !selected;
    }
}
