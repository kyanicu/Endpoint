using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    //An array holding the 3 buttons used in the QTE panel
    public QTEButton[] buttons;

    // Array containing the chevrons next to the hack buttons
    public Image[] HackDialogChevrons;

    //returns whether or not QTE buttons are listening for input
    private bool listening = false;

    //The current index in the QTE panel
    private int listIndex; 

    //Holds the most current active QTE button in the stack
    private QTEButton activeButton;

    //Our stack of QTE buttons
    private List<QTEButton> buttonStack = new List<QTEButton>();

    //How many buttons we'll be generating
    public int ListSize;

    public bool InstantHack;

    //Amount of time it takes to start the hack
    public float WaitTime = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        //Loop through space on panel
        for (int i = 0; i < 3; i++)
        {
            buttons[i].Initialize();
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        listening = false;
        listIndex = 0;
        ListSize = 3;
        stackCreate();
    }

    /// <summary>
    /// Return amount of QTE buttons left to press
    /// </summary>
    /// <returns></returns>
    public int getButtonsLeft()
    {
        return ListSize - listIndex;
    }

    /// <summary>
    /// This functions creates a stack of QTE buttons of size numItems
    /// </summary>
    /// <param name="numItems"></param>
    private void stackCreate()
    {
        for (int i = 0; i < 3; i++)
        {
            buttons[i].gameObject.SetActive(false);
            HackDialogChevrons[i].color = new Color32(0xff, 0xff, 0xff, 0x00);
        }
        buttonStack = new List<QTEButton>();

        //Loop through space on panel
        for (int i = 0; i < 3; i++)
        {
            //No buttons left to generate
            if (i >= ListSize - listIndex)   break;

            //Initialize a new random button
            buttons[i].gameObject.SetActive(true);
            buttons[i].Randomize();

            //Add the button to our stack
            buttonStack.Add(buttons[i]);
        }
        //Activate our first button
        activateButton();

        //Start listening for input
        StartCoroutine(Listener());
    }

    /// <summary>
    /// Activates first button in stack and changes color to active color
    /// </summary>
    private void activateButton()
    {
        //Set active button to first button in stack
        activeButton = buttonStack[listIndex % 3];

        //Make sure it is active
        activeButton.Active = true;

        // Set the matching chevron to visible and white.
        HackDialogChevrons[listIndex % 3].color = new Color32(0xff, 0xff, 0xff, 0xff);

        //begin listening again
        listening = true;
    }

    // Update is called once per frame
    private IEnumerator Listener()
    {
        if (buttonStack.Count == 0 || listIndex == ListSize) yield return null;
        
        //If first button in stack is active
        while (activeButton.Active && listIndex % 3 < buttonStack.Count)
        {
            if (!listening) break;

            QTEButton.KeyNames? key = InputManager.instance.CheckQTEButtonPress();

            if (key != null)
            {
                //If correct QTE button is pressed
                if (key == activeButton.keyName)
                {
                    listening = false;
                    yield return null;

                    // Set the matching chevron to green.
                    HackDialogChevrons[listIndex % 3].color = Color.green;

                    //Remove it from stack
                    listIndex++;

                    //If player has completed all QTE buttons in panel
                    if (listIndex == ListSize)
                    {
                        listening = false;
                        successfulHack();
                        break;
                    }
                    //If player has no more QTE buttons but listIndex isn't at last button
                    //Create a new set of QTE buttons
                    if (listIndex != 0 && listIndex % 3 == 0)
                    {
                        stackCreate();
                        yield return null;
                    }

                    yield return null;

                    //Activate the next button in the stack
                    activateButton();
                }
                else //Misinput
                {
                    listening = false;

                    // Set the matching chevron to red.
                    HackDialogChevrons[listIndex % 3].color = Color.red;

                    //Update ListIndex, player must reset set of QTEs
                    listIndex -= (listIndex % 3);

                    //Pause so player knows they done goofed
                    yield return new WaitForSeconds(WaitTime);

                    stackCreate();
                }
            }
            yield return null;
        }
        listening = false;

        //end coroutine
        yield return null;
    }

    /// <summary>
    /// Function that gets called after player successfully completes QTE
    /// Upon completion, switches Player and Enemy bodies
    /// </summary>
    public void successfulHack()
    {
        if (listIndex == buttonStack.Count || InstantHack)
        {
            StopCoroutine(Listener());
            listening = false;
            PlayerController.instance.Switch();
        }
    }
}