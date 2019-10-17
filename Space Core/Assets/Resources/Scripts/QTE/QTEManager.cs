﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    //An array holding the 3 buttons used in the QTE panel
    public QTEButton[] buttons;

    //String holding text for last button pressed
    private string lastInputString;

    //returns whether or not QTE buttons are listening for input
    private bool listening = false;

    //The current index in the QTE panel
    private int listIndex; 

    //Holds the most current active QTE button in the stack
    private QTEButton activeButton;

    //Our stack of QTE buttons
    private List<QTEButton> buttonStack = new List<QTEButton>();

    public GameObject Player;    

    //How many buttons we'll be generating
    public int listSize { get; private set; }

    public float WaitTime = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        listening = false;
        listIndex = 0;
        //Loop through space on panel
        for (int i = 0; i < 4; i++)
        {
            buttons[i].Initialize();
            buttons[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public int getButtonsLeft()
    {
        return listSize - listIndex;
    }

    public void onActivate(int size)
    {
        if (size == 0)
        {
            successfulHack();
            return;
        }
        listening = false;
        listIndex = 0;
        listSize = size;
        stackCreate();
    }

    /// <summary>
    /// This functions creates a stack of QTE buttons of size numItems
    /// </summary>
    /// <param name="numItems"></param>
    private void stackCreate()
    {
        for (int i = 0; i < 4; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        buttonStack = new List<QTEButton>();

        //Loop through space on panel
        for (int i = 0; i < 4; i++)
        {
            //No buttons left to generate
            if (i >= listSize - listIndex)   break;

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
        activeButton = buttonStack[listIndex % 4];

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
        if (buttonStack.Count == 0 || listIndex == listSize) yield return null;
        
        //If first button in stack is active
        while (activeButton.Active && listIndex % 4 < buttonStack.Count)
        {
            if (!listening) break;

            QTEButton.KeyNames? key = InputController.instance.CheckQTEButtonPress();

            if (key != null)
            {
                //If correct QTE button is pressed
                if (key == activeButton.keyName)
                {
                    listening = false;
                    yield return new WaitForSeconds(.01f);

                    //Change button color to green
                    activeButton.SetColor(Color.green);

                    //Remove it from stack
                    listIndex++;

                    //If player has completed all QTE buttons in panel
                    if (listIndex == listSize)
                    {
                        listening = false;
                        successfulHack();
                        break;
                    }
                    //If player has no more QTE buttons but listIndex isn't at last button
                    //Create a new set of QTE buttons
                    if (listIndex != 0 && listIndex % 4 == 0)
                    {
                        stackCreate();
                        yield return null;
                    }

                    yield return new WaitForSeconds(.01f);

                    //Activate the next button in the stack
                    activateButton();
                }
                else //Misinput
                {
                    listening = false;
                    //Set misinput button color to red
                    activeButton.SetColor(Color.red);

                    //Update ListIndex, player must reset set of QTEs
                    listIndex -= (listIndex % 4);

                    //Pause so player knows they done goofed
                    yield return new WaitForSeconds(WaitTime);

                    stackCreate();
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
    /// Upon completion, switches Player and Enemy bodies
    /// </summary>
    private void successfulHack()
    {
        StopCoroutine(Listener());
        Player.GetComponent<PlayerBehavior>().Switch();
    }

    private static QTEManager _instance = null;

    public static QTEManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QTEManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(QTEManager).Name).AddComponent<QTEManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }
}