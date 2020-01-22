using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StashedBootupAnimation : MonoBehaviour
{
    public Image OverlayAnimBG, OverlayAnimFrame, OverlayAnimLogo, OverlayAnimProgressFrame, OverlayAnimProgressBar, OverlayAnimTitleBack;
    public TextMeshProUGUI OverlayAnimTitle, OverlayAnimChangingDesc, OverlayAnimChangingLinesRight, OverlayAnimChangingLinesLeft;

    #region OverlaySYMBIOSScreenOpenCloseTweens
    // Stores tweens.
    private Tween DPSymbiosBGFade, DPSymbiosFrameFade;
    #endregion
    /*
    private IEnumerator OpenSYMBIOSWindow(float animTimeScale)
    {
        // Fade in the SYMBIOS window BG.
        DPSymbiosBGFade = OverlayAnimBG.DOFade(1f, 0.2f * animTimeScale);
        DPSymbiosFrameFade = OverlayAnimFrame.DOFade(1f, 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Blink in the title.
        OverlayAnimTitle.DOFade(1f, 0.05f * animTimeScale);
        OverlayAnimTitleBack.DOFade(1f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Blink in the logo.
        OverlayAnimLogo.DOFade(1f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Blink in the progress bar/frame.
        OverlayAnimProgressFrame.DOFade(1f, 0.05f * animTimeScale);
        OverlayAnimProgressBar.DOFade(1f, 0.05f * animTimeScale);

        OverlayAnimChangingDesc.DOFade(1f, 0.05f * animTimeScale);
        OverlayAnimChangingLinesRight.DOFade(1f, 0.05f * animTimeScale);
        OverlayAnimChangingLinesLeft.DOFade(1f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Trigger progress bar fillamount.
        OverlayAnimProgressBar.DOFillAmount(1f, 0.2f * animTimeScale);

        // TODO: Stutter the fillamount.
        // Find random number between 2 and 5.
        // Divide 100 by random number.
        // Iterate over random number. Find another random value for each one between -5 and 5.

        OverlayAnimChangingLinesRight.text = "omniplatform operating system (OPOS)";
        OverlayAnimChangingLinesLeft.text = "omniplatform operating system (OPOS)";

        OverlayAnimChangingDesc.text = "Accessing devices...";
        yield return new WaitForSeconds(0.066f * animTimeScale);

        OverlayAnimChangingLinesRight.text = "omniplatform operating system (OPOS)\nDeveloped for use on any standard iot device";
        OverlayAnimChangingLinesLeft.text = "omniplatform operating system (OPOS)\nDeveloped for use on any standard iot device";
        OverlayAnimChangingDesc.text = "Initializing core components...";
        yield return new WaitForSeconds(0.066f * animTimeScale);

        OverlayAnimChangingLinesRight.text = "omniplatform operating system (OPOS)\nDeveloped for use on any standard iot device\nVER 3.5";
        OverlayAnimChangingLinesLeft.text = "omniplatform operating system (OPOS)\nDeveloped for use on any standard iot device\nVER 3.5";
        OverlayAnimChangingDesc.text = "Starting processes...";
        yield return new WaitForSeconds(0.066f * animTimeScale);

        // Blink out the title.
        OverlayAnimTitle.DOFade(0f, 0.05f * animTimeScale);
        OverlayAnimTitleBack.DOFade(0f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Blink out the logo.
        OverlayAnimLogo.DOFade(0f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Blink out the progress bar/frame.
        OverlayAnimProgressFrame.DOFade(0f, 0.05f * animTimeScale);
        OverlayAnimProgressBar.DOFade(0f, 0.05f * animTimeScale);

        OverlayAnimChangingDesc.DOFade(0f, 0.05f * animTimeScale);
        OverlayAnimChangingLinesRight.DOFade(0f, 0.05f * animTimeScale);
        OverlayAnimChangingLinesLeft.DOFade(0f, 0.05f * animTimeScale);

        // Fade out the SYMBIOS window BG.
        DPSymbiosBGFade = OverlayAnimBG.DOFade(0f, 0.05f * animTimeScale);
        DPSymbiosFrameFade = OverlayAnimFrame.DOFade(0f, 0.05f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        OverlayAnimProgressBar.fillAmount = 0f;
        OverlayAnimChangingLinesRight.text = "";
        OverlayAnimChangingLinesLeft.text = "";

        OverlayAnimChangingDesc.text = "";
    }
    */
}
