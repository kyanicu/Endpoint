using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    public QTEButton[] buttons; //An array holding the 3 buttons used in the QTE panel
    public Text lastInputText; //Txt displaying last key pressed
    private string lastInputString; //String holding text for last button pressed

    [SerializeField]
    private bool listening = false; //returns whether or not QTE buttons are listening for input

    private int listIndex; //The current index in the QTE panel

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
        StackCreate(true);
        listening = false;
    }

    /// <summary>
    /// Updates txt panel to show last button pressed.
    /// For prototyping purposes only
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(keys[0]))
        {
            lastInputString = "Up Key";
        }
        else if (Input.GetKeyDown(keys[1]))
        {
            lastInputString = "Down Key";
        }
        else if (Input.GetKeyDown(keys[2]))
        {
            lastInputString = "Left Key";
        }
        if (Input.GetKeyDown(keys[3]))
        {
            lastInputString = "Right Key";
        }
        lastInputText.text = lastInputString;
    }

    /// <summary>
    /// This functions creates a stack of QTE buttons of size numItems
    /// </summary>
    /// <param name="numItems"></param>
    public void StackCreate(bool firstTime)
    {
        buttonStack = new List<QTEButton>();
        listIndex = 0;

        //Loop through size of stack
        for (int i = 0; i < startSize; i++)
        {
            //If there is still room to display buttons on panel
            if (i < startSize)
            {
                buttons[i].transform.position = ButtonLoc[i].position;
                if (firstTime)   buttons[i].Initialize();
                else             buttons[i].Randomize();
            }

            //Add the button to our stack
            buttonStack.Add(buttons[i]);
        }
        //Activate our first button
        activateButton();

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

        //begin listening again
        listening = true;
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
                    listening = false;

                    //Change button color to green
                    activeButton.SetColor(Color.green);

                    //Remove it from stack
                    listIndex++;

                    if (listIndex == buttonStack.Count)
                    {
                        successfulHack();
                        yield return null;
                    }

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

                    StackCreate(false);
                }
            }
            yield return new WaitForSeconds(.01f);
        }
        listening = false;

        //end coroutine
        yield return null;
    }

    /// <summary>
    /// Function that gets called after player successfully completes QTE
    /// </summary>
    private void successfulHack() { }
}