using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangeFinder : MonoBehaviour
{

    #region Loading Bar
    //Loading bar depicting how much hack time is left
    public Image LoadingBar, LoadingBarSpin, LoadingFrame, LoadingBG;
    public TextMeshProUGUI LoadingPercent;
    private Tween loadingBarTween;
    public GameObject LoadingBarPanel;
    #endregion

    #region HackDialog
    public Image HackDialogBG, QTEButton1, QTEButton2, QTEButton3, HackDialogChevron1, HackDialogChevron2, HackDialogChevron3;
    #endregion

    //Quick time event panel, activated when in range for long enough
    public QTEManager QTEManager;

    public GameObject QTEPanel;

    //Flag denoting whether player is currently uploading
    private bool hackStart = false;

    // Flags denoting whether these elements are visible.
    // To be used to figure out which fade out animation to play.
    private bool loadingBarActive = false;
    private bool hackDialogActive = false;

    //Time it takes to upload, used in coroutine
    private float uploadTime = 1f;

    //How much fill amount gets increased 
    private const float UPLOAD_BAR_DIVISIONS = 20;

    public int QTEButtonsAmt;

    /// <summary>
    /// Begins uploading upon player entering hack area
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.tag == "Player")
        {
            if (hackStart == false)
            {
                // Animate the loading bar in.
                float fadeTime = 0.1f;

                LoadingBarPanel.gameObject.SetActive(true);
                loadingBarActive = true;
                LoadingBar.enabled = true;

                // Set the loading bar element alphas to 0.
                changeAlphaTo(LoadingBar, 0f);
                changeAlphaTo(LoadingBarSpin, 0f);
                changeAlphaTo(LoadingFrame, 0f);
                changeAlphaTo(LoadingBG, 0f);
                changeAlphaTo(LoadingPercent, 0f);

                // Start loading bar elements alpha tweens to 1.
                LoadingBar.DOFade(1, fadeTime);
                LoadingBarSpin.DOFade(1, fadeTime);
                LoadingFrame.DOFade(1, fadeTime);
                LoadingBG.DOFade(1, fadeTime);
                LoadingPercent.DOFade(1, fadeTime);
            }

            hackStart = true;

            startHackLoadingBar();
            StartCoroutine(Uploading());
        }
    }

    /// <summary>
    /// Halts uploading upon player leaving hack area
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            hackStart = false;

            float fadeTime = 0.1f;
            // Check which element is visible to play appropriate fade out animation.
            if (loadingBarActive)
            {
                // Fade out the loading bar.
                // Set the loading bar element alphas to 1.
                changeAlphaTo(LoadingBar, 1f);
                changeAlphaTo(LoadingBarSpin, 1f);
                changeAlphaTo(LoadingFrame, 1f);
                changeAlphaTo(LoadingBG, 1f);
                changeAlphaTo(LoadingPercent, 1f);

                // Start loading bar elements alpha tweens to 0.
                LoadingBar.DOFade(0, fadeTime);
                LoadingBarSpin.DOFade(0, fadeTime);
                LoadingFrame.DOFade(0, fadeTime);
                LoadingBG.DOFade(0, fadeTime);
                LoadingPercent.DOFade(0, fadeTime);
            } 
            else if (hackDialogActive)
            {
                // Fade out the hack dialog.
                // Set dialog's alpha to 1.
                changeAlphaTo(HackDialogBG, 1f);
                changeAlphaTo(QTEButton1, 1f);
                changeAlphaTo(QTEButton2, 1f);
                changeAlphaTo(QTEButton3, 1f);

                // Start dialog's alpha tween to 0.
                HackDialogBG.DOFade(0, fadeTime);
                QTEButton1.DOFade(0, fadeTime);
                QTEButton2.DOFade(0, fadeTime);
                QTEButton3.DOFade(0, fadeTime);
                HackDialogChevron1.DOFade(0, fadeTime);
                HackDialogChevron2.DOFade(0, fadeTime);
                HackDialogChevron3.DOFade(0, fadeTime);
            }

            resetHackLoadingBar();
            StopCoroutine(Uploading());

            loadingBarActive = false;
            hackDialogActive = false;

            QTEButtonsAmt = QTEManager.getButtonsLeft();
            LoadingBar.fillAmount = 0;
        }
    }

    /// <summary>
    /// Function called by enemy to hide and undo hack progress
    /// </summary>
    public void CancelHack()
    {
        hackStart = false;
        loadingBarActive = false;
        hackDialogActive = false;
        QTEPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activate the qte panel and populate it with new buttons
    /// </summary>
    private void startQTE()
    {
        if (hackStart)
        {
            QTEPanel.gameObject.SetActive(true);
            StartCoroutine(QTEStartAnimation());
        }
    }

    /// <summary>
    /// Swap the loading bar with the hack dialog through an animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator QTEStartAnimation()
    {
        // Define some animation time variables.
        float fadeTime = 0.1f;

        // Set dialog's alpha to 0.
        changeAlphaTo(HackDialogBG, 0f);
        changeAlphaTo(QTEButton1, 0f);
        changeAlphaTo(QTEButton2, 0f);
        changeAlphaTo(QTEButton3, 0f);

        // Set the loading bar element alphas to 1.
        changeAlphaTo(LoadingBar, 1f);
        changeAlphaTo(LoadingBarSpin, 1f);
        changeAlphaTo(LoadingFrame, 1f);
        changeAlphaTo(LoadingBG, 1f);
        changeAlphaTo(LoadingPercent, 1f);

        // Start loading bar elements alpha tweens to 0.
        LoadingBar.DOFade(0, fadeTime);
        LoadingBarSpin.DOFade(0, fadeTime);
        LoadingFrame.DOFade(0, fadeTime);
        LoadingBG.DOFade(0, fadeTime);
        LoadingPercent.DOFade(0, fadeTime);

        // Wait for the fade out tween to finish.
        yield return new WaitForSeconds(fadeTime);

        loadingBarActive = false;

        // Start dialog's alpha tween to 1.
        HackDialogBG.DOFade(1, fadeTime);
        QTEButton1.DOFade(1, fadeTime);
        QTEButton2.DOFade(1, fadeTime);
        QTEButton3.DOFade(1, fadeTime);

        hackDialogActive = true;

        // Run the QTE mechanisms. 
        QTEManager.onActivate(QTEButtonsAmt);
    }

    /// <summary>
    /// Changes the Alpha value of a given element to the given float.
    /// </summary>
    /// <returns></returns>
    private void changeAlphaTo(Image element, float newAlpha)
    {
        Color newColor = element.color;
        newColor.a = newAlpha;
        element.color = newColor;
    }

    /// <summary>
    /// Changes the Alpha value of a given element to the given float. (Text version)
    /// </summary>
    /// <returns></returns>
    private void changeAlphaTo(TextMeshProUGUI element, float newAlpha)
    {
        Color newColor = element.color;
        newColor.a = newAlpha;
        element.color = newColor;
    }

    /// <summary>
    /// Function which triggers the hack dialog QTE once the loading bar is finished.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Uploading()
    {
        StartCoroutine(UpdatePercentage());
        yield return new WaitForSeconds(uploadTime);
        if(hackStart)
        {
            startQTE();
        }
        yield return null;
    }

    /// <summary>
    /// Continues incrementing percentage while player is in hack area.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdatePercentage()
    {
        while (hackStart)
        {
            // Update the percentage text to the proper percentage.
            LoadingPercent.text = (int) (LoadingBar.fillAmount * 100) + "%";

            // Wait a fraction of a second of the upload time before updating bar again
            yield return new WaitForSeconds(uploadTime / UPLOAD_BAR_DIVISIONS);
        }
        StopCoroutine(UpdatePercentage());
        yield return null;
    }

    /// <summary>
    /// Resets the loading bar and associated elements to 0. Cancels the tween of the loading bar fill amount.
    /// </summary>
    /// <returns></returns>
    private void resetHackLoadingBar()
    {
        // Stop the tween.
        loadingBarTween.Kill();

        // Reset the bar fillamount to 0.
        LoadingBar.fillAmount = 0;

        // Reset the percentage to 0.
        LoadingPercent.text = "0%";
    }

    /// <summary>
    /// Triggers the start of the loading bar tween.
    /// </summary>
    /// <returns></returns>
    private void startHackLoadingBar()
    {
        // Start the tween.
        loadingBarTween = LoadingBar.DOFillAmount(1.0f, uploadTime);
    }
}
