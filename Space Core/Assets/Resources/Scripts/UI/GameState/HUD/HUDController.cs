using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("HUD Component Managers")]
    public CharacterPanelManager CharacterPM;
    public WeaponPanelManager WeaponPM;
    public SwapPanelManager SwapPM;
    public MinimapController Minimap;
    
    [Header("Loading Screen")]
    public Image LoadingScreen;
    public MainMenuAnimations MainMenuAnims;
    public TextMeshProUGUI LoadingText;

    // Stores the current HUD highlight color (set by the class the player is occupying)
    public Color activeClassColor;

    private bool diagnosticPanelsVisible = true;
    private bool firstRun = true;
    private static HUDController _instance;
    public static HUDController instance { get { return _instance; } }

    private void Awake()
    {
        //Setup singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        //If a game is being loaded, update minimap for save room
        if(SaveSystem.loadedData != null)
        {
            HUDController.instance.UpdateMinimap(GameManager.Sector, "Save Room");
        }

        //Empty saved data cache as confirmation that data was successfully loaded
        SaveSystem.loadedData = null;

        //Update minimap with player's position and HUD
        UpdateHUD(Player.instance);
    }
    private void Start()
    {
        LoadingText.text = "LOADING";
        StartCoroutine(FadeOutLoadingScreen());
    }

    private IEnumerator FadeOutLoadingScreen()
    {
        // Run the main menu logo's glitching image animation.
        StartCoroutine(MainMenuAnims.AnimationMenuLogoGlitchImage());

        // Run the helper function for animating the tinybits around the logo.
        MainMenuAnims.AnimationTinybitHelper();

        // Run the helper function for animating the text of the tinybits around the logo.
        MainMenuAnims.AnimationTinybitTextHelper();
        for (int x = 0; x < 3; x++)
        {
            yield return new WaitForSeconds(.5f);
            LoadingText.text += ".";
        }
        Destroy(LoadingText, .25f);
        float timer = 1f;
        float counter = 0;
        while (counter < timer)
        {
            counter += .025f;
            Color updatedAlpa = LoadingScreen.color;
            updatedAlpa.a = timer - counter;
            LoadingScreen.color = updatedAlpa;
            yield return null;
        }
        Destroy(LoadingScreen.gameObject);
    }

    /// <summary>
    /// Updates every Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateHUD(Player p)
    {
        //Can only update HUD if not currently reloading a save file
        if (SaveSystem.loadedData == null)
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
    }

    /// <summary>
    /// Updates the ammo Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateAmmo(Player p)
    {
        WeaponPM.UpdateAmmo(p.Weapon);
    }

    /// <summary>
    /// Updates the weapon Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateWeapon(Player p)
    {
        WeaponPM.UpdateWeapon(p);
    }

    /// <summary>
    /// Updates the character Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdatePlayer(Player p)
    {
        CharacterPM.UpdateHealth(p.MaxHealth, p.Health);
    }

    /// <summary>
    /// Updates the swap Section of the HUD
    /// </summary>
    /// <param name="rechargeTime"></param>
    public void UpdateSwap(float rechargeTime)
    {
        SwapPM.RechargeSwap(rechargeTime);
    }

    private IEnumerator updateAbilityCooldownUIRoutine;

    /// <summary>
    /// Updates the ability image in the character Section of the HUD
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
        WeaponPM.hideDiagnosticPanelWeaponInitial();
    }

    // Stores the coroutines for the diagnostic panel animations.
    private IEnumerator charDiagnosticPanel;
    private IEnumerator weaponDiagnosticPanel;

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

            if (weaponDiagnosticPanel != null)
            {
                StopCoroutine(weaponDiagnosticPanel);
            }
            weaponDiagnosticPanel = WeaponPM.showDiagnosticPanelWeapon();
            StartCoroutine(weaponDiagnosticPanel);
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

            if (weaponDiagnosticPanel != null)
            {
                StopCoroutine(weaponDiagnosticPanel);
            }
            weaponDiagnosticPanel = WeaponPM.hideDiagnosticPanelWeapon();
            StartCoroutine(weaponDiagnosticPanel);
        }
    }

    public void UpdateMinimap(string sector, string room)
    {
        if (sector == "")
        {
            sector = GameManager.Sector;
        }
        else
        {
            GameManager.Sector = sector;
        }
        Minimap.UpdateLocation(sector, room);
    }
}
