using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image healthBar, HealthBarDamage, ammoBar;
    public Text ammoAmt;
    public TextMeshProUGUI healthAmt;

    #region CharacterClass Section
    public TextMeshProUGUI CharacterClassClassText, CharacterClassNameText;
    public Image CharacterClassImage;
    #endregion

    #region Swapping Section
    public TextMeshProUGUI SwappingText;
    public Image SwappingBarLeft, SwappingBarRight, SwappingBarFrame, SwappingBarReset;
    #endregion

    [SerializeField]
    private Sprite[] CharacterClassImages = {};

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

    public enum Category
    {
        Damage,
        FireRate,
        ReloadTime,
        MagazineSize,
        AmmoTotal
    }

    public Image[] WeaponDiagnosticBars;
    public Text[] WeaponDiagnosticValues;

    public GameObject WeaponDiagnosticInfoPanel;
    public GameObject ClassDiagnosticInfoPanel;

    private bool diagnosticPanelsVisible = true;

    private static HUDController _instance = null;
    public static HUDController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HUDController>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(HUDController).Name).AddComponent<HUDController>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }

    private void Start()
    {
        // Close the diagnostic panels and set boolean to false.
        toggleDiagnosticPanels();
    }

    /// <summary>
    /// Updates healthbar and text ui element in Player HUD Canvas
    /// </summary>
    /// <param name="maxHealth"></param>
    /// <param name="health"></param>
    public void UpdateHealth(float maxHealth, float health)
    {
        // Tween the health bar to the upated value.
        healthBar.DOFillAmount(health / maxHealth, 1);
        // Tween the damage health bar slightly slower, to provide the effect of showing damage taken.
        HealthBarDamage.DOFillAmount(health / maxHealth, 2);
        healthAmt.text = "<style=\"HPNumber\">" + health + "</style><sprite=0>" + maxHealth;
    }

    /// <summary>
    /// Update the HUD to reflect the player's current class.
    /// </summary>
    public void updateCharacterClass()
    {
        string playerClass = Player.instance.Class;
        // Based on the class, change some UI elements.
        if (playerClass == "small")
        {
            CharacterClassImage.sprite = CharacterClassImages[0];
            CharacterClassImage.color = colorCharacterLight;
            CharacterClassClassText.text = "Daitengu Class";
            CharacterClassClassText.color = colorCharacterLight;
            CharacterClassNameText.text = "Larry";
        } else if (playerClass == "medium")
        {
            CharacterClassImage.sprite = CharacterClassImages[1];
            CharacterClassImage.color = colorCharacterMedium;
            CharacterClassClassText.text = "Koshchei Class";
            CharacterClassClassText.color = colorCharacterMedium;
            CharacterClassNameText.text = "Ivan";
        }
            
    }

    /// <summary>
    /// Updates ammo bar and amount ui element in Player HUD Canvas 
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateAmmo(Weapon weapon)
    {
        float ammoInClip = weapon.AmmoInClip;
        float clipSize = weapon.ClipSize;
        float totalAmmo = weapon.TotalAmmo;

        ammoBar.fillAmount = ammoInClip / clipSize;
        ammoAmt.text = ammoInClip + "rnd/ " + totalAmmo;
    }

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
                SwappingText.color = barHighlight;

                SwappingText.text = "Swap Ready";
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
        Debug.Log("completed swap animation");
    }

    /// <summary>
    /// Toggles the visibility for diagnostic panels
    /// </summary>
    public void toggleDiagnosticPanels()
    {
        diagnosticPanelsVisible = !diagnosticPanelsVisible;
        WeaponDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
        ClassDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
    }
    public void UpdateDiagnosticPanels()
    {
        float[] weaponDiagnosticValues = {
            Player.instance.Weapon.Damage,
            Mathf.Round(Player.instance.Weapon.RateOfFire * 100f) / 100f,
            Mathf.Round(Player.instance.Weapon.ReloadTime * 100f) / 100f,
            Player.instance.Weapon.ClipSize,
            Player.instance.Weapon.TotalAmmo
            };

        float[] weaponDiagnosticMaxs = { 
            WeaponGenerationInfo.TotalRangeStats.MaxDamage,
            WeaponGenerationInfo.TotalRangeStats.MaxRateOfFire,
            WeaponGenerationInfo.TotalRangeStats.MaxReloadTime,
            WeaponGenerationInfo.TotalRangeStats.MaxClipSize,
            WeaponGenerationInfo.TotalRangeStats.MaxClipSize * 5
            };

        //Loop through each stat and update value and fill amount for bar
        foreach (Category c in System.Enum.GetValues(typeof(Category)))
        {
            WeaponDiagnosticBars[(int) c].fillAmount = weaponDiagnosticValues[(int) c] / weaponDiagnosticMaxs[(int) c];
            WeaponDiagnosticValues[(int) c].text = $"{weaponDiagnosticValues[(int) c]}";
            if (WeaponDiagnosticBars[(int) c].fillAmount > 0.95)
            {
                WeaponDiagnosticBars[(int)c].color = customBlue;
            }
            else if (WeaponDiagnosticBars[(int) c].fillAmount > 0.66)
            {
                WeaponDiagnosticBars[(int)c].color = customGreen;
            }
            else if (WeaponDiagnosticBars[(int) c].fillAmount > 0.33)
            {
                WeaponDiagnosticBars[(int)c].color = customYellow;
            }
            else
            {
                WeaponDiagnosticBars[(int)c].color = customRed;
            }
        }
        
    }


}
