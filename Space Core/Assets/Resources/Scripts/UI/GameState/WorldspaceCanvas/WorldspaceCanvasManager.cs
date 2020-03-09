using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldspaceCanvasManager : MonoBehaviour
{
    // Ammo reloaded text
    public TextMeshProUGUI ammoText;

    // Enemy HP bar
    public GameObject EnemyHPBarPanel;
    public Image WSCEnemyHPIcon, WSCEnemyHPBarBG, WSCEnemyHPBar;

    // Player HP bar
    public GameObject PlayerHPBarPanel;
    public Image WSCPlayerHPIcon, WSCPlayerHPBarBG, WSCPlayerHPBar;

    // Player AMMO bar
    public GameObject PlayerAmmoBarPanel;
    public Image WSCPlayerAmmoIcon, WSCPlayerAmmoBarBG, WSCPlayerAmmoBar;

    // Colors used for recoloring elements on worldspace canvas.
    private Color EnemyHPColor = new Color32(0x00, 0xff, 0xfe, 0xff);
    private Color PlayerHPColor = new Color32(0x34, 0xFC, 0x23, 0xff);
    // Weapon color is determined by the current weapon of the player.

    // Initialize this worldspace canvas as an enemy worldspace canvas.
    public void InitializeAsEnemyCanvas(Character e)
    {
        // Hide ammo Text.
        ammoText.DOFade(0f, 0f);

        // Show enemy HP bar.
        EnemyHPBarPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        // Set colors of enemy HP bar.
        WSCEnemyHPIcon.DOColor(EnemyHPColor, 0f);
        WSCEnemyHPBar.DOColor(EnemyHPColor, 0f);

        // Hide player bars.
        PlayerHPBarPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        PlayerAmmoBarPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);

        UpdateAsEnemyCanvas(e);
    }

    // Update this worldspace canvas as an enemy canvas.
    public void UpdateAsEnemyCanvas(Character e)
    {
        // Update enemy HP.
        WSCEnemyHPBar.DOFillAmount(e.Health / e.MaxHealth, 0.3f);
    }

    // Initialize this worldspace canvas as a player's worldspace canvas.
    public void InitializeAsPlayerCanvas(Character p)
    {
        // Hide ammo Text.
        ammoText.DOFade(0f, 0f);

        // Hide enemy HP bar.
        EnemyHPBarPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);

        // Show player bars.
        PlayerHPBarPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        PlayerAmmoBarPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        // Set player bar colors.
        // Set HP bar color.
        WSCPlayerHPIcon.DOColor(PlayerHPColor, 0f);
        WSCPlayerHPBar.DOColor(PlayerHPColor, 0f);

        // Set Ammo bar color. (based on player's current weapon color.)
        Color targetColor = HUDController.instance.WeaponPM.RetrieveCorrespondingWeaponColor(p.Weapon);
        WSCPlayerAmmoIcon.DOColor(targetColor, 0f);
        WSCPlayerAmmoBar.DOColor(targetColor, 0f);

        // Perform initial update.
        UpdateAsPlayerCanvas(p);
    }

    // Update this worldspace canvas as a player canvas.
    public void UpdateAsPlayerCanvas(Character p)
    {
        // Update player HP.
        WSCPlayerHPBar.DOFillAmount(p.Health / p.MaxHealth, 0.3f);

        // Update player Ammo.
        WSCPlayerAmmoBar.DOFillAmount((float)p.Weapon.AmmoInClip / p.Weapon.ClipSize, 0.05f);
    }

    private Tween tweenAmmoText;

    /// Updates text above player when they recieve ammo.
    public void UpdatePlayerAmmo()
    {
        tweenAmmoText = ammoText.DOFade(1f, 0f);
        tweenAmmoText = ammoText.DOFade(0f, 1f);
    }

}
