using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image healthBar, ammoBar, swapBar;
    public Text healthAmt, ammoAmt;

    #region Custom Colors
    Color customRed = new Color(.94921875f, .15234375f, .2265625f, 1);
    Color customYellow = new Color(.89453125f, .8359375f, .19140625f, 1);
    Color customGreen = new Color(.35546875f, .89453125f, .19140625f, 1);
    Color customBlue = new Color(.19140625f, .78515625f, .89453125f, 1);
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
        healthBar.fillAmount = health / maxHealth;
        healthAmt.text = health + "hp" + " / " + maxHealth;
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
    public void UpdateSwap(float val, float max)
    {
        swapBar.fillAmount = val / max;
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
