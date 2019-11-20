using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)

public class CharacterPanelManager : MonoBehaviour
{
    #region CharacterHealth
    public Image HealthBar, HealthBarDamage;
    public TextMeshProUGUI HealthAmountText;
    #endregion

    #region CharacterClass
    public TextMeshProUGUI CharacterClassClassText, CharacterClassNameText;
    public Image CharacterClassImage;

    [SerializeField]
    private Sprite[] CharacterClassImages = { };
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

    public GameObject CharacterDiagnosticInfoPanel;
    public Image CharacterAbilityFill, CharacterAbilityEmpty;

    public void Start()
    {
        CharacterAbilityFill.fillAmount = 1f;
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
        HealthAmountText.text = "<style=\"HPNumber\">" + health + "</style><sprite=0>" + maxHealth;
    }

    /// <summary>
    /// Update the HUD to reflect the player's current class.
    /// </summary>
    public void UpdateCharacterClass(Player p)
    {
        string playerClass = p.Class;
        // Based on the class, change some UI elements.
        if (playerClass == "small")
        {
            CharacterClassImage.sprite = CharacterClassImages[0];
            CharacterClassImage.color = colorCharacterLight;
            CharacterClassClassText.text = "Daitengu Class";
            CharacterClassClassText.color = colorCharacterLight;
            CharacterClassNameText.text = "Larry";
        }
        else if (playerClass == "medium")
        {
            CharacterClassImage.sprite = CharacterClassImages[1];
            CharacterClassImage.color = colorCharacterMedium;
            CharacterClassClassText.text = "Koshchei Class";
            CharacterClassClassText.color = colorCharacterMedium;
            CharacterClassNameText.text = "Ivan";
        }
    }

    /// <summary>
    /// Coroutine to slowly refill ability meter
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator UpdateAbilityCooldownUI(float seconds)
    {
        //Counter should start at 0 and continue until cooldown amount has passed
        float counter = 0;
        
        //Continue loop while bar is still refilling
        while (counter < seconds)
        {
            //Fill the bar with how much time has passed
            CharacterAbilityFill.fillAmount = counter / seconds;
            counter += .1f;
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }
}
