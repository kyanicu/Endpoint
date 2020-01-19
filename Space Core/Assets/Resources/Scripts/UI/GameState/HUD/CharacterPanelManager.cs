using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)

public class CharacterPanelManager : MonoBehaviour
{
    #region CharacterHealth
    [Header("Character Health")]
    public Image HealthBar;
    public Image HealthBarDamage;

    public TextMeshProUGUI HealthAmountText;
    #endregion

    #region CharacterClass
    [Header("Character Class")]
    public Image CharacterClassImage;
    public TextMeshProUGUI CharacterClassClassText;
    public TextMeshProUGUI CharacterClassNameText;

    [SerializeField]
    private Sprite[] CharacterClassImages = { };
    
    [SerializeField]
    private Sprite[] CharDiagnosticImages = { };
    #endregion

    #region Custom Colors
    Color customRed = new Color(.94921875f, .15234375f, .2265625f, 1);
    Color customYellow = new Color(.89453125f, .8359375f, .19140625f, 1);
    Color customGreen = new Color(.35546875f, .89453125f, .19140625f, 1); 
    Color customBlue = new Color(.19140625f, .78515625f, .89453125f, 1);

    Color barBlank = new Color(1f, 1f, 1f, 1);
    Color barHighlight = new Color(0.136f, 0.855f, 0.984f, 1);

    static Color colorCharacterLight = new Color(.984375f, .65625f, .13671875f, 1);
    static Color colorCharacterMedium = new Color32(0xFf, 0x41, 0x45, 0xff);
    static Color colorCharacterHeavy = new Color32(0x23, 0xdb, 0xfc, 0xff);
    #endregion

    #region CharacterAbility
    [Header("Character Ability")]
    public Image CharacterActiveAbilityFill;
    public Image CharacterActiveAbilityEmpty;
    public Image CharacterPassiveAbilityFill;
    public Image CharacterPassiveAbilityEmpty;

    public TextMeshProUGUI CharacterActiveAbilityText;
    public TextMeshProUGUI CharacterPassiveAbilityText;
    #endregion

    #region CharacterDiagnostic
    [Header("Character Diagnostic")]
    public GameObject CharacterDiagnosticInfoPanel;

    public Image CharDiagnosticClassIcon;
    public Image CharDiagnosticActiveAbilityIcon;
    public Image CharDiagnosticPassiveAbilityIcon;
    public Image CharDiagnosticAnimFrame;
    public Image CharDiagnosticBG;

    [Space]
    public TextMeshProUGUI CharDiagnosticClassTitle;
    public TextMeshProUGUI CharDiagnosticClassText;
    public TextMeshProUGUI CharDiagnosticActiveAbilityAbbr;
    public TextMeshProUGUI CharDiagnosticActiveAbilityTitle;
    public TextMeshProUGUI CharDiagnosticActiveAbilityText;
    public TextMeshProUGUI CharDiagnosticPassiveAbilityAbbr;
    public TextMeshProUGUI CharDiagnosticPassiveAbilityTitle;
    public TextMeshProUGUI CharDiagnosticPassiveAbilityText;
    #endregion

    #region CharacterUIDetails
    private Color[] CharacterClassColors = { colorCharacterLight, colorCharacterMedium, colorCharacterHeavy };
    private string[] CharacterClassClassTexts = { 
        "Daitengu Class", 
        "Koshchei Class", 
        "Titan Class" 
    };
    private string[] CharacterClassNameTexts = { 
        "Type 89", 
        "BEK-276", 
        "Mark 4" 
    };
    private string[] CharacterClassDescTexts = { 
        "A fast moving chassis with light armor.", 
        "A moderate chassis with average armor.", 
        "A slow chassis with heavy armor." 
    };
    #endregion

    public void Start()
    {
        CharacterActiveAbilityFill.fillAmount = 1f;
        CharacterPassiveAbilityFill.fillAmount = 1f;
    }

    /// <summary>
    /// Updates healthbar and text ui element in Player HUD Canvas
    /// </summary>
    /// <param name="maxHealth"></param>
    /// <param name="health"></param>
    public void UpdateHealth(float maxHealth, float health)
    {
        // Tween the health bar to the upated value.
        HealthBar.DOFillAmount(health / maxHealth, 1);
        // Tween the damage health bar slightly slower, to provide the effect of showing damage taken.
        HealthBarDamage.DOFillAmount(health / maxHealth, 2);
        HealthAmountText.text = "<style=\"HPNumber\">" + (int) health + "</style><sprite=0>" + (int) maxHealth;
    }

    /// <summary>
    /// Update the HUD to reflect the player's current class.
    /// </summary>
    public void UpdateCharacterClass(Player p)
    {
        string playerClass = p.Class;
        int playerClassID;

        switch (playerClass)
        {
            case "light":
                playerClassID = 0;
                break;
            case "medium":
                playerClassID = 1;
                break;
            case "heavy":
                playerClassID = 2;
                break;
            default:
                playerClassID = 0;
                break;
        }

        CharacterClassImage.sprite = CharacterClassImages[playerClassID];
        CharacterClassImage.color = CharacterClassColors[playerClassID];
        CharacterClassClassText.text = CharacterClassClassTexts[playerClassID];
        CharacterClassClassText.color = CharacterClassColors[playerClassID];
        CharacterClassNameText.text = CharacterClassNameTexts[playerClassID];

        CharDiagnosticClassIcon.sprite = CharDiagnosticImages[playerClassID];
        CharDiagnosticClassTitle.text = CharacterClassClassTexts[playerClassID];
        CharDiagnosticClassText.text = CharacterClassDescTexts[playerClassID];

        HUDController.instance.activeClassColor = CharacterClassColors[playerClassID];
        HUDController.instance.RecolorHUD();
    }

    /// <summary>
    /// Update the HUD to reflect the player's current abilities.
    /// </summary>
    public void UpdateCharacterAbilities(Player p)
    {
        Ability playerActiveAbility = p.ActiveAbility;
        Ability playerPassiveAbility = p.PassiveAbility;

        // Change Ability source images based on the ability.
        CharacterActiveAbilityFill.sprite = playerActiveAbility.AbilityImage;
        CharacterActiveAbilityEmpty.sprite = playerActiveAbility.AbilityImage;
        CharacterActiveAbilityText.text = playerActiveAbility.AbilityShortName;

        CharDiagnosticActiveAbilityIcon.sprite = playerActiveAbility.AbilityImage;
        CharDiagnosticActiveAbilityAbbr.text = playerActiveAbility.AbilityShortName;
        CharDiagnosticActiveAbilityTitle.text = playerActiveAbility.AbilityName;
        CharDiagnosticActiveAbilityText.text = playerActiveAbility.AbilityDescription;

        CharacterPassiveAbilityFill.sprite = playerPassiveAbility.AbilityImage;
        CharacterPassiveAbilityEmpty.sprite = playerPassiveAbility.AbilityImage;
        CharacterPassiveAbilityText.text = playerPassiveAbility.AbilityShortName;

        CharDiagnosticPassiveAbilityIcon.sprite = playerPassiveAbility.AbilityImage;
        CharDiagnosticPassiveAbilityAbbr.text = playerPassiveAbility.AbilityShortName;
        CharDiagnosticPassiveAbilityTitle.text = playerPassiveAbility.AbilityName;
        CharDiagnosticPassiveAbilityText.text = playerPassiveAbility.AbilityDescription;
    }

    // Recolors all elements within the Character Panel to match the current character's class.
    public void RecolorCharacterHUD()
    {
        // Recolor abilities.
        CharacterActiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterActiveAbilityText.color = HUDController.instance.activeClassColor;
        CharacterPassiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterPassiveAbilityText.color = HUDController.instance.activeClassColor;

        Color activeClassColor = HUDController.instance.activeClassColor;
        CharacterActiveAbilityEmpty.color = new Vector4(activeClassColor[0], activeClassColor[1], activeClassColor[2], 0.2f);
        CharacterPassiveAbilityEmpty.color = new Vector4(activeClassColor[0], activeClassColor[1], activeClassColor[2], 0.2f);

        // Recolor character diagnostic.
        CharDiagnosticAnimFrame.color = HUDController.instance.activeClassColor;
        CharDiagnosticClassIcon.color = HUDController.instance.activeClassColor;
        CharDiagnosticActiveAbilityIcon.color = HUDController.instance.activeClassColor;
        CharDiagnosticPassiveAbilityIcon.color = HUDController.instance.activeClassColor;
        CharDiagnosticClassTitle.color = HUDController.instance.activeClassColor;
        CharDiagnosticActiveAbilityAbbr.color = HUDController.instance.activeClassColor;
        CharDiagnosticPassiveAbilityAbbr.color = HUDController.instance.activeClassColor;
        CharDiagnosticActiveAbilityTitle.color = HUDController.instance.activeClassColor;
        CharDiagnosticPassiveAbilityTitle.color = HUDController.instance.activeClassColor;
    }

    /// <summary>
    /// Coroutine to slowly refill ability meter
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator UpdateAbilityCooldownUI(float seconds)
    {
        // Reset all of the values to what is nessesary at the start of the tween.
        CharacterActiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterActiveAbilityFill.fillAmount = 0;
        CharacterActiveAbilityText.color = HUDController.instance.activeClassColor;

        // Begin the tween to refill the bar.
        CharacterActiveAbilityFill.DOFillAmount(1, seconds).SetEase(Ease.Linear);

        // Wait the amount of seconds it will take to refill the bar.
        yield return new WaitForSeconds(seconds);

        // Define a tiny amount of time between flashes.
        float tinyWaitTime = 0.04f;

        // Flash the icon white.
        CharacterActiveAbilityFill.color = Color.white;
        CharacterActiveAbilityText.color = Color.white;

        // Wait a tiny amount of time.
        yield return new WaitForSeconds(tinyWaitTime);

        // Flash the icon its normal color.
        CharacterActiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterActiveAbilityText.color = HUDController.instance.activeClassColor;

        // Wait a tiny amount of time.
        yield return new WaitForSeconds(tinyWaitTime);

        // Flash the icon white.
        CharacterActiveAbilityFill.color = Color.white;
        CharacterActiveAbilityText.color = Color.white;

        // Wait a tiny amount of time.
        yield return new WaitForSeconds(tinyWaitTime);

        // Flash the icon its normal color.
        CharacterActiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterActiveAbilityText.color = HUDController.instance.activeClassColor;

        // Wait a tiny amount of time.
        yield return new WaitForSeconds(tinyWaitTime);

        // Flash the icon white.
        CharacterActiveAbilityFill.color = Color.white;
        CharacterActiveAbilityText.color = Color.white;

        // Wait a tiny amount of time.
        yield return new WaitForSeconds(tinyWaitTime);

        // Return the icon to its normal color.
        CharacterActiveAbilityFill.color = HUDController.instance.activeClassColor;
        CharacterActiveAbilityText.color = HUDController.instance.activeClassColor;
    }

    #region DiagnosticPanelTweens
    // Stores tween for the animated frame.
    private Tween DPFillAmountFrame;
    private Tween DPFadeFrame;
    // Stores tween for dialog bg.
    private Tween DPFadeBG;
    // Stores tweens for character class (first Section in diagnostic).
    private Tween DPFadeClassIcon, DPFadeClassTitle, DPFadeClassText;
    // Stores tweens for active ability elements.
    private Tween DPFadeAAIcon, DPFadeAAAbbr, DPFadeAATitle, DPFadeAAText;
    // Stores tweens for passive ability elements.
    private Tween DPFadePAIcon, DPFadePAAbbr, DPFadePATitle, DPFadePAText;
    #endregion

    // Animates the character diagnostic panel and its elements into view.
    public IEnumerator showDiagnosticPanelChar()
    {
        // Enable the character diagnostic panel.
        CharacterDiagnosticInfoPanel.SetActive(true);

        // Create a local variable for the fade in color for highlighted elements.
        Color targetColor = HUDController.instance.activeClassColor;
        // Define a time scale for this animation, to easily shorten or lengthen it.
        float animTimeScale = 1f;

        // NOTE: The floats within the following animation and wait functions define a duration for and between various animations.

        // Set fill direction.
        CharDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start fill amount tween.
        DPFillAmountFrame = CharDiagnosticAnimFrame.DOFillAmount(1, 0.4f * animTimeScale);

        // Start bg fade in tween.
        DPFadeBG = CharDiagnosticBG.DOColor(new Color(1f,1f,1f,1f), 0.4f * animTimeScale);

        // Fade the frame in.
        DPFadeFrame = CharDiagnosticAnimFrame.DOColor(targetColor, 0.1f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Start class items fade in.
        DPFadeClassTitle = CharDiagnosticClassTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeClassIcon = CharDiagnosticClassIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeClassText = CharDiagnosticClassText.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Reverse fill direction.
        CharDiagnosticAnimFrame.fillOrigin = (int) Image.Origin90.TopLeft;
        // Start reverse fill amount tween.
        DPFillAmountFrame = CharDiagnosticAnimFrame.DOFillAmount(0, 0.4f * animTimeScale);

        // Start active ability fade in.
        DPFadeAAIcon = CharDiagnosticActiveAbilityIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAAAbbr = CharDiagnosticActiveAbilityAbbr.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAATitle = CharDiagnosticActiveAbilityTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAAText = CharDiagnosticActiveAbilityText.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Start passive ability fade in.
        DPFadePAIcon = CharDiagnosticPassiveAbilityIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePAAbbr = CharDiagnosticPassiveAbilityAbbr.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePATitle = CharDiagnosticPassiveAbilityTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePAText = CharDiagnosticPassiveAbilityText.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        // Fade the frame out.
        DPFadeFrame = CharDiagnosticAnimFrame.DOColor(new Color(HUDController.instance.activeClassColor[0],
            HUDController.instance.activeClassColor[1],
            HUDController.instance.activeClassColor[2],
            0f), 0.2f * animTimeScale);

        yield return null;
    }

    // Animates the character diagnostic panel and its elements out of view.
    public IEnumerator hideDiagnosticPanelChar()
    {
        // Create a local variable for the fade out color for highlighted elements.
        // (in this case, it is the faded out version of the color).
        Color targetColor = new Color(HUDController.instance.activeClassColor[0],
            HUDController.instance.activeClassColor[1],
            HUDController.instance.activeClassColor[2],
            0f);
        // Define a time scale for this animation, to easily shorten or lengthen it.
        float animTimeScale = 1f;

        // NOTE: The floats within the following animation and wait functions define a duration for and between various animations.

        // Set fill direction.
        CharDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.TopLeft;
        // Start fill amount tween.
        DPFillAmountFrame = CharDiagnosticAnimFrame.DOFillAmount(1, 0.4f * animTimeScale);

        // Fade the frame in.
        DPFadeFrame = CharDiagnosticAnimFrame.DOColor(HUDController.instance.activeClassColor, 0.1f * animTimeScale);

        // Start class items fade out.
        DPFadeClassTitle = CharDiagnosticClassTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeClassIcon = CharDiagnosticClassIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeClassText = CharDiagnosticClassText.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Start bg fade out tween.
        DPFadeBG = CharDiagnosticBG.DOColor(new Color(1f, 1f, 1f, 0f), 0.4f * animTimeScale);

        // Reverse fill direction.
        CharDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start reverse fill amount tween.
        DPFillAmountFrame = CharDiagnosticAnimFrame.DOFillAmount(0, 0.4f * animTimeScale);

        // Start active ability fade out.
        DPFadeAAIcon = CharDiagnosticActiveAbilityIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAAAbbr = CharDiagnosticActiveAbilityAbbr.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAATitle = CharDiagnosticActiveAbilityTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeAAText = CharDiagnosticActiveAbilityText.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Start passive ability fade out.
        DPFadePAIcon = CharDiagnosticPassiveAbilityIcon.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePAAbbr = CharDiagnosticPassiveAbilityAbbr.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePATitle = CharDiagnosticPassiveAbilityTitle.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadePAText = CharDiagnosticPassiveAbilityText.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f * animTimeScale);

        // Fade the frame out.
        DPFadeFrame = CharDiagnosticAnimFrame.DOColor(targetColor, 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Disable the character diagnostic panel.
        CharacterDiagnosticInfoPanel.SetActive(false);

        yield return null;
    }

    // Instantly reset the Character diagnostic panel elements to their initial hidden state.
    public void hideDiagnosticPanelCharInitial()
    {
        // Create a local variable for the fade out color for highlighted elements.
        // (in this case, it is the faded out version of the color).
        Color targetColor = new Color(HUDController.instance.activeClassColor[0],
            HUDController.instance.activeClassColor[1],
            HUDController.instance.activeClassColor[2],
            0f);

        CharDiagnosticAnimFrame.fillAmount = 0f;
        CharDiagnosticAnimFrame.color = targetColor;

        CharDiagnosticClassTitle.color = targetColor;
        CharDiagnosticClassIcon.color = targetColor;
        CharDiagnosticClassText.color = new Color(1f, 1f, 1f, 0f);
        
        CharDiagnosticBG.color = new Color(1f, 1f, 1f, 0f);

        CharDiagnosticActiveAbilityIcon.color = targetColor;
        CharDiagnosticActiveAbilityAbbr.color = targetColor;
        CharDiagnosticActiveAbilityTitle.color = targetColor;
        CharDiagnosticActiveAbilityText.color = new Color(1f, 1f, 1f, 0f);

        CharDiagnosticPassiveAbilityIcon.color = targetColor;
        CharDiagnosticPassiveAbilityAbbr.color = targetColor;
        CharDiagnosticPassiveAbilityTitle.color = targetColor;
        CharDiagnosticPassiveAbilityText.color = new Color(1f, 1f, 1f, 0f);
    }
}
