using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)
using UnityEngine.UI;

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
    public Image WeaponImage;
    public Image WeaponClassImage;
    public Image WeaponClassFrame;
    public TextMeshProUGUI WeaponNameText, WeaponClassText;
    public Text DiagnosticWeaponName, DiagnosticWeaponDescription;


    private Color ColorWeaponClassAutomatic = new Color32(0xe5, 0x2a, 0xfb, 0xff);
    private Color ColorWeaponClassScatter = new Color32(0x2a, 0xf9, 0xfb, 0xff);
    private Color ColorWeaponClassPrecision = new Color32(0xea, 0xfb, 0x2a, 0xff);
    private Color currentWeaponClassColor;
    private string currentWeaponClassText;
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

    [Header("Diagnostic GameObjects")]
    [SerializeField]
    private Sprite[] WeaponClassImages = { };
    public Image[] WeaponDiagnosticBars;
    public Text[] WeaponDiagnosticValues;
    public GameObject WeaponDiagnosticInfoPanel;

    /// <summary>
    /// Updates ammo and weapon aspects in the HUD.
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateWeapon(Player p)
    {
        Weapon weapon = p.Weapon;
        // Update weapon HUD elements to reflect current weapon...
        // Update current weapon class color and set various elements to that color.
        if (weapon is Automatic)
        {
            currentWeaponClassColor = ColorWeaponClassAutomatic;
            currentWeaponClassText = "auto";
            WeaponClassImage.sprite = WeaponClassImages[0];
        }
        else if (weapon is Precision)
        {
            currentWeaponClassColor = ColorWeaponClassPrecision;
            currentWeaponClassText = "precise";
            WeaponClassImage.sprite = WeaponClassImages[1];
        }
        else if (weapon is Spread)
        {
            currentWeaponClassColor = ColorWeaponClassScatter;
            currentWeaponClassText = "scatter";
            WeaponClassImage.sprite = WeaponClassImages[2];
        }

        // class frame
        WeaponClassFrame.color = currentWeaponClassColor;
        // class text
        WeaponClassText.color = currentWeaponClassColor;
        // class image
        WeaponClassImage.color = currentWeaponClassColor;
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

        // Update the class name.
        WeaponClassText.text = currentWeaponClassText;
        // Update Diagnostic weapon name text
        DiagnosticWeaponName.text = p.Weapon.Name;
        // Update Diagnostic weapon description text
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
    public void UpdateWeaponDiagnostic(Player p)
    {
        float[] weaponDiagnosticValues = {
            p.Weapon.Damage,
            Mathf.Round(p.Weapon.RateOfFire * 100f) / 100f,
            Mathf.Round(p.Weapon.ReloadTime * 100f) / 100f,
            (int) p.Weapon.Range,
            (int) p.Weapon.BulletVeloc
            };

        float[] weaponDiagnosticMaxs = {
            GameManager.MaxValues[(int)Category.Damage],
            GameManager.MaxValues[(int)Category.FireRate],
            GameManager.MaxValues[(int)Category.ReloadTime],
            GameManager.MaxValues[(int)Category.Range],
            GameManager.MaxValues[(int)Category.BulletSpeed] 
            };

        //Loop through each stat and update value and fill amount for bar
        foreach (Category c in System.Enum.GetValues(typeof(Category)))
        {
            WeaponDiagnosticBars[(int)c].fillAmount = weaponDiagnosticValues[(int)c] / weaponDiagnosticMaxs[(int)c];
            WeaponDiagnosticValues[(int)c].text = $"{weaponDiagnosticValues[(int)c]}";
            if (WeaponDiagnosticBars[(int)c].fillAmount > 0.95)
            {
                WeaponDiagnosticBars[(int)c].color = customBlue;
            }
            else if (WeaponDiagnosticBars[(int)c].fillAmount > 0.66)
            {
                WeaponDiagnosticBars[(int)c].color = customGreen;
            }
            else if (WeaponDiagnosticBars[(int)c].fillAmount > 0.33)
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
