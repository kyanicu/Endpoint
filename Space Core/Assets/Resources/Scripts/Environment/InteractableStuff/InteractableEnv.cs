using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableEnv : MonoBehaviour
{
    #region Attached Button Object
    public GameObject ButtonToPress;
    private Image buttonImage;
    public bool hidden { get; protected set; }
    protected bool isAnimatingButton;
    protected RectTransform rectTransform;
    protected const float distToMove = 1f;
    #endregion

    protected void Awake()
    {
        //Retrieve attached components
        buttonImage = ButtonToPress.GetComponent<Image>();
        rectTransform = ButtonToPress.GetComponent<RectTransform>();
        hidden = true;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.gameObject.CompareTag("Player"))
        {
            //If button is hidden and currently not being animated, animate it
            if (hidden && !isAnimatingButton)
            {
                col.gameObject.GetComponent<Player>().InteractableObject = this;
                hidden = false;
                StartCoroutine(animateButton(true));
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //If button is hidden and currently not being animated, animate it
            if (hidden && !isAnimatingButton)
            {
                col.gameObject.GetComponent<Player>().InteractableObject = this;
                hidden = false;
                StartCoroutine(animateButton(true));
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D col)
    {
        col.gameObject.GetComponent<Player>().InteractableObject = null;
        if (col.gameObject.CompareTag("Player"))
        {
            //If button is visible and currently not being animated, animate it
            if (!hidden && !isAnimatingButton)
            {
                hidden = true;
                StartCoroutine(animateButton(false));
            }
        }
    }

    public abstract void ActivateFunctionality();

    /// <summary>
    /// Coroutine responsible for animating the button's position and color alpha
    /// </summary>
    /// <param name="unhide"></param>
    /// <returns></returns>
    protected IEnumerator animateButton(bool unhide)
    {
        //Check if we should be animating button
        if (!isAnimatingButton)
        {
            isAnimatingButton = true;
            float counter = 0;
            float countGoal = unhide ? distToMove : -distToMove;
            float step = unhide ? .1f : -.1f;
            
            //Continue looping until button has moved certain distance
            while(Mathf.Abs(counter) < Mathf.Abs(countGoal))
            {
                counter += step;
                rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, 
                                                          rectTransform.localPosition.y + step);
                Color c = buttonImage.color;
                c.a += step;
                buttonImage.color = c;
                yield return null;
            }
            isAnimatingButton = false;
            yield return null;
        }
    }

}
