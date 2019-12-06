using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)

public class SwapPanelManager : MonoBehaviour
{
    #region Swapping
    public TextMeshProUGUI SwappingText;
    public Image SwappingBarLeft, SwappingBarRight, SwappingBarFrame, SwappingBarReset;
    #endregion

    #region Custom Colors
    Color customRed = new Color(.94921875f, .15234375f, .2265625f, 1);
    Color customYellow = new Color(.89453125f, .8359375f, .19140625f, 1);
    Color customGreen = new Color(.35546875f, .89453125f, .19140625f, 1);
    Color customBlue = new Color(.19140625f, .78515625f, .89453125f, 1);

    Color barBlank = new Color(1f, 1f, 1f, 1);
    Color barHighlight = new Color(0.136f, 0.855f, 0.984f, 1);

    Color colorCharacterLight = new Color(.984375f, .65625f, .13671875f, 1);
    Color colorCharacterMedium = new Color(.94921875f, .15234375f, .2265625f, 1);
    // Color characterHeavy = new Color(.94921875f, .15234375f, .2265625f, 1);
    #endregion

    /// <summary>
    /// Updates the swap bar to correctly display swap cooldown
    /// </summary>
    /// <param name="val"></param>
    /// <param name="max"></param>
    public void UpdateSwap(float val)
    {
        SwappingBarLeft.fillAmount = val;
        SwappingBarRight.fillAmount = val;
    }

    /// <summary>
    /// Runs the animation loop for the swapping bars.
    /// </summary>
    /// <param name="rechargeTime"></param>
    public void RechargeSwap(float rechargeTime)
    {
        // Update the Swaping Bars to make sure fill amounts are 0.
        UpdateSwap(0);
        // Run the animation.
        StartCoroutine(AnimationSwappingBars(rechargeTime));
    }

    private IEnumerator AnimationSwappingBars(float rechargeTime)
    {
        float timer = 0;
        float animationTime = 0.2f;

        // Variables to ensure each animation is only triggered once when the timer hits a certain point.
        bool isAnimationShowRunning = false;
        bool isAnimationRefillRunning = false;
        bool isAnimationHideRunning = false;

        float frameWidth = SwappingBarFrame.rectTransform.sizeDelta.x;
        float frameHeight = SwappingBarFrame.rectTransform.sizeDelta.y;

        float barWidth = SwappingBarLeft.rectTransform.sizeDelta.x;
        float barHeight = SwappingBarLeft.rectTransform.sizeDelta.y;

        while (timer < rechargeTime)
        {
            //Check that player is not in a menu
            if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            {
                SwappingBarLeft.DOPause();
                SwappingBarRight.DOPause();
                yield return null;
            }
            else
            {
                SwappingBarLeft.DOPlay();
                SwappingBarRight.DOPlay();

            // Update bar positions according to values set during tweening.
            SwappingBarFrame.rectTransform.sizeDelta = new Vector2(frameWidth, frameHeight);
            SwappingBarLeft.rectTransform.sizeDelta = new Vector2(barWidth, barHeight);
            SwappingBarRight.rectTransform.sizeDelta = new Vector2(barWidth, barHeight);

            // Play the animation to show the Swapping Bars.
            if (isAnimationShowRunning == false)
            {
                SwappingText.color = barBlank;

                SwappingText.text = "Charging";
                DOTween.To(() => frameHeight, x => frameHeight = x, 15f, animationTime);
                DOTween.To(() => barHeight, x => barHeight = x, 13f, animationTime);

                DOTween.To(() => frameWidth, x => frameWidth = x, 393f, animationTime);
                DOTween.To(() => barWidth, x => barWidth = x, 195f, animationTime);

                SwappingBarReset.enabled = false;
                isAnimationShowRunning = true;
            }

            // Play the animation of the Swapping Bars filling up.
            if (isAnimationRefillRunning == false)
            {
                SwappingBarLeft.DOFillAmount(1, rechargeTime);
                SwappingBarRight.DOFillAmount(1, rechargeTime);
                isAnimationRefillRunning = true;
            }

            // Play the animation of the Swapping Bars hiding again.
            if (timer >= rechargeTime - animationTime - 0.2f && isAnimationHideRunning == false)
            {
                SwappingText.color = HUDController.instance.activeClassColor;

                SwappingText.text = "Hack Ready";
                SwappingBarReset.enabled = true;
                DOTween.To(() => frameHeight, x => frameHeight = x, 0, animationTime);
                DOTween.To(() => barHeight, x => barHeight = x, 0, animationTime);

                DOTween.To(() => frameWidth, x => frameWidth = x, 370f, animationTime);
                DOTween.To(() => barWidth, x => barWidth = x, 170f, animationTime);
                isAnimationHideRunning = true;
            }

            // Increment timer and continue loop.
            timer += .05f;
            yield return new WaitForSeconds(.05f);
            }
        }
    }

    // Recolors all elements within the Swap Panel to match the current character's class.
    public void RecolorSwapHUD()
    {
        SwappingBarLeft.color = HUDController.instance.activeClassColor;
        SwappingBarRight.color = HUDController.instance.activeClassColor;
        SwappingBarFrame.color = HUDController.instance.activeClassColor;
        SwappingBarReset.color = HUDController.instance.activeClassColor;
        SwappingText.color = barBlank;
    }
}
