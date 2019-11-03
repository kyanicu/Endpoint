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

    public void UpdateHUD(Player p)
    {
        UpdateAmmo(p);
        UpdateWeapon(p);
        UpdatePlayer(p);
        WeaponPM.UpdateWeaponDiagnostic(p);
        CharacterPM.UpdateCharacterClass(p);
    }

    public void UpdateAmmo(Player p)
    {
        WeaponPM.UpdateAmmo(p.Weapon);
    }

    public void UpdateWeapon(Player p)
    {
        WeaponPM.UpdateWeapon(p);
    }

    public void UpdatePlayer(Player p)
    {
        CharacterPM.UpdateHealth(p.MaxHealth, p.Health);
    }

    public void UpdateSwap(float rechargeTime)
    {
        SwapPM.RechargeSwap(rechargeTime);
    }

    private void Start()
    {
        // Close the diagnostic panels and set boolean to false.
        toggleDiagnosticPanels();
    }

    /// <summary>
    /// Toggles the visibility for diagnostic panels
    /// </summary>
    public void toggleDiagnosticPanels()
    {
        diagnosticPanelsVisible = !diagnosticPanelsVisible;
        WeaponPM.WeaponDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
        CharacterPM.CharacterDiagnosticInfoPanel.SetActive(diagnosticPanelsVisible);
    }
}
