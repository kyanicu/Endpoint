using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalWindow : MonoBehaviour
{
    public GameObject windowPanel;
    public Image OverlayDarkBG, windowFrame, windowLogo, windowIcon, windowButtonImage, windowLeftArrow, windowRightArrow;
    public TextMeshProUGUI windowTitle, windowTextBody, windowButtonText;
    public Sprite[] windowLogos, windowIcons;

    // Data used by each version of the terminal window.
    // Indices: 0 - Lore, 1 - Paradigm, 2 - Save
    private Color32[] windowColors = {
        new Color32(0x23, 0xde, 0xfb, 0xff),
        new Color32(0xfc, 0x23, 0x8e, 0xff),
        new Color32(0x35, 0xfc, 0x23, 0xff)
    };
    private string[] windowTitles =
    {
        "Lore Entry",
        "Paradigm Unlock",
        "Save Checkpoint"
    };
    private string[] windowTexts =
    {
@"Welcome back, ${USER.NOT_FOUND}.
it has been ${NUL} days since last login.

=======================================

<color=#23defb>recovering uncorrupted entry...</color>
found: 'LORE_NAME'

~>",
@"pinging exodus network...
Last contact: 18554 days ago
last known location: mars +4

=======================================

<color=#fc238e>DOWNLOADING UPDATE PACKAGE...</color>
INSTALL PACKAGE: 'PARADIGM_NAME' ?

~>",
@"Hardened/Kaizen powerrack: smash that data!
Authentication: <color=red>[bypassed]</color>
verifying connection... 100%
server status: full online

=======================================

Welcome, NOT_FOUND.
<color=#35fc23>would you like to backup your data?</color>

~>"
    };
    private string[] windowButtonTexts =
    {
        "SAVE [+100 XP, NEW ENTRY]",
        "INSTALL [UNLOCK PARADIGM]",
        "BACKUP [SAVE GAME]"
    };
    private string windowBodyTextCursor, windowBodyTextNoCursor;

    // Stores information about the current state of the terminal window.
    // Indices: 0 - Lore, 1 - Paradigm, 2 - Save
    private int terminalType;
    // LORE TERMINAL
    private Console c;
    // PARADIGM TERMINAL
    private ParadigmTerminal p;
    // SAVE TERMINAL
    private SaveObject s;

    private static TerminalWindow _instance;
    public static TerminalWindow instance { get { return _instance; } }

    private void Awake()
    {
        //Setup singleton
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // Hide the terminal canvas.
        OverlayDarkBG.GetComponent<CanvasGroup>().alpha = 0.0f;
        windowPanel.GetComponent<CanvasGroup>().alpha = 0.0f;

        windowLogo.GetComponent<CanvasGroup>().alpha = 0.0f;

        windowIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
        windowTitle.GetComponent<CanvasGroup>().alpha = 0.0f;

        windowTextBody.GetComponent<CanvasGroup>().alpha = 0.0f;

        windowButtonImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        windowLeftArrow.GetComponent<CanvasGroup>().alpha = 0.0f;
        windowRightArrow.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    public void openLoreWindow(Console _c)
    {
        // Set up the object to open a lore window.
        c = _c;
        terminalType = 0;

        // Open the window.
        StartCoroutine(openWindow(terminalType));
    }

    public void openParadigmWindow(ParadigmTerminal _p)
    {
        // Set up the object to open a paradigm window.
        p = _p;
        terminalType = 1;

        // Open the window.
        StartCoroutine(openWindow(terminalType));
    }

    public void openSaveWindow(SaveObject _s)
    {
        // Set up the object to open a paradigm window.
        s = _s;
        terminalType = 2;

        // Open the window.
        StartCoroutine(openWindow(terminalType));
    }

    private Tween TWOpenCloseBG, TWOpenCloseWindow, TWFillAmountFrame;
    private Coroutine TWCOpenClose, TWBlinkingCursor;
    private Tween TWWindowLogo, TWWindowIcon, TWWindowTitle, TWWindowTextBody, TWWindowButtonImage, TWWindowLeftArrow, TWWindowRightArrow;

    // windowOpen: Load and open correct terminal window according to given index.
    // Indices are described at beginning of this class.
    private IEnumerator openWindow(int index)
    {
        // Set window colors to given index.
        windowChangeColor(index);

        // Set correct images and text content per given index.
        windowChangeContent(index);

        // If this is a Lore terminal...
        if (index == 0)
        {
            // Run the supplementary content changer.
            windowChangeContentLore(index);
        }
        // If this is a Paradigm terminal...
        if (index == 1)
        {
            // Run the supplementary content changer.
            windowChangeContentParadigm(index);
        }
        // If this is a Save terminal...
        if (index == 2)
        {
            // Run the supplementary content changer.
            windowChangeContentSave(index);
        }

        // Animate window open.
        TWCOpenClose = StartCoroutine(openWindowAnim());

        yield return new WaitForSeconds(0.2f);

        // Set gameplay state to Terminal Window.
        InputManager.instance.currentState = InputManager.InputState.TERMINAL_WINDOW;
    }

    private const float BLINK_TIME = 0.2f;

    // Animates a blinking cursor at the end of the terminal window's text body.
    private IEnumerator blinkingCursor()
    {
        // Show cursor.
        windowTextBody.text = windowBodyTextCursor;

        // Wait a bit.
        yield return new WaitForSeconds(BLINK_TIME);

        // Hide cursor.
        windowTextBody.text = windowBodyTextNoCursor;

        // Wait a bit.
        yield return new WaitForSeconds(BLINK_TIME);

        // Run this function again, recursively.
        TWBlinkingCursor = StartCoroutine(blinkingCursor());
    }

    private IEnumerator openWindowAnim()
    {
        // Start cursor blinking animation.
        windowBodyTextNoCursor = windowTextBody.text;
        windowBodyTextCursor = windowTextBody.text + " _";
        TWBlinkingCursor = StartCoroutine(blinkingCursor());

        // Hide HUD.
        HUDController.instance.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        // Run opening animation for window and bg.
        TWOpenCloseBG = OverlayDarkBG.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        TWOpenCloseWindow = windowPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        yield return new WaitForSeconds(0.1f);
        
        // Run cool animation on frame.
        // Set fill direction.
        windowFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start fill amount tween.
        TWFillAmountFrame = windowFrame.DOFillAmount(1, 0.4f);

        yield return new WaitForSeconds(0.1f);

        TWWindowLogo = windowLogo.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowIcon = windowIcon.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        TWWindowTitle = windowTitle.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowTextBody = windowTextBody.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowButtonImage = windowButtonImage.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        // AFTER 0.4s
        // Reverse fill direction.
        windowFrame.fillOrigin = (int)Image.Origin90.TopLeft;
        // Start reverse fill amount tween.
        TWFillAmountFrame = windowFrame.DOFillAmount(0, 0.4f);

        yield return new WaitForSeconds(0.1f);

        TWWindowLeftArrow = windowLeftArrow.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        TWWindowRightArrow = windowRightArrow.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
    }

    private void windowChangeColor(int index)
    {
        windowTitle.color = windowColors[index];
        windowIcon.color = windowColors[index];
        windowButtonImage.color = windowColors[index];
        windowLeftArrow.color = windowColors[index];
        windowRightArrow.color = windowColors[index];
        windowFrame.color = windowColors[index];
    }

    private void windowChangeContent(int index)
    {
        windowLogo.sprite = windowLogos[index];
        windowIcon.sprite = windowIcons[index];
        windowTitle.text = windowTitles[index];
        windowButtonText.text = windowButtonTexts[index];
    }

    private void windowChangeContentLore(int index)
    {
        // Replace body content, but replace lore name with the actual name of the lore entry.
        windowTextBody.text = windowTexts[index].Replace("LORE_NAME", c.EntryArticle + " >> " + c.EntryName);
    }

    private void windowChangeContentParadigm(int index)
    {
        // Replace body content, but replace lore name with the actual name of the paradigm.
        windowTextBody.text = windowTexts[index].Replace("PARADIGM_NAME", p.paradigmName);
    }

    private void windowChangeContentSave(int index)
    {
        // Replace body content.
        windowTextBody.text = windowTexts[index];
    }

    // buttonClick: Close window and perform correct action, given correct info.
    public void ButtonClick()
    {
        // If this is a Lore window...
        if (terminalType == 0)
        {
            // Run Lore functionality.
            c.RunFunctionality();
        }
        // If this is a Paradigm window...
        else if (terminalType == 1)
        {
            // Run Paradigm functionality.
            p.RunFunctionality();
        }
        // If this is a Save window...
        else if (terminalType == 2)
        {
            // Run Save functionality.
            s.RunFunctionality();
        }

        // Reset input state to gameplay.
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;

        // Animate window close.
        TWCOpenClose = StartCoroutine(closeWindowAnim()); 
    }

    private IEnumerator closeWindowAnim()
    {

        // Run cool animation on frame.
        // Set fill direction.
        windowFrame.fillOrigin = (int)Image.Origin90.TopLeft;
        // Start fill amount tween.
        TWFillAmountFrame = windowFrame.DOFillAmount(1, 0.4f);

        TWWindowLogo = windowLogo.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowIcon = windowIcon.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        TWWindowTitle = windowTitle.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowTextBody = windowTextBody.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        TWWindowButtonImage = windowButtonImage.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        // AFTER 0.4s
        // Reverse fill direction.
        windowFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start reverse fill amount tween.
        TWFillAmountFrame = windowFrame.DOFillAmount(0, 0.4f);

        yield return new WaitForSeconds(0.1f);

        TWWindowLeftArrow = windowLeftArrow.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        TWWindowRightArrow = windowRightArrow.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        // Run closing animation for window and bg.
        TWOpenCloseBG = OverlayDarkBG.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        TWOpenCloseWindow = windowPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.1f);

        // Show HUD.
        HUDController.instance.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

        // End cursor blinking animation.
        StopCoroutine(TWBlinkingCursor);
    }
}
