using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image healthBar, ammoBar, swapBar;
    public Text healthAmt, ammoAmt;

    public enum WeaponDiagnosticCategories
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

    public void UpdateSwap(float val, float max)
    {
        swapBar.fillAmount = val / max;
    }

    public void toggleDiagnosticPanels()
    {
        diagnosticPanelsVisible = !diagnosticPanelsVisible;
        WeaponDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
        ClassDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
        UpdateDiagnosticPanels();
    }
    public void UpdateDiagnosticPanels()
    {
        // Bar colors (red, yellow, green, blue)
        //Color[] weaponDiagnosticColors = { new Color(243, 39, 58, 1), new Color(229, 214, 49, 1), new Color(91, 229, 49, 1), new Color(49, 201, 229, 1) };
        Color[] weaponDiagnosticColors = { Color.red, Color.yellow, Color.green, Color.blue };

        float[] weaponDiagnosticValues = {
            Player.instance.Weapon.Damage,
            Player.instance.Weapon.RateOfFire,
            Player.instance.Weapon.ReloadTime,
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

        for (var i = 0; i < 5; i++)
        {
            WeaponDiagnosticBars[i].fillAmount = weaponDiagnosticValues[i] / weaponDiagnosticMaxs[i];
            WeaponDiagnosticValues[i].text = weaponDiagnosticValues[i] + "";
            if (WeaponDiagnosticBars[i].fillAmount > 0.95)
            {
                WeaponDiagnosticBars[i].color = weaponDiagnosticColors[3];
            }
            else if (WeaponDiagnosticBars[i].fillAmount > 0.66)
            {
                WeaponDiagnosticBars[i].color = weaponDiagnosticColors[2];
            }
            else if (WeaponDiagnosticBars[i].fillAmount > 0.33)
            {
                WeaponDiagnosticBars[i].color = weaponDiagnosticColors[1];
            }
            else
            {
                WeaponDiagnosticBars[i].color = weaponDiagnosticColors[0];
            }
        }
        
    }


}
