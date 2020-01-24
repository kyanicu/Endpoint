using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Screen Components")]
    public Image LoadingScreenPanel;
    public Image LoadingScreenImage;
    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI LoadingTipText;

    private string[] LoadingScreenTips;
    private Image[] LoadingScreenImages;

    // Start is called before the first frame update
    void Awake()
    {
        InputManager.instance.currentState = InputManager.InputState.LOADING;
        LoadingText.text = "LOADING";
        StartCoroutine(FadeOutLoadingScreen());

        TextAsset tipsFile = Resources.Load<TextAsset>("Text/LoadingScreenTips");
        LoadingScreenTips = tipsFile.text.Split("\n"[0]);
        LoadingTipText.text = RetrieveRandomTip();
        //LoadingScreenImage = RetrieveRandomImage();
        LoadingScreenImages = Resources.LoadAll<Image>("Images/LoadingScreenImages");
    }

    /// <summary>
    /// Retrieves a random tip from the LoadingScreenTips file
    /// </summary>
    /// <returns></returns>
    private string RetrieveRandomTip()
    {
        int start = 0;
        int end = LoadingScreenTips.Length;
        int index = Random.Range(start, end);
        return LoadingScreenTips[index];
    }

    /// <summary>
    /// Retrieves a random image from LoadingScreenImages folder to populate loading screen with
    /// </summary>
    /// <returns></returns>
    private Image RetrieveRandomImage()
    {
        int start = 0;
        int end = LoadingScreenImages.Length;
        int index = Random.Range(start, end);
        return LoadingScreenImages[index];
    }

    /// <summary>
    /// Coroutine for fading out loading screen on scene start
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutLoadingScreen()
    {
        for (int x = 0; x < 3; x++)
        {
            yield return new WaitForSeconds(.5f);
            LoadingText.text += ".";
        }
        Destroy(LoadingText, .25f);
        float timer = 1f;
        float counter = 0;
        while (counter < timer)
        {
            counter += .025f;
            Color updatedAlpa = LoadingScreenPanel.color;
            updatedAlpa.a = timer - counter;
            LoadingScreenPanel.color = updatedAlpa;
            LoadingScreenImage.color = updatedAlpa;
            yield return null;
        }
        gameObject.SetActive(false);
        HUDController.instance.visible = true;
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
    }
}
