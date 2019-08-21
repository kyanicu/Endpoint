using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public QTEButton[] buttons;

    [SerializeField]
    private bool listening = false;

    private int listIndex = 0;

    //The startSize locations where we'll be populating buttons
    public Transform[] ButtonLoc;

    //Holds the most current active QTE button in the stack
    private QTEButton activeButton;

    //Our stack of QTE buttons
    private List<QTEButton> buttonStack = new List<QTEButton>();

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

    public int startSize;

    [SerializeField]
    private float waitTime = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        //Load our QTE button from resource ONCE so we don't have to again
        qteButton = Resources.Load<GameObject>("Prefabs/QTE Button");

        //Create a sample stack of QTE buttons size startSize
        StackCreate(startSize);
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
            //If there is still room to display buttons on panel
            if (i < startSize)
            {
                buttons[i].transform.position = ButtonLoc[i].position;
                buttons[i].Initialize();
            }

            //Add the button to our stack
            buttonStack.Add(buttons[i]);
        }

        //Activate our first button
        activateButton();

        listening = true;

        StartCoroutine(Listener());
    }

    /// <summary>
    /// Activates first button in stack and changes color to active color
    /// </summary>
    private void activateButton()
    {

        if (listIndex == buttonStack.Count) return;

        //Set active button to first button in stack
        activeButton = buttonStack[listIndex];

        //Make sure it is active
        activeButton.Active = true;

        //Set it to active color (white)
        activeButton.SetColor(Color.white);
    }

    // Update is called once per frame
    private IEnumerator Listener()
    {
        if (buttonStack.Count == 0) yield return null;
        
        //If first button in stack is active
        while (activeButton.Active && listIndex < buttonStack.Count)
        {
            if (!listening) break;

            if (Input.GetKeyDown(keys[0]) || Input.GetKeyDown(keys[1]) || Input.GetKeyDown(keys[2]) || Input.GetKeyDown(keys[startSize]))
            {
                //If correct QTE button is pressed
                if (Input.GetKeyDown(keys[(int)activeButton.keyName]))
                {
                    //Change button color to green
                    activeButton.SetColor(Color.green);

                    //Remove it from stack
                    listIndex++;

                    if (listIndex == buttonStack.Count)   successfulHack();

                    //TO DO
                    //We'll probably need to destroy it and move the others up
                    //if there are more than startSize items in our stack

                    yield return new WaitForSeconds(.01f);

                    //Activate the next button in the stack
                    activateButton();
                }
                else //Misinput
                {
                    listening = false;
                    //Set misinput button color to red
                    activeButton.SetColor(Color.red);

                    //Pause so player knows they done goofed
                    yield return new WaitForSeconds(waitTime);

                    //Iterate through stack
                    foreach (QTEButton button in buttonStack)
                    {
                        Destroy(button);
                    }

                    buttonStack = new List<QTEButton>();
                    listIndex = 0;
                    StackCreate(startSize);
                }
            }
            yield return new WaitForSeconds(.01f);
        }

        //end coroutine
        yield return null;
    }

    private void successfulHack() { }

}