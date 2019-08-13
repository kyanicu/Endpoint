using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    private KeyCode[] keys =
{
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };
    private GameObject qteButton;
    private Stack<GameObject> buttonStack = new Stack<GameObject>();
    public Transform[] ButtonLoc;
    private QTEButton activeButton;
    // Start is called before the first frame update
    void Start()
    {
        qteButton = Resources.Load<GameObject>("Prefabs/QTE Button");
        StackPush(3);
    }
    
    public void StackPush(int numItems)
    {
        for (int i = 0; i < numItems; i++)
        {
            GameObject button = Instantiate(qteButton);
            if (i < 3)
            {
                button.transform.position = ButtonLoc[i].position;
            }
            buttonStack.Push(button);
        }
        ActivateButton();
    }

    /// <summary>
    /// Activates first button in stack and changes color to active color
    /// </summary>
    private void ActivateButton()
    {
        activeButton = buttonStack.Peek().GetComponent<QTEButton>();
        activeButton.active = true;
        activeButton.setColor(Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeButton.active)
        {
            if (Input.GetKeyDown(keys[(int)activeButton.keyName]))
            {
                activeButton.setColor(Color.green);
                buttonStack.Pop();
                ActivateButton();
            }
            else
            {
                StartCoroutine(ClearStack());
            }
        }
    }

    private IEnumerator ClearStack()
    {
        activeButton.setColor(Color.red);
        yield return new WaitForSeconds(2);
        for(int i = buttonStack.Count; i >= 0; i--)
        {
            GameObject button = buttonStack.Peek();
            if (button != null)
            {
                Destroy(button);
            }
            buttonStack.Pop();
        }
        yield return null;
    }
}
