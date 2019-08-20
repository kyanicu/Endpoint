using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    //The 3 locations where we'll be populating buttons
    public Transform[] ButtonLoc;

    //Holds the most current active QTE button in the stack
    private QTEButton activeButton;

    //Our stack of QTE buttons
    private Stack<GameObject> buttonStack = new Stack<GameObject>();

    //Keycodes for the key inputs needed for quick time events
    private static KeyCode[] keys =
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    //The QTE button we'll constantly be instantiating
    private GameObject qteButton;

    [SerializeField]
    private float waitTime = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        //Load our QTE button from resource ONCE so we don't have to again
        qteButton = Resources.Load<GameObject>("Prefabs/QTE Button");

        //Create a sample stack of QTE buttons size 3
        StackCreate(3);
    }

    /// <summary>
    /// This functions creates a stack of QTE buttons of size numItems
    /// </summary>
    /// <param name="numItems"></param>
    public void StackCreate(int numItems)
    {
        //Loop through size of stack
        for (int i = 0; i < numItems; i++)
        {
            //Create our button
            GameObject button = Instantiate(qteButton);

            //If there is still room to display buttons on panel
            if (i < 3)
            {
                button.transform.position = ButtonLoc[i].position;
            }

            //Add the button to our stack
            buttonStack.Push(button);
        }

        //Activate our first button
        activateButton();
    }

    /// <summary>
    /// Activates first button in stack and changes color to active color
    /// </summary>
    private void activateButton()
    {

        if (buttonStack.Count == 0) return;

        //Set active button to first button in stack
        activeButton = buttonStack.Peek().GetComponent<QTEButton>();

        //Make sure it is active
        activeButton.Active = true;

        //Set it to active color (white)
        activeButton.SetColor(Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonStack.Count == 0) return;
        
        //If first button in stack is active
        if (activeButton.Active)
        {
            if (Input.anyKey)
            {
                //If correct QTE button is pressed
                if (Input.GetKeyDown(keys[(int)activeButton.keyName]))
                {
                    //Change button color to green
                    activeButton.SetColor(Color.green);

                    //Remove it from stack
                    buttonStack.Pop();

                    //TO DO
                    //We'll probably need to destroy it and move the others up
                    //if there are more than 3 items in our stack

                    //Activate the next button in the stack
                    activateButton();
                }
                else //Misinput
                {
                    //Clear and repopulate stack
                    StartCoroutine(ClearStack());
                }
            }
        }
    }

    /// <summary>
    /// Function responsible for pausing after a misinput.
    /// Will then clear and repopulate stack with new QTE buttons
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClearStack()
    {
        //Set misinput button color to red
        activeButton.SetColor(Color.red);

        //Pause so player knows they done goofed
        yield return new WaitForSeconds(waitTime);

        //Iterate through stack
        for(int i = buttonStack.Count; i >= 0; i--)
        {
            //Pull first item in stack
            GameObject button = buttonStack.Peek();

            //If the button hasn't been destroyed yet
            if (button != null)
            {
                //Destroy first button in stack
                Destroy(button);
            }

            //Remove the button's reference from the stack
            buttonStack.Pop();
        }

        //End coroutine
        yield return null;
    }
}
