using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)
using UnityEngine.UI;
using System.Linq;

public class WeaponPanelManager : MonoBehaviour
{
    #region Ammo
    [Header("Ammo")]
    public Image AmmoBar;
    public Image AmmoBarFrame;
    public RawImage AmmoBarTiled;
    public TextMeshProUGUI AmmoAmountText, AmmoBarLabel;
    #endregion

    #region Weapon
    [Header("Weapon")]
    public Image WeaponImage, WeaponFrame;
    public TextMeshProUGUI WeaponNameText, WeaponClassText;

    // Colors for various weapons
    private Color ColorWeaponShockLance = new Color32(0x00, 0xa8, 0xff, 0xff);
    private Color ColorWeaponGaussCannon = new Color32(0xe5, 0x00, 0xff, 0xff);
    private Color ColorWeaponBreachMissile = new Color32(0xff, 0xc9, 0x00, 0xff);
    private Color ColorWeaponVortexLauncher = new Color32(0xff, 0x00, 0x21, 0xff);
    private Color ColorWeaponRotaryRepeater = new Color32(0xff, 0x98, 0x00, 0xff);
    private Color ColorWeaponPulseProjector = new Color32(0x00, 0xff, 0xab, 0xff);

    private Color currentWeaponClassColor;
    private string currentWeaponClassText;

    [SerializeField]
    private Sprite[] WeaponImages = { };
    [Space]
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
    public enum Category
    {
        Damage,
        FireRate,
        ReloadTime,
        Range,
        BulletSpeed
    }

    #region Weapon Diagnostic
    // Stores the current weapon highlight color (set by the weapon the player is using)
    public Color activeWeaponColor;

    [Header("Diagnostic GameObjects")]
    [SerializeField]
    private Sprite[] WeaponClassImages = { };

    public GameObject WeaponDiagnosticInfoPanel;

    // Generic elements associated with weapon diagnostic.
    public Image WeaponDiagnosticAnimFrame, WeaponDiagnosticBG, DiagnosticWeaponImage;

    // Main elements at top of weapon diagnostic.
    public TextMeshProUGUI DiagnosticWeaponName, DiagnosticWeaponDescription;

    // Elements within sections of weapon diagnostic.
    // Bars
    public Image[] WeaponDiagnosticBars;
    // Bar Frames
    public Image[] WeaponDiagnosticBarFrames;
    // Value Text
    public TextMeshProUGUI[] WeaponDiagnosticValues;
    // Titles
    public TextMeshProUGUI[] WeaponDiagnosticTitles;
    // Icons
    public Image[] WeaponDiagnosticIcons;

    // Stores the values for each section of the weapon diagnostic.
    private float[] WeaponDiagnosticBarValues = new float[5];
    // Stores the color for each section of the weapon diagnostic.
    private Color[] WeaponDiagnosticColors = new Color[5];
    #endregion

    // Returns a color corresponding to the given weapon.
    public Color RetrieveCorrespondingWeaponColor(Weapon w)
    {
        if (w is GaussCannon)
        {
            return ColorWeaponGaussCannon;
        }
        else if (w is PulseProjector)
        {
            return ColorWeaponPulseProjector;
        }
        else if (w is VortexLauncher)
        {
            return ColorWeaponVortexLauncher;
        }
        else if (w is RotaryRepeater)
        {
            return ColorWeaponRotaryRepeater;
        }
        else if (w is ShockLance)
        {
            return ColorWeaponShockLance;
        }
        else if (w is BreachMissile)
        {
            return ColorWeaponBreachMissile;
        } 
        else
        {
            return Color.white;
        }
    }

    // Returns a name string corresponding to the given weapon.
    public string RetrieveCorrespondingWeaponName(Weapon w)
    {
        if (w is GaussCannon)
        {
            return "Gauss Cannon";
        }
        else if (w is PulseProjector)
        {
            return "Pulse Projector";
        }
        else if (w is VortexLauncher)
        {
            return "Vortex Launcher";
        }
        else if (w is RotaryRepeater)
        {
            return "Rotary Repeater";
        }
        else if (w is ShockLance)
        {
            return "Shock Lance";
        }
        else if (w is BreachMissile)
        {
            return "Breach Missile";
        }
        else
        {
            return "Undefined Weapon";
        }
    }

    // Returns a formal name string corresponding to the given weapon.
    public string RetrieveCorrespondingWeaponFormalName(Weapon w)
    {
        if (w is GaussCannon)
        {
            return "Mokumokuren Type R9";
        }
        else if (w is PulseProjector)
        {
            return "GW Sonic Projector";
        }
        else if (w is VortexLauncher)
        {
            return "SHIVA Type 88";
        }
        else if (w is RotaryRepeater)
        {
            return "M-88 Liberty";
        }
        else if (w is ShockLance)
        {
            return "NANCOM K-1 Multitool";
        }
        else if (w is BreachMissile)
        {
            return "Tsarevich TK-2";
        }
        else
        {
            return "???";
        }
    }

    // Returns a sprite index corresponding to the given weapon.
    public int RetrieveCorrespondingWeaponSprite(Weapon w)
    {
        if (w is GaussCannon)
        {
            return 1;
        }
        else if (w is PulseProjector)
        {
            return 2;
        }
        else if (w is VortexLauncher)
        {
            return 3;
        }
        else if (w is RotaryRepeater)
        {
            return 0;
        }
        else if (w is ShockLance)
        {
            return 4;
        }
        else if (w is BreachMissile)
        {
            return 5;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Updates ammo and weapon aspects in the HUD.
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateWeapon(Character p)
    {
        Weapon weapon = p.Weapon;

        // Update weapon HUD elements to reflect current weapon...
        // Update current weapon class color and set various elements to that color.
        currentWeaponClassColor = RetrieveCorrespondingWeaponColor(weapon);

        // class text
        //WeaponClassText.color = currentWeaponClassColor;
        // ammo bar frame
        AmmoBarFrame.color = currentWeaponClassColor;
        // ammo bar image
        AmmoBarTiled.color = currentWeaponClassColor;
        // ammo bar label
        AmmoBarLabel.color = currentWeaponClassColor;
        // current ammo text color
        AmmoAmountText.color = currentWeaponClassColor;
        // weapon name
        WeaponNameText.color = currentWeaponClassColor;
        // weapon image
        WeaponImage.color = currentWeaponClassColor;
        // weapon frame
        //WeaponFrame.color = currentWeaponClassColor;

        // Update weapon silhouette in diagnostic and HUD.
        int weaponImageIndex = RetrieveCorrespondingWeaponSprite(weapon);
        // Set the image sprites based on the image stored at that index on the weapon images array.
        WeaponImage.sprite = WeaponImages[weaponImageIndex];
        DiagnosticWeaponImage.sprite = WeaponImages[weaponImageIndex];

        currentWeaponClassText = RetrieveCorrespondingWeaponName(weapon);

        // Update diagnostic image and title with class color.
        DiagnosticWeaponName.color = currentWeaponClassColor;
        DiagnosticWeaponImage.color = currentWeaponClassColor;

        // Update active color class.
        activeWeaponColor = currentWeaponClassColor;
        HUDController.instance.activeWeaponColor = currentWeaponClassColor;

        // Update the class name.
        WeaponClassText.text = RetrieveCorrespondingWeaponFormalName(weapon);
        // Update Diagnostic and HUD weapon name text.
        DiagnosticWeaponName.text = p.Weapon.FullName;
        WeaponNameText.text = RetrieveCorrespondingWeaponName(weapon);
        // Update Diagnostic weapon description text.
        DiagnosticWeaponDescription.text = p.Weapon.Description;

        // Update current ammo.
        UpdateAmmo(weapon);
    }

    /// <summary>
    /// Updates ammo aspects in the HUD.
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateAmmo(Weapon weapon)
    {
        float ammoInClip = weapon.AmmoInClip;
        float clipSize = weapon.ClipSize;
        float totalAmmo = weapon.TotalAmmo;

        float ammoRatio = ammoInClip / clipSize;

        AmmoBarTiled.uvRect = new Rect(0, 0, ammoInClip, ammoInClip);

        float frameWidth = AmmoBarFrame.rectTransform.sizeDelta.x;
        float frameHeight = AmmoBarFrame.rectTransform.sizeDelta.y;

        AmmoBarTiled.rectTransform.sizeDelta = new Vector2(frameWidth * ammoRatio, frameHeight);

        if (ammoRatio == 0)
        {
            AmmoBarFrame.fillAmount = 1;
        }
        else
        {
            AmmoBarFrame.fillAmount = (float)1.0 - ammoRatio;
        }

        // Update text for 
        AmmoAmountText.text = "<style=\"AmmoNumber\">" + ammoInClip + "</style><sprite=0>" + totalAmmo;
    }

    /// <summary>
    /// Update the weapon diagnostic info whenever a new weapon is equipped
    /// </summary>
    public void UpdateWeaponDiagnostic(Character p)
    {
        Weapon playerWeapon = p.Weapon;
        float[] weaponDiagnosticValues = {
            playerWeapon.Damage,
            Mathf.Round(p.Weapon.RateOfFire * 100f) / 100f,
            Mathf.Round(p.Weapon.ReloadTime * 100f) / 100f,
            (int) p.Weapon.Range,
            (int) p.Weapon.BulletVeloc
            };

        float[] weaponDiagnosticMaxs = GameManager.MaxStats[playerWeapon.Name];

        // Loop through each stat and update value and fill amount for bar
        foreach (Category c in System.Enum.GetValues(typeof(Category)))
        {
            // Store the value for this bar.
            WeaponDiagnosticBarValues[(int)c] = weaponDiagnosticValues[(int)c] / weaponDiagnosticMaxs[(int)c];

            // Reflect the values for this bar in the UI.
            WeaponDiagnosticBars[(int)c].fillAmount = weaponDiagnosticValues[(int)c] / weaponDiagnosticMaxs[(int)c];
            WeaponDiagnosticValues[(int)c].text = $"{weaponDiagnosticValues[(int)c]}";
            if (WeaponDiagnosticBars[(int)c].fillAmount > 0.95)
            {
                WeaponDiagnosticValues[(int)c].color = customBlue;
                WeaponDiagnosticBarFrames[(int)c].color = customBlue;
                WeaponDiagnosticBars[(int)c].color = customBlue;
                // Store the color for this bar.
                WeaponDiagnosticColors[(int)c] = customBlue;
            }
            else if (WeaponDiagnosticBars[(int)c].fillAmount > 0.66)
            {
                WeaponDiagnosticValues[(int)c].color = customGreen;
                WeaponDiagnosticBarFrames[(int)c].color = customGreen;
                WeaponDiagnosticBars[(int)c].color = customGreen;
                // Store the color for this bar.
                WeaponDiagnosticColors[(int)c] = customGreen;
            }
            else if (WeaponDiagnosticBars[(int)c].fillAmount > 0.33)
            {
                WeaponDiagnosticValues[(int)c].color = customYellow;
                WeaponDiagnosticBarFrames[(int)c].color = customYellow;
                WeaponDiagnosticBars[(int)c].color = customYellow;
                // Store the color for this bar.
                WeaponDiagnosticColors[(int)c] = customYellow;
            }
            else
            {
                WeaponDiagnosticValues[(int)c].color = customRed;
                WeaponDiagnosticBarFrames[(int)c].color = customRed;
                WeaponDiagnosticBars[(int)c].color = customRed;
                // Store the color for this bar.
                WeaponDiagnosticColors[(int)c] = customRed;
            }
        }

    }

    #region DiagnosticPanelTweens
    // Stores tween for the animated frame.
    private Tween DPFillAmountFrame;
    private Tween DPFadeFrame;
    // Stores tween for dialog bg.
    private Tween DPFadeBG;
    // Stores tweens for main weapon elements (first section in diagnostic).
    private Tween DPFadeWeaponTitle, DPFadeWeaponIcon, DPFadeWeaponText;
    // Stores tweens for each of the bar sections.
    private Tween[] DPIconFade = new Tween[5],
        DPTitleFade = new Tween[5],
        DPValueFade = new Tween[5],
        DPBarFade = new Tween[5],
        DPBarFrameFade = new Tween[5],
        DPBarFillAmount = new Tween[5];
    #endregion

    // Animates the Weapon diagnostic panel and its elements into view.
    public IEnumerator showDiagnosticPanelWeapon()
    {
        // Enable the character diagnostic panel.
        WeaponDiagnosticInfoPanel.SetActive(true);

        // Create a local variable for the fade in color for highlighted elements.
        Color targetColor = activeWeaponColor;
        // Define a time scale for this animation, to easily shorten or lengthen it.
        float animTimeScale = 1f;

        // NOTE: The floats within the following animation and wait functions define a duration for and between various animations.

        // Set fill direction.
        WeaponDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start fill amount tween.
        DPFillAmountFrame = WeaponDiagnosticAnimFrame.DOFillAmount(1, 0.4f * animTimeScale);

        // Start bg fade in tween.
        DPFadeBG = WeaponDiagnosticBG.DOColor(new Color(1f, 1f, 1f, 1f), 0.4f * animTimeScale);

        // Fade the frame in.
        DPFadeFrame = WeaponDiagnosticAnimFrame.DOColor(targetColor, 0.1f * animTimeScale);

        yield return new WaitForSeconds(0.2f * animTimeScale);

        // Start class items fade in.
        DPFadeWeaponTitle = DiagnosticWeaponName.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeWeaponIcon = DiagnosticWeaponImage.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeWeaponText = DiagnosticWeaponDescription.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        showWeaponDiagnosticBarSection(0, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        showWeaponDiagnosticBarSection(1, animTimeScale);

        // Reverse fill direction.
        WeaponDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.TopLeft;
        // Start reverse fill amount tween.
        DPFillAmountFrame = WeaponDiagnosticAnimFrame.DOFillAmount(0, 0.4f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        showWeaponDiagnosticBarSection(2, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        showWeaponDiagnosticBarSection(3, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        showWeaponDiagnosticBarSection(4, animTimeScale);

        // Fade the frame out.
        DPFadeFrame = WeaponDiagnosticAnimFrame.DOColor(new Color(activeWeaponColor[0],
            activeWeaponColor[1],
            activeWeaponColor[2],
            0f), 0.2f * animTimeScale);

        yield return null;
    }

    // Triggers tweens for showing sections within the weapon diagnostic based on given index.
    private void showWeaponDiagnosticBarSection(int index, float animTimeScale)
    {
        // Fade in various elements that make up the bar.
        // White elements
        Color blankColor = new Color(1f, 1f, 1f, 1f);
        DPIconFade[index] = WeaponDiagnosticIcons[index].DOColor(blankColor, 0.2f * animTimeScale);
        DPTitleFade[index] = WeaponDiagnosticTitles[index].DOColor(blankColor, 0.2f * animTimeScale);
        // Hue elements
        Color targetColor = new Color(WeaponDiagnosticColors[index][0],
            WeaponDiagnosticColors[index][1],
            WeaponDiagnosticColors[index][2],
            1f);
        DPValueFade[index] = WeaponDiagnosticValues[index].DOColor(targetColor, 0.2f * animTimeScale);
        DPBarFade[index] = WeaponDiagnosticBars[index].DOColor(targetColor, 0.2f * animTimeScale);
        DPBarFrameFade[index] = WeaponDiagnosticBarFrames[index].DOColor(targetColor, 0.2f * animTimeScale);

        // Tween the bar's fill amount to current value.
        DPBarFillAmount[index] = WeaponDiagnosticBars[index].DOFillAmount(WeaponDiagnosticBarValues[index], 0.3f * animTimeScale);
    }

    // Animates the weapon diagnostic panel and its elements out of view.
    public IEnumerator hideDiagnosticPanelWeapon()
    {
        // Create a local variable for the fade out color for highlighted elements.
        // (in this case, it is the faded out version of the color).
        Color targetColor = new Color(activeWeaponColor[0],
            activeWeaponColor[1],
            activeWeaponColor[2],
            0f);
        // Define a time scale for this animation, to easily shorten or lengthen it.
        float animTimeScale = 1f;

        // NOTE: The floats within the following animation and wait functions define a duration for and between various animations.

        // Set fill direction.
        WeaponDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.TopLeft;
        // Start fill amount tween.
        DPFillAmountFrame = WeaponDiagnosticAnimFrame.DOFillAmount(1, 0.4f * animTimeScale);

        // Fade the frame in.
        DPFadeFrame = WeaponDiagnosticAnimFrame.DOColor(activeWeaponColor, 0.1f * animTimeScale);

        // Start class items fade out.
        DPFadeWeaponTitle = DiagnosticWeaponName.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeWeaponIcon = DiagnosticWeaponImage.DOColor(targetColor, 0.2f * animTimeScale);
        DPFadeWeaponText = DiagnosticWeaponDescription.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        hideWeaponDiagnosticBarSection(0, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        hideWeaponDiagnosticBarSection(1, animTimeScale);

        // Start bg fade out tween.
        DPFadeBG = WeaponDiagnosticBG.DOColor(new Color(1f, 1f, 1f, 0f), 0.4f * animTimeScale);

        // Reverse fill direction.
        WeaponDiagnosticAnimFrame.fillOrigin = (int)Image.Origin90.BottomRight;
        // Start reverse fill amount tween.
        DPFillAmountFrame = WeaponDiagnosticAnimFrame.DOFillAmount(0, 0.4f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        hideWeaponDiagnosticBarSection(2, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        hideWeaponDiagnosticBarSection(3, animTimeScale);

        // Fade the frame out.
        DPFadeFrame = WeaponDiagnosticAnimFrame.DOColor(targetColor, 0.2f * animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        hideWeaponDiagnosticBarSection(4, animTimeScale);

        yield return new WaitForSeconds(0.1f * animTimeScale);

        // Disable the character diagnostic panel.
        WeaponDiagnosticInfoPanel.SetActive(false);

        yield return null;
    }

    // Triggers tweens for hiding sections within the weapon diagnostic based on given index.
    private void hideWeaponDiagnosticBarSection(int index, float animTimeScale)
    {
        // Fade out various elements that make up the bar.
        // White elements
        Color blankColor = new Color(1f, 1f, 1f, 0f);
        DPIconFade[index] = WeaponDiagnosticIcons[index].DOColor(blankColor, 0.2f * animTimeScale);
        DPTitleFade[index] = WeaponDiagnosticTitles[index].DOColor(blankColor, 0.2f * animTimeScale);
        // Hue elements
        Color targetColor = new Color(WeaponDiagnosticColors[index][0],
            WeaponDiagnosticColors[index][1],
            WeaponDiagnosticColors[index][2],
            0f);
        DPValueFade[index] = WeaponDiagnosticValues[index].DOColor(targetColor, 0.2f * animTimeScale);
        DPBarFade[index] = WeaponDiagnosticBars[index].DOColor(targetColor, 0.2f * animTimeScale);
        DPBarFrameFade[index] = WeaponDiagnosticBarFrames[index].DOColor(targetColor, 0.2f * animTimeScale);

        // Tween the bar's fill amount to 0.
        DPBarFillAmount[index] = WeaponDiagnosticBars[index].DOFillAmount(0f, 0.1f * animTimeScale);
    }

    // Instantly reset the Character diagnostic panel elements to their initial hidden state.
    public void hideDiagnosticPanelWeaponInitial()
    {
        // Create a local variable for the fade out color for highlighted elements.
        // (in this case, it is the faded out version of the color).
        Color targetColor = new Color(activeWeaponColor[0],
            activeWeaponColor[1],
            activeWeaponColor[2],
            0f);

        // Hide and reset anim frame.
        WeaponDiagnosticAnimFrame.fillAmount = 0f;
        WeaponDiagnosticAnimFrame.color = targetColor;

        // Hide colored elements.
        DiagnosticWeaponImage.color = targetColor;
        DiagnosticWeaponName.color = targetColor;

        // Hide non colored elements.
        WeaponDiagnosticBG.color = new Color(1f, 1f, 1f, 0f);
        DiagnosticWeaponDescription.color = new Color(1f, 1f, 1f, 0f);

        // Hide all of the elements within the sections of the weapon diagnostic.
        for (int i = 0; i < 5; i++)
        {
            hideWeaponDiagnosticBarSectionInitial(i);
        }
    }

    private void hideWeaponDiagnosticBarSectionInitial(int index)
    {
        // Hide various elements that make up the bar.
        // White elements
        Color blankColor = new Color(1f, 1f, 1f, 0f);
        WeaponDiagnosticIcons[index].color = blankColor;
        WeaponDiagnosticTitles[index].color = blankColor;
        // Hue elements
        Color targetColor = new Color(WeaponDiagnosticColors[index][0],
            WeaponDiagnosticColors[index][1],
            WeaponDiagnosticColors[index][2],
            0f);
        WeaponDiagnosticValues[index].color = targetColor;
        WeaponDiagnosticBars[index].color = targetColor;
        WeaponDiagnosticBarFrames[index].color = targetColor;

        // Set the bar's fill amount to 0.
        WeaponDiagnosticBars[index].fillAmount = 0f;
    }

}
