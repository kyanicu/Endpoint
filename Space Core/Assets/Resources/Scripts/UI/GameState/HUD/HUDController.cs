using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HUDController : MonoBehaviour
{

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
    public CharacterPanelManager CharacterPM;
    public WeaponPanelManager WeaponPM;
    public SwapPanelManager SwapPM;
    public MinimapController Minimap;

    // Stores the current HUD highlight color (set by the class the player is occupying)
    public Color activeClassColor;

    private bool diagnosticPanelsVisible = true;
    private bool firstRun = true;

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
    /// Updates every section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateHUD(Player p)
    {
        UpdateAmmo(p);
        UpdateWeapon(p);
        UpdatePlayer(p);
        WeaponPM.UpdateWeaponDiagnostic(p);
        CharacterPM.UpdateCharacterClass(p);
        CharacterPM.UpdateCharacterAbilities(p);
        RecolorHUD();

        if (firstRun)
        {
            // This needs to be run at the start only once, but after the player's class has been extracted.
            // Close the diagnostic panels and set boolean to false.
            toggleDiagnosticPanelsInitial();
            firstRun = false;
        }
    }

    /// <summary>
    /// Updates the ammo section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateAmmo(Player p)
    {
        WeaponPM.UpdateAmmo(p.Weapon);
    }

    /// <summary>
    /// Updates the weapon section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateWeapon(Player p)
    {
        WeaponPM.UpdateWeapon(p);
    }

    /// <summary>
    /// Updates the character section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdatePlayer(Player p)
    {
        CharacterPM.UpdateHealth(p.MaxHealth, p.Health);
    }

    /// <summary>
    /// Updates the swap section of the HUD
    /// </summary>
    /// <param name="rechargeTime"></param>
    public void UpdateSwap(float rechargeTime)
    {
        SwapPM.RechargeSwap(rechargeTime);
    }

    private IEnumerator updateAbilityCooldownUIRoutine;

    /// <summary>
    /// Updates the ability image in the character section of the HUD
    /// </summary>
    /// <param name="seconds"></param>
    public void StartAbilityCooldown(float seconds)
    {
        if (updateAbilityCooldownUIRoutine != null)
        {
            StopCoroutine(updateAbilityCooldownUIRoutine);
        }
        updateAbilityCooldownUIRoutine = CharacterPM.UpdateAbilityCooldownUI(seconds);
        StartCoroutine(updateAbilityCooldownUIRoutine);
    }

    // Recolors all elements within the HUD to match the current character's class.
    public void RecolorHUD()
    {
        CharacterPM.RecolorCharacterHUD();
        SwapPM.RecolorSwapHUD();
    }

    // Function that initializes the diagnostic panel visibility at start.
    public void toggleDiagnosticPanelsInitial()
    {
        diagnosticPanelsVisible = false;
        CharacterPM.CharacterDiagnosticInfoPanel.SetActive(false);
        WeaponPM.WeaponDiagnosticInfoPanel.SetActive(false);
        CharacterPM.hideDiagnosticPanelCharInitial();
    }

    /// <summary>
    /// Toggles the visibility for diagnostic panels.
    /// </summary>
    public void toggleDiagnosticPanels()
    {
        diagnosticPanelsVisible = !diagnosticPanelsVisible;

        // If the bool is now true, play the show animations.
        if (diagnosticPanelsVisible)
        {
            //slideAnimator.Play("DiagnosticLeftIn");
            if (charDiagnosticPanel != null)
            {
                StopCoroutine(charDiagnosticPanel);
            }
            charDiagnosticPanel = CharacterPM.showDiagnosticPanelChar();
            StartCoroutine(charDiagnosticPanel);

            WeaponPM.WeaponDiagnosticInfoPanel.SetActive(true);
        }
        // If the bool is now false, play the hide animations. 
        else
        {
            if (charDiagnosticPanel != null)
            {
                StopCoroutine(charDiagnosticPanel);
            }
            charDiagnosticPanel = CharacterPM.hideDiagnosticPanelChar();
            StartCoroutine(charDiagnosticPanel);

            WeaponPM.WeaponDiagnosticInfoPanel.SetActive(false);
        }
    }

    public void UpdateMinimap(string section, string area)
    {
        if (section == "")
        {
            section = GameManager.Section;
        }
        else
        {
            GameManager.Section = section;
        }
        Minimap.UpdateLocation(section, area);
    }

    private IEnumerator charDiagnosticPanel;
}
