using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)

public class MainMenuAnimations : MonoBehaviour
{
    public GameObject MainMenuLogoPanel;

    #region Main Menu Logo Animation
    public Image MainMenuLogoImageMain;
    public Image MainMenuLogoImageMainBase;
    public Image MainMenuLogoImageG1;
    [SerializeField]
    private Sprite[] MainMenuLogoGlitchedImages = { };
    #endregion

    public GameObject TinybitPanel1;
    public GameObject TinybitPanel2;
    public GameObject TinybitPanel3;

    public TextMeshProUGUI TinybitText4;
    public Image TinybitImage5;
    public Image TinybitImage6;
    public Image TinybitImage7;
    public Image TinybitImage8;
    public Image TinybitImage9;
    public Image TinybitImage10;
    public Image TinybitImage11;
    public Image TinybitImage12;

    Color colorHighlightedTinybit = new Color32(0xff, 0x9f, 0x0a, 255);
    Color colorUnhighlightedTinybit = new Color32(0xff, 0xff, 0xff, 255);

    private string[] tinybitsRandomText = { 
        "del NST.EXE~555", 
        "future -PXT", 
        "open DATASET 905",
        "ASSET ===& error",
        "REEF 90024./",
        "destroy tech34-int",
        "_011",
        "_899*",
        "_56624%",
        "COL data text",
        "now fea0--4=",
        "dat project_exodus.dat",
        "run mercuryRising",

    };

    private static MainMenuManager _instance = null;
    public static MainMenuManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainMenuManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(MainMenuManager).Name).AddComponent<MainMenuManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }

    public IEnumerator AnimationMenuLogoGlitchImage(float restPeriod = 0)
    {
        if (restPeriod > 30)
        {
            // Set a lower and upper bound for random times within glitch.
            float lowerTime = 0.05f;
            float upperTime = 0.18f;

            int randomLoopIterations = Random.Range(3, 7);

            for (int i = 0; i < randomLoopIterations; i++)
            {
                // Change the main logo image source.
                int glitchedImageIndex = Random.Range(1, MainMenuLogoGlitchedImages.Length);
                MainMenuLogoImageMain.sprite = MainMenuLogoGlitchedImages[glitchedImageIndex];
                MainMenuLogoImageG1.sprite = MainMenuLogoGlitchedImages[glitchedImageIndex];

                // Change the glitchy secondary images positions to random.
                // Randomize a float for the new position.
                float randomAnimationPositionX = Random.Range(-18.0f, 18.0f);
                float randomAnimationPositionY = Random.Range(-18.0f, 18.0f);

                Vector3 mainMenuLogoImagePosition = MainMenuLogoImageMain.GetComponent<RectTransform>().localPosition;
                float baseXPos = mainMenuLogoImagePosition.x;
                float baseYPos = mainMenuLogoImagePosition.y;

                // Calculate the new X position based on the randomized float.
                Vector3 newRandomPositionG1 = new Vector3(baseXPos + randomAnimationPositionX, baseYPos + randomAnimationPositionY, 0);

                MainMenuLogoImageG1.GetComponent<RectTransform>().localPosition = newRandomPositionG1;

                MainMenuLogoImageG1.enabled = true;
                MainMenuLogoImageMain.enabled = true;
                MainMenuLogoImageMainBase.enabled = false;

                // Run the tinybit animation cycle (the small elements that appear randomly in the logo)
                AnimationTinybitHelper();

                yield return new WaitForSeconds(Random.Range(lowerTime, upperTime));
            }

            MainMenuLogoImageMain.sprite = MainMenuLogoGlitchedImages[0];
            MainMenuLogoImageG1.sprite = MainMenuLogoGlitchedImages[0];

            restPeriod = 0;
        }
        else
        {
            restPeriod++;

            MainMenuLogoImageG1.enabled = false;
            MainMenuLogoImageMain.enabled = false;
            MainMenuLogoImageMainBase.enabled = true;

            yield return new WaitForSeconds(0.1f);
        }

        // Start the next animation cycle.
        StartCoroutine(AnimationMenuLogoGlitchImage(restPeriod));
    }

    public void AnimationTinybitHelper()
    {
        animationRandomizePosition(TinybitPanel1);
        animationRandomizePosition(TinybitPanel2);
        animationRandomizePosition(TinybitPanel3);

        animationRandomizePosition(TinybitText4);

        animationRandomizePosition(TinybitImage5);
        animationRandomizePosition(TinybitImage6);
        animationRandomizePosition(TinybitImage7);
        animationRandomizePosition(TinybitImage8);
        animationRandomizePosition(TinybitImage9);
        animationRandomizePosition(TinybitImage10);
        animationRandomizePosition(TinybitImage11);
        animationRandomizePosition(TinybitImage12);
    }

    public void AnimationTinybitTextHelper()
    {
        StartCoroutine(animationRandomizeText(TinybitPanel1.GetComponentsInChildren<TextMeshProUGUI>()[0], 3));
        StartCoroutine(animationRandomizeText(TinybitPanel2.GetComponentsInChildren<TextMeshProUGUI>()[0], 2));
        StartCoroutine(animationRandomizeText(TinybitPanel3.GetComponentsInChildren<TextMeshProUGUI>()[0], 2));
        StartCoroutine(animationRandomizeText(TinybitText4, 2));
    }

    private IEnumerator animationRandomizeText(TextMeshProUGUI textObj, int numLines)
    {
        // Create a string to store the new text.
        string newText = "";

        // Avoid duplicates to ensure that the newly randomly generated
        int[] alreadyExistInText = new int[numLines];

        // Loop over the number of lines and generate a new string.
        for (int i = 0; i < numLines; i++)
        {
            // Generate a new line.
            int newLine = Random.Range(0, tinybitsRandomText.Length);

            // Avoid duplicate lines by rerunning the random line until we get a line that hasn't been run for this text element yet.
            while (System.Array.IndexOf(alreadyExistInText, newLine) != -1) {
                newLine = Random.Range(0, tinybitsRandomText.Length);
            }

            // Add the line to the new text string.
            newText += tinybitsRandomText[newLine] + "\n";

            // Add this line to the duplicate checker.
            alreadyExistInText[i] = newLine;
        }

        // Set the object's text to the new text string.
        textObj.text = newText;

        // Wait for a random amount of time and start the next animation cycle.
        yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        StartCoroutine(animationRandomizeText(textObj, numLines));
    }

    private void animationRandomizePosition(GameObject obj)
    {
        Vector3 MainMenuLogoPanelPosition = MainMenuLogoPanel.GetComponent<RectTransform>().sizeDelta;

        float newRandomX = Random.Range(0.0f, MainMenuLogoPanelPosition.x) - MainMenuLogoPanelPosition.x / 2;
        float newRandomY = Random.Range(0.0f, MainMenuLogoPanelPosition.y) - MainMenuLogoPanelPosition.y / 2;

        Vector3 newRandomPosition = new Vector3(newRandomX, newRandomY, 0);

        obj.GetComponent<RectTransform>().localPosition = newRandomPosition;

        if (Random.Range(0, 11) < 4)
        {
            obj.transform.GetChild(1).GetComponent<Image>().color = colorHighlightedTinybit;
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorHighlightedTinybit;
        }
        else
        {
            obj.transform.GetChild(1).GetComponent<Image>().color = colorUnhighlightedTinybit;
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorUnhighlightedTinybit;
        }
    }

    private void animationRandomizePosition(Image obj)
    {
        Vector3 MainMenuLogoPanelPosition = MainMenuLogoPanel.GetComponent<RectTransform>().sizeDelta;

        float newRandomX = Random.Range(0.0f, MainMenuLogoPanelPosition.x) - MainMenuLogoPanelPosition.x / 2;
        float newRandomY = Random.Range(0.0f, MainMenuLogoPanelPosition.y) - MainMenuLogoPanelPosition.y / 2;

        Vector3 newRandomPosition = new Vector3(newRandomX, newRandomY, 0);

        obj.GetComponent<RectTransform>().localPosition = newRandomPosition;

        if (Random.Range(0, 11) < 3)
        {
            obj.color = colorHighlightedTinybit;
        } else
        {
            obj.color = colorUnhighlightedTinybit;
        }
    }

    private void animationRandomizePosition(TextMeshProUGUI obj)
    {
        Vector3 MainMenuLogoPanelPosition = MainMenuLogoPanel.GetComponent<RectTransform>().sizeDelta;

        float newRandomX = Random.Range(0.0f, MainMenuLogoPanelPosition.x) - MainMenuLogoPanelPosition.x / 2;
        float newRandomY = Random.Range(0.0f, MainMenuLogoPanelPosition.y) - MainMenuLogoPanelPosition.y / 2;

        Vector3 newRandomPosition = new Vector3(newRandomX, newRandomY, 0);

        obj.GetComponent<RectTransform>().localPosition = newRandomPosition;
    }
}
