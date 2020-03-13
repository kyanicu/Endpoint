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
    public ExperiencePanelManager ExperiencePM;
    public MinimapController MinimapManager;
    public DialogueManager DialogueManager;
    public PopupManager PopupManager;
    public LoadingScreenManager LoadingManager;

    // Stores the current HUD highlight color (set by the class the player is occupying)
    public Color activeClassColor;
    public bool ObjectivesPopupIsActive;
    public string[] RecentDataBaseEntry;
    public Color activeWeaponColor;

    private bool diagnosticPanelsVisible = true;
    private bool firstRun = true;
    public bool visible { get; set; }
    private static HUDController _instance;
    public static HUDController instance { get { return _instance; } }

    private void Awake()
    {
        //Setup singleton
        if (_instance == null)
        {
            _instance = this;
        }

        //If a game is being loaded, update minimap for save room
        if(SaveSystem.loadedData != null)
        {
            instance.UpdateMinimap(GameManager.Sector, "Save Room");
        }
    }
    private void Start()
    {
        visible = true;
    }
    
    /// <summary>
    /// Updates every Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateHUD(Character p)
    {
        //Can only update HUD if not currently reloading a save file
        if (SaveSystem.loadedData == null && visible)
        {
            UpdateAmmo(p);
            UpdateWeapon(p);
            UpdatePlayer(p);
            WeaponPM.UpdateWeaponDiagnostic(p);
            CharacterPM.UpdateCharacterClass(p);
            CharacterPM.UpdateCharacterAbilities(p);
            RecolorHUD();

            ExperiencePM.UpdateExperienceFromSystem();

            // After updating the HUD, also make sure to update the player's worldspace canvas.
            p.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().UpdateAsPlayerCanvas(p);
        }
    }

    /// <summary>
    /// Updates every Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void InitalHUDStart()
    {
        //Can only update HUD if not currently reloading a save file
        if (SaveSystem.loadedData == null && visible)
        {
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
    public void UpdateAmmo(Character c)
    {
        WeaponPM.UpdateAmmo(c.Weapon);

        // After updating the HUD, also make sure to update the player's worldspace canvas.
        c.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().UpdateAsPlayerCanvas(c);
    }

    /// <summary>
    /// Updates the weapon Section of the HUD
    /// </summary>
    /// <param name="p"></param>
    public void UpdateWeapon(Character c)
    {
        WeaponPM.UpdateWeapon(c);
    }

    /// <summary>
    /// Updates the character Section of the HUD
    /// </summary>
    /// <param name="c"></param>
    public void UpdatePlayer(Character c)
    {
        CharacterPM.UpdateHealth(c.MaxHealth, c.Health);

        // After updating the HUD, also make sure to update the player's worldspace canvas.
        c.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().UpdateAsPlayerCanvas(c);
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

    /// <summary>
    /// Updates the Experience Section of the HUD
    /// </summary>
    public void UpdateExperiencePanel(int level, int experience, int nextLevelExperience)
    {
        ExperiencePM.UpdateExperience(level, experience, nextLevelExperience);
    }

    /// <summary>
    /// Updates the minimap with the player's current sector/room
    /// </summary>
    /// <param name="sector"></param>
    /// <param name="room"></param>
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
        MinimapManager.UpdateLocation(sector, room);
    }

    /// <summary>
    /// Contacts the dialogue manager to load dialogue into window
    /// Call with: HUDController.instance.InitiateDialogue(nameOfDialogueItem);
    /// </summary>
    /// <param name="DialogueKey"></param>
    public void InitiateDialogue(string DialogueKey)
    {
        //Unhide the dialogue window
        DialogueManager.gameObject.SetActive(true);

        //Call the manager's function to load the dialogue into the window
        DialogueManager.LoadDialogue(DialogueKey);
    }

    /// <summary>
    /// Activates objectives popup on screen and loads content onto it
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void InitiateObjectivesPopup(string title, string content)
    {
        //If popup is currently inactive
        if (!ObjectivesPopupIsActive)
        {
            ObjectivesPopupIsActive = true;
            PopupManager.InitiateObjectivesPopup(title, content);
        }
    }

    /// <summary>
    /// Activates database popup on screen
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void InitiateDatabasePopup(string title, string content)
    {
        //If popup is currently inactive
        if (RecentDataBaseEntry != null)
        {
            string[] arr = { title, content };
            RecentDataBaseEntry = arr;
            PopupManager.InitiateDatabasePopup();
        }
    }

    /// <summary>
    /// Automatically closes an open database popup
    /// </summary>
    public void CloseDataBasePopup()
    {
        PopupManager.CloseDBPopup();
    }

    /// <summary>
    /// Activates Save popup on screen
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void InitiateSavePopup()
    {
        PopupManager.InitiateSavePopup();
    }

    /// <summary>
    /// Toggles the visibility of HUD upon closing/opening overlay
    /// </summary>
    public void ToggleHUDVisibility()
    {
        CharacterPM.gameObject.SetActive(visible);
        WeaponPM.gameObject.SetActive(visible);
        SwapPM.gameObject.SetActive(visible);
        MinimapManager.gameObject.SetActive(visible);
        PopupManager.gameObject.SetActive(visible);
        InitalHUDStart();
    }
}
