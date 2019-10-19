using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image healthBar, ammoBar, swapBar;
    public Text healthAmt, ammoAmt;

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
}
