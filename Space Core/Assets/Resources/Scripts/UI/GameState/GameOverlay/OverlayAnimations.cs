using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayAnimations : MonoBehaviour
{

    private static OverlayAnimations _instance;
    public static OverlayAnimations instance { get { return _instance; } }

    OverlayManager OverlayManager;

    private Color OverlayHighlightColor;
    private Color[] OverlayHighlightColors = {
        new Color32(0xFD, 0xC7, 0x0A, 0xff),
        new Color32(0x23, 0xDB, 0xFC, 0xff),
        new Color32(0x0A, 0xFD, 0x77, 0xff),
        new Color32(0xF4, 0x0A, 0xFD, 0xff)
    };

    #region OverlayScreenOpenClose
    // Define a time scale for this animation, to easily shorten or lengthen it.
    private float overlayAnimTimeScale = 1f;
    // Stores tween for general panel fades.
    private Tween DPDarkBGFade, DPTopPanelFade, DPNavPanelFade, DPBottomPanelFade;
    // Slide top and bottom bars.
    private Tween DPSlideTopBar, DPSlideBottomBar;
    Vector3 topPanelOriginalDimensions, bottomPanelOriginalDimensions;
    // Coroutines for overarching sub-animations.
    private Coroutine OverlayBGAnimation, FrostedGlassFade;
    // Original dimensions for bg block squares.
    private Vector2[] bgBlockOriginalDimensions = new Vector2[5];
    #endregion

    private void Awake()
    {
        //Setup singleton
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }
    }

    public void OverlayAnimationsInit()
    {
        OverlayManager = OverlayManager.instance;
        // Grab original locations for objects that will change position.
        // Bottom and top panels
        topPanelOriginalDimensions = OverlayManager.OverlayTopPanel.GetComponent<RectTransform>().anchoredPosition;
        bottomPanelOriginalDimensions = OverlayManager.OverlayBottomPanel.GetComponent<RectTransform>().anchoredPosition;
        // BG blocks
        for (int i = 0; i < 5; i++)
        {
            bgBlockOriginalDimensions[i] = OverlayManager.OverlayBGAnimBlocks[i].GetComponent<RectTransform>().sizeDelta;
        }

        // Set a temporary starting highlight.
        OverlayHighlightColor = new Color32(0xFD, 0xC7, 0x0A, 0xff);

        // Set up the original positions and calculated positions for each of the primary panels, to prepare them for the transition animations.
        //OverlayNavigateBaseLeftPosition, OverlayNavigateBaseOriginalPosition, OverlayNavigateBaseRightPosition
        for (int i = 0; i < 4; i++)
        {
            OverlayNavigateBaseOriginalPosition[i] = OverlayManager.OverlayPanels[i].GetComponent<RectTransform>().anchoredPosition;
            OverlayNavigateBaseLeftPosition[i] = new Vector3(OverlayNavigateBaseOriginalPosition[i].x - 90f, OverlayNavigateBaseOriginalPosition[i].y, OverlayNavigateBaseOriginalPosition[i].z);
            OverlayNavigateBaseRightPosition[i] = new Vector3(OverlayNavigateBaseOriginalPosition[i].x + 90f, OverlayNavigateBaseOriginalPosition[i].y, OverlayNavigateBaseOriginalPosition[i].z);
        }
    }

    public IEnumerator OpenOverlayAnimation()
    {
        float animTimeScale = overlayAnimTimeScale;

        // Enable the overlay.
        OverlayManager.overlay.gameObject.SetActive(true);

        // Slow fade the dark background in.
        DPDarkBGFade = OverlayManager.DarkBGPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.4f * animTimeScale);

        // Trigger the Frosted Glass fade.
        if (FrostedGlassFade != null)
        {
            StopCoroutine(FrostedGlassFade);
        }
        FrostedGlassFade = StartCoroutine(OpenFrostedGlass(OverlayManager.FrostedGlassBlurGameplay.GetComponent<Image>(), animTimeScale));

        // *** Trigger the BG Blocks animations
        // Check if the coroutine exists. Delete it if it does.
        if (OverlayBGAnimation != null)
        {
            StopCoroutine(OverlayBGAnimation);
        }
        // Delete any continuous animation coroutines if they exist.
        for (int i = 0; i < 5; i++)
        {
            if (OverlayBGBlockRoutines[i] != null)
            {
                StopCoroutine(OverlayBGBlockRoutines[i]);
            }
        }
        // Start this opening coroutine.
        OverlayBGAnimation = StartCoroutine(OpenBGAnim(animTimeScale));

        // Hide the HUD.

        // Slide top bar in from top.
        // Start animation to slide the panel into its original location.
        DPSlideTopBar = OverlayManager.OverlayTopPanel.GetComponent<RectTransform>().DOAnchorPosY(topPanelOriginalDimensions.y, 0.2f * animTimeScale);

        // Slide bottom bar in from bottom.
        // Start animation to slide the panel into its original location.
        DPSlideBottomBar = OverlayManager.OverlayBottomPanel.GetComponent<RectTransform>().DOAnchorPosY(bottomPanelOriginalDimensions.y, 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Fade in the top and bottom panels.
        DPTopPanelFade = OverlayManager.OverlayTopPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.1f * animTimeScale);
        DPBottomPanelFade = OverlayManager.OverlayBottomPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.1f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        OverlayManager.TopPanelClock.DOFade(1f, 0.1f * animTimeScale);

        // Fade in the nav buttons one by one.
        OverlayManager.NavPanelMapButton.DOFade(1f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);

        OverlayManager.TopPanelProcessesText.DOFade(1f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC1.DOFade(1f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC2.DOFade(1f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC3.DOFade(1f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC4.DOFade(1f, 0.1f * animTimeScale);
        OverlayManager.TopPanelSymbiosLogo.DOFade(1f, 0.1f * animTimeScale);

        OverlayManager.NavPanelObjectivesButton.DOFade(1f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
        OverlayManager.NavPanelUpgradesButton.DOFade(1f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
        OverlayManager.NavPanelDatabaseButton.DOFade(1f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
    }

    // Function which increases the blur of the frosted glass behind the overlay IN (by modifying the shader details).
    private IEnumerator OpenFrostedGlass(Image el, float animTimeScale)
    {
        float animDuration = 0.5f;
        float radius = el.material.GetFloat("_Radius");
        DOTween.To(() => radius, x => radius = x, 4f, animDuration * animTimeScale);
        float counter = 0f;
        while (counter < animDuration)
        {
            el.material.SetFloat("_Radius", radius);
            counter += animDuration / 5f;
            yield return new WaitForSeconds(animDuration / 5f * animTimeScale);
        }
    }

    // Function which turns off the blur of the frosted glass behind the overlay OUT (by modifying the shader details).
    private IEnumerator CloseFrostedGlass(Image el, float animTimeScale)
    {
        float animDuration = 0.5f;
        float radius = el.material.GetFloat("_Radius");
        DOTween.To(() => radius, x => radius = x, 0f, animDuration * animTimeScale);
        float counter = 0f;
        while (counter < animDuration)
        {
            el.material.SetFloat("_Radius", radius);
            counter += animDuration / 10f;
            yield return new WaitForSeconds(animDuration / 10f * animTimeScale);
        }
    }

    private Coroutine[] OverlayBGBlockRoutines = new Coroutine[5];

    private IEnumerator OpenBGAnim(float animTimeScale)
    {
        // For each background block, do the presetting actions.
        for (int i = 0; i < OverlayManager.OverlayBGAnimBlocks.Length; i++)
        {
            // Randomize its position.
            animationRandomizePosition(OverlayManager.OverlayBGAnimBlocks[i]);
            // Set its color to highlight.
            OverlayManager.OverlayBGAnimBlocks[i].color = OverlayHighlightColor;
            OverlayManager.OverlayBGAnimBlocks[i].DOFade(0f, 0f);
        }

        // For each background block, do the delayed actions (animations).
        for (int i = 0; i < OverlayManager.OverlayBGAnimBlocks.Length; i++)
        {
            // Fade the block in.
            OverlayManager.OverlayBGAnimBlocks[i].DOFade(1f, 0.2f * animTimeScale);
            // Fade the scale from 0.8 to 1 size.
            Vector2 bgBlockStartDimensions = bgBlockOriginalDimensions[i] * new Vector2(1.2f, 1.2f);
            OverlayManager.OverlayBGAnimBlocks[i].GetComponent<RectTransform>().DOSizeDelta(bgBlockStartDimensions, 0.2f * animTimeScale);
            // Trigger the background animation for this block.
            if (OverlayBGBlockRoutines[i] != null)
            {
                StopCoroutine(OverlayBGBlockRoutines[i]);
            }
            OverlayBGBlockRoutines[i] = StartCoroutine(AnimationBGBlockScale(OverlayManager.OverlayBGAnimBlocks[i], i));
            // Wait for a short amount of time to stagger the block animations.
            yield return new WaitForSeconds(0.05f * animTimeScale);
        }
    }

    private IEnumerator CloseBGAnim(float animTimeScale)
    {
        // For each background block, do the delayed actions (animations).
        for (int i = 0; i < OverlayManager.OverlayBGAnimBlocks.Length; i++)
        {
            // Cancel the background animation for this block.
            if (OverlayBGBlockRoutines[i] != null)
            {
                StopCoroutine(OverlayBGBlockRoutines[i]);
            }
            // Fade the block out.
            OverlayManager.OverlayBGAnimBlocks[i].DOFade(0f, 0.2f * animTimeScale);
            // Fade the scale from 1 to 0.8 size.
            Vector2 bgBlockStartDimensions = bgBlockOriginalDimensions[i] * new Vector2(0.8f, 0.8f);
            OverlayManager.OverlayBGAnimBlocks[i].GetComponent<RectTransform>().DOSizeDelta(bgBlockStartDimensions, 0.2f * animTimeScale);
            // Wait for a short amount of time to stagger the block animations.
            yield return new WaitForSeconds(0.05f * animTimeScale);
        }
    }

    private IEnumerator AnimationBGBlockScale(Image el, int iteratorEl, int counter = 0, int lastDirection = 0)
    {
        if (lastDirection == 0)
        {
            float randomChance = Random.Range(0f, 1f);
            if (randomChance > 0.5)
            {
                lastDirection = -1;
            }
            else
            {
                lastDirection = 1;
            }
        }
        // Check if enough cycles have passed to start a possible fade.
        if (counter > 2)
        {
            // If so, see if the animation happens this cycle by random chance.
            float randomChance = Random.Range(0f, 1f);
            if (randomChance < 0.3f)
            {
                // If it happens, set counter to 0.
                counter = 0;
                // Trigger the randomized animation...
                // lastDirection indicates which direction was previously taken by this object (grow [true] or shrink [false]).
                // This animation will always do the opposite.
                float newScale;
                // If it just shrank, grow it.
                if (lastDirection > 0)
                {
                    // Find random target scale.
                    newScale = Random.Range(1f, 1.2f);
                    lastDirection = -1;
                }
                else
                {
                    newScale = Random.Range(0.8f, 1f);
                    lastDirection = 1;
                }
                // Run animation for a random amount of time.
                float randomDuration = Random.Range(1.2f, 2.5f);
                Vector2 bgBlockTargetDimensions = bgBlockOriginalDimensions[iteratorEl] * new Vector2(newScale, newScale);
                el.GetComponent<RectTransform>().DOSizeDelta(bgBlockTargetDimensions, randomDuration);
                yield return new WaitForSeconds(randomDuration);
            }
        }
        counter++;
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        OverlayBGBlockRoutines[iteratorEl] = StartCoroutine(AnimationBGBlockScale(el, iteratorEl, counter, lastDirection));
    }

    private void animationRandomizePosition(Image obj)
    {
        Vector3 MainMenuLogoPanelPosition = OverlayManager.OverlayCanvas.GetComponent<RectTransform>().sizeDelta;

        float newRandomX = Random.Range(0.0f, MainMenuLogoPanelPosition.x) - MainMenuLogoPanelPosition.x / 2;
        float newRandomY = Random.Range(0.0f, MainMenuLogoPanelPosition.y) - MainMenuLogoPanelPosition.y / 2;

        Vector3 newRandomPosition = new Vector3(newRandomX, newRandomY, 0);

        obj.GetComponent<RectTransform>().localPosition = newRandomPosition;
    }

    public IEnumerator CloseOverlayAnimation()
    {
        float animTimeScale = overlayAnimTimeScale;

        // Slow fade the dark background out.
        DPDarkBGFade = OverlayManager.DarkBGPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.4f * animTimeScale);

        // Trigger the Frosted Glass fade.
        if (FrostedGlassFade != null)
        {
            StopCoroutine(FrostedGlassFade);
        }
        FrostedGlassFade = StartCoroutine(CloseFrostedGlass(OverlayManager.instance.FrostedGlassBlurGameplay.GetComponent<Image>(), animTimeScale));

        // *** Trigger the BG Blocks animations
        // Check if the coroutine exists. Delete it if it does.
        if (OverlayBGAnimation != null)
        {
            StopCoroutine(OverlayBGAnimation);
        }
        // Delete any continuous animation coroutines if they exist.
        for (int i = 0; i < 5; i++)
        {
            if (OverlayBGBlockRoutines[i] != null)
            {
                StopCoroutine(OverlayBGBlockRoutines[i]);
            }
        }
        // Start this closing coroutine.
        OverlayBGAnimation = StartCoroutine(CloseBGAnim(animTimeScale));

        // Trigger the G elements close animation.
        //OverlayGAnimation = StartCoroutine(CloseGAnim(animTimeScale));

        // Show the HUD.

        OverlayManager.TopPanelClock.DOFade(0f, 0.1f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        OverlayManager.TopPanelProcessesText.DOFade(0f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC1.DOFade(0f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC2.DOFade(0f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC3.DOFade(0f, 0.1f * animTimeScale);
        OverlayManager.TopPanelC4.DOFade(0f, 0.1f * animTimeScale);
        OverlayManager.TopPanelSymbiosLogo.DOFade(0f, 0.1f * animTimeScale);

        // Slide top bar back to top.
        // Then, calculate the "starting" position for the panel (offset position). The Y is slightly offset, opposite direction from original.
        Vector3 topPanelTargetDimensions = new Vector3(topPanelOriginalDimensions.x, topPanelOriginalDimensions.y + 50f, topPanelOriginalDimensions.z);
        // Start animation to slide the panel into its original location.
        DPSlideTopBar = OverlayManager.OverlayTopPanel.GetComponent<RectTransform>().DOAnchorPosY(topPanelTargetDimensions.y, 0.2f * animTimeScale);

        // Slide bottom bar back to bottom.
        // Then, calculate the "starting" position for the panel (offset position). The Y is slightly offset.
        Vector3 bottomPanelTargetDimensions = new Vector3(bottomPanelOriginalDimensions.x, bottomPanelOriginalDimensions.y - 80f, bottomPanelOriginalDimensions.z);
        // Start animation to slide the panel into its original location.
        DPSlideBottomBar = OverlayManager.OverlayBottomPanel.GetComponent<RectTransform>().DOAnchorPosY(bottomPanelTargetDimensions.y, 0.2f * animTimeScale);

        // Fade in the top and bottom panels.
        DPTopPanelFade = OverlayManager.OverlayTopPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.1f * animTimeScale);
        DPBottomPanelFade = OverlayManager.OverlayBottomPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.1f * animTimeScale);

        // Fade in the nav buttons one by one.
        OverlayManager.NavPanelMapButton.DOFade(0f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
        OverlayManager.NavPanelObjectivesButton.DOFade(0f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
        OverlayManager.NavPanelUpgradesButton.DOFade(0f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);
        OverlayManager.NavPanelDatabaseButton.DOFade(0f, 0.1f * animTimeScale);
        yield return new WaitForSeconds(0.05f * animTimeScale);

        // Disable the overlay.
        //overlay.gameObject.SetActive(false);
    }

    // Stores the base coordinates for each panel's left, original, and right positions in order to animate from one to the other.
    private Vector3[] OverlayNavigateBaseLeftPosition = new Vector3[4], OverlayNavigateBaseOriginalPosition = new Vector3[4], OverlayNavigateBaseRightPosition = new Vector3[4];
    // Store the current positional Tweens for each panel.
    private Tween[] OverlayNavigatePosition = new Tween[4];
    // Store the current fade tweens for each panel.
    private Tween[] OverlayNavigateFade = new Tween[4];
    private float overlayNavigateAnimationDuration = 0.3f;

    public void NavigateOpenAnimation(int targetPanelNum)
    {
        // Grab the target panel object.
        GameObject targetPanel = OverlayManager.OverlayPanels[targetPanelNum];

        // Prepare panel by instantly placing it in the left position.
        targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseLeftPosition[targetPanelNum].x, 0f);

        // Transition the panel to original position.
        OverlayNavigatePosition[targetPanelNum] = targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseOriginalPosition[targetPanelNum].x, overlayNavigateAnimationDuration);
        // Transition the panel to 1 opacity.
        OverlayNavigateFade[targetPanelNum] = targetPanel.GetComponent<CanvasGroup>().DOFade(1f, overlayNavigateAnimationDuration).SetDelay(0.1f);

        UpdateHighlightColors(targetPanelNum);
        UpdateHighlightedElements();
    }

    public void NavigatePanelCloseRightAnimation(int targetPanelNum)
    {
        // Grab the target panel object.
        GameObject targetPanel = OverlayManager.OverlayPanels[targetPanelNum];

        // Transition the panel to right position.
        OverlayNavigatePosition[targetPanelNum] = targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseRightPosition[targetPanelNum].x, overlayNavigateAnimationDuration);
        // Transition the panel to 0 opacity.
        OverlayNavigateFade[targetPanelNum] = targetPanel.GetComponent<CanvasGroup>().DOFade(0f, overlayNavigateAnimationDuration);
    }

    public void NavigatePanelOpenRightAnimation(int targetPanelNum)
    {
        // Grab the target panel object.
        GameObject targetPanel = OverlayManager.OverlayPanels[targetPanelNum];

        // Prepare panel by instantly placing it in the left position.
        targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseLeftPosition[targetPanelNum].x, 0f);

        // Transition the panel to original position.
        OverlayNavigatePosition[targetPanelNum] = targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseOriginalPosition[targetPanelNum].x, overlayNavigateAnimationDuration);
        // Transition the panel to 1 opacity.
        OverlayNavigateFade[targetPanelNum] = targetPanel.GetComponent<CanvasGroup>().DOFade(1f, overlayNavigateAnimationDuration).SetDelay(0.1f);
    
        UpdateHighlightColors(targetPanelNum);
        UpdateHighlightedElements();
    }

    public void NavigatePanelCloseLeftAnimation(int targetPanelNum)
    {
        // Grab the target panel object.
        GameObject targetPanel = OverlayManager.OverlayPanels[targetPanelNum];

        // Transition the panel to right position.
        OverlayNavigatePosition[targetPanelNum] = targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseLeftPosition[targetPanelNum].x, overlayNavigateAnimationDuration);
        // Transition the panel to 0 opacity.
        OverlayNavigateFade[targetPanelNum] = targetPanel.GetComponent<CanvasGroup>().DOFade(0f, overlayNavigateAnimationDuration);
    }

    public void NavigatePanelOpenLeftAnimation(int targetPanelNum)
    {
        // Grab the target panel object.
        GameObject targetPanel = OverlayManager.OverlayPanels[targetPanelNum];

        // Prepare panel by instantly placing it in the left position.
        targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseRightPosition[targetPanelNum].x, 0f);

        // Transition the panel to original position.
        OverlayNavigatePosition[targetPanelNum] = targetPanel.GetComponent<RectTransform>().DOAnchorPosX(OverlayNavigateBaseOriginalPosition[targetPanelNum].x, overlayNavigateAnimationDuration);
        // Transition the panel to 1 opacity.
        OverlayNavigateFade[targetPanelNum] = targetPanel.GetComponent<CanvasGroup>().DOFade(1f, overlayNavigateAnimationDuration).SetDelay(0.1f);

        UpdateHighlightColors(targetPanelNum);
        UpdateHighlightedElements();
    }

    public void UpdateHighlightColors(int targetPanelNum)
    {
        OverlayHighlightColor = OverlayHighlightColors[targetPanelNum];
    }
    public void UpdateHighlightedElements()
    {
        OverlayManager.instance.TopPanelSymbiosLogo.color = OverlayHighlightColor;
        OverlayManager.instance.TopPanelC3.color = OverlayHighlightColor;

        // Change colors of bg squares
        for (int i = 0; i < 5; i++)
        {
            OverlayManager.instance.OverlayBGAnimBlocks[i].color = OverlayHighlightColor;
        }
    }

}
