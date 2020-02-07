using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesOverlayManager : MonoBehaviour
{
    [Header("Disposition Panel")]
    #region Attached GameObjects [Disposition Panel]
    public List<TextMeshProUGUI> DispositionHeaders;
    public Image[] IconBackgrounds;
    public Image[] Icons;
    public List<TextMeshProUGUI> DispositionModValues;
    #endregion

    [Header("Upgrade Details")]
    public Image SelectedUpgradeIcon;
    public TextMeshProUGUI SelectedIconShortName;
    public TextMeshProUGUI SelectedIconFullName;
    public TextMeshProUGUI SelectedIconDescription;
    public TextMeshProUGUI[] SelectedDispositionValues;

    [Header("Wheel Panel")]
    #region Attached GameObjects [Wheel Panel]
    public List<Image> Nodes;
    public List<Image> Branches;
    public List<WheelUpgrade> UpgradeGameObjects;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI ParadigmName;
    public TextMeshProUGUI Equipped;
    public Image WheelCenter;
    public TextMeshProUGUI[] ParadigmUpgradeNames;
    public Image[] ParadigmUpgradeIcons;
    public Transform ParadigmTreeGroup;
    private Sprite[] normalBranchSprites;
    private Sprite[] dottedBranchSprites;
    private Sprite[] upgradeIcons;
    private string dominantDispositionTxt = "white";
    #endregion

    #region Static Disposition Stuff
    public static Color[] DispositionColors = 
    {
        Color.magenta,
        Color.cyan,
        new Color(1, 1, 0, 1),
        Color.black
    };

    public static string[] DispositionAbreviation = 
    {
        "RP",
        "CS",
        "VR",
        ""
    };
    public enum Disposition
    {
        RougeProgram,
        CoreSentinel,
        VirtualRanger,
        None = default
    }

    public static string[] DispositionMod =
    {
        "0% HACK SPEED",
        "0% HIT POINTS",
        "0% FIRE RATE",
        ""
    };
    #endregion
    public static int PlayerLevel = 5;
    public Transform Reticle;
    private const int maxLevel = 15;
    public static List<Paradigm> UnlockedParadigms { get; private set; }
    private static List<Upgrade> Upgrades;
    private static int equippedParadigmID = 0;
    private int selectedParadigmID = 0;
    private float reticleSpeed = 150f;
    private Vector3 reticleStartPos;
    private Vector3 paradigmTreeGroupStartPos;
    private bool lockReticle;
    private bool lockParadigmSwap;

    private void Awake()
    {
        ///PROTOTYPING - GENERATE PARADIGM TESTS AND TEMPLATE UPGRADES-----------------------------------------------
        UnlockedParadigms = new List<Paradigm>();
        for (int x = 0; x < 10; x++)
            UnlockedParadigms.Add(new Paradigm());
        Upgrades = new List<Upgrade>();
        Upgrade u = new Upgrade("IFF Spoofer", "IFF_STEALTH.ZIP", "Nearby enemies can’t see you for a few seconds after hack", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Fortification Chipset", "FTFY.EXE", "Heal enemy bots with your hack bullet", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Scorched Earth", "SCORCH.ZIP", "Damage enemy you hack out of", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Tactical Trojan", "IYBKYD.EXE", "Cancelling a hack deals damage to target", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Force Compensator", "FORC.PI", "More damage from behind less damage from front", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Defense Module", "DEF.CAT", "Gain invincibility after reloading for a short while", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Salvage Scanner", "SALVAGE.PI", "Ammo health regen small, pick up less ammo", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Active Protection Module", "APM.CAT", "Shield that defends from x number of shots", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Tactical ", "TACTICAL ", "Larger mag but slow reload", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("Reverse Engineering Protocol", "REVERSI.DLL", "Getting shot gives you bullets proportional to damage", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("Sapper Subroutine", "SAPPER.MAT", "Dealing damage to enemies heals you slightly", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Rate of Fire Optimizer", "HI_ROF.DLL", "Rate of fire increases the more shot till reload", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        ///--------------------------------------------------------------------------------------------------------------
        normalBranchSprites = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/wheel-lines/solid");
        dottedBranchSprites = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/wheel-lines/solid");
        upgradeIcons = Resources.LoadAll<Sprite>("Images/UI/UpgradesOverlay/UpgradeIcons");
        reticleStartPos = Reticle.position;
        paradigmTreeGroupStartPos = ParadigmTreeGroup.position;
    }

    void OnEnable()
    {
        selectedParadigmID = equippedParadigmID;
        Equipped.gameObject.SetActive(false);
        Reticle.position = reticleStartPos;
        ParadigmTreeGroup.position = paradigmTreeGroupStartPos;
        RefreshUpgradesOverlay(equippedParadigmID);
        EquipNewParadigm(equippedParadigmID);
    }

    /// <summary>
    /// Swaps the active paradigm used by the player
    /// </summary>
    /// <param name="direction"></param>
    public void SwapParadigm(bool isRightTrigger)
    {
        if (!lockParadigmSwap)
        {
            lockParadigmSwap = true;
            StartCoroutine(canSwapParadigms());
            //Determine direction to change paradigms in
            float horiz = isRightTrigger ? 1 : -1;

            //If user scrolls to the left
            if (horiz > 0)
            {
                selectedParadigmID--;
                if (selectedParadigmID < 0)
                {
                    selectedParadigmID = UnlockedParadigms.Count - 1;
                }
            }
            //If user scrolls to the right
            else if (horiz < 0)
            {
                selectedParadigmID++;
                if (selectedParadigmID == UnlockedParadigms.Count)
                {
                    selectedParadigmID = 0;
                }
            }
            RefreshUpgradesOverlay(selectedParadigmID);
        }
    }

    /// <summary>
    /// Moves the player controlled reticle on the paradigm map
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveReticle(float x, float y)
    {
        if (!lockReticle)
        {
            float newX = x * reticleSpeed * Time.deltaTime;
            float newY = y * reticleSpeed * Time.deltaTime;
            Vector3 newPos = Reticle.localPosition + new Vector3(newX, newY, 0);

            //Only allow the reticle to move within bounds of the wheel
            if (Mathf.Abs(newPos.x) < 200 && Mathf.Abs(newPos.y) < 200)
            {
                Reticle.position += new Vector3(newX, newY, 0);
                ParadigmTreeGroup.position -= new Vector3(newX / 4, newY / 4, 0);
            }
        }
    }

    /// <summary>
    /// Snaps a reticle to a node and updates the upgrade details panel accordingly
    /// </summary>
    /// <param name="iconID"></param>
    public void SnapReticle(int iconID)
    {
        lockReticle = true;
        StartCoroutine(unlockReticle());
        int nodeID = iconID - 1;
        Reticle.position = Nodes[nodeID].transform.position;
        Upgrade u = Upgrades[nodeID];

        SelectedIconShortName.text = u.ShortName;
        SelectedIconFullName.text = u.FullName;
        SelectedIconDescription.text = u.Description;

        for (int index = 0; index < u.DispositionValues.Length; index++)
        {
            SelectedDispositionValues[index].text = $"+{u.DispositionValues[index]}{DispositionAbreviation[index]}";
        }
    }

    public void EquipNewParadigm()
    {
        if (selectedParadigmID < 0 || selectedParadigmID > UnlockedParadigms.Count)
        {
            return;
        }

        foreach (int branch in UnlockedParadigms[equippedParadigmID].Branches)
        {
            UpgradeGameObjects[branch].DeactivateAbility();
        }

        foreach (int branch in UnlockedParadigms[selectedParadigmID].Branches)
        {
            UpgradeGameObjects[branch].enabled = true;
            UpgradeGameObjects[branch].EnableAbility();
        }

        equippedParadigmID = selectedParadigmID;
    }

    public void EquipNewParadigm(int paradigmID)
    {
        if (paradigmID < 0 || paradigmID > UnlockedParadigms.Count)
        {
            return;
        }

        foreach (int branch in UnlockedParadigms[paradigmID].Branches)
        {
            UpgradeGameObjects[branch].enabled = true;
            UpgradeGameObjects[branch].EnableAbility();
        }

        equippedParadigmID = paradigmID;
    }

    /// <summary>
    /// Refreshes all the information on the Upgrades Overlay
    /// </summary>
    private void RefreshUpgradesOverlay(int paradigmID)
    {
        if (selectedParadigmID < 0 || selectedParadigmID > UnlockedParadigms.Count)
        {
            return;
        }

        #region Disposition Section

        //Unhide each disposition panel in Dispositions Section
        for (int x = 0; x <= (int)Disposition.VirtualRanger; x++)
        {
            //Set header text color to dis color
            DispositionHeaders[x].color = DispositionColors[x];

            //Hide color background
            IconBackgrounds[x].color = ColorWithVariedTransparency(IconBackgrounds[x].color, 0);

            //Set icon color to dis color
            Icons[x].color = DispositionColors[x];
        }
        #endregion

        #region Wheel Section
        Level.text = $"{PlayerLevel}";
        Equipped.gameObject.SetActive(paradigmID == equippedParadigmID);

        //Hide each branch
        foreach (Image branch in Branches)
        {
            branch.sprite = null;
            branch.color = ColorWithVariedTransparency(branch.color, 1);
            branch.gameObject.SetActive(false);
        }

        //Hide Paradigm upgrade list 
        for (int x = 0; x < ParadigmUpgradeNames.Length; x++)
        {
            ParadigmUpgradeNames[x].color = ColorWithVariedTransparency(ParadigmUpgradeNames[x].color, 1);
            ParadigmUpgradeIcons[x].color = ColorWithVariedTransparency(ParadigmUpgradeIcons[x].color, 1);
        }

        if (UnlockedParadigms.Count > 0)
        {
            Paradigm activeParadigm = UnlockedParadigms[selectedParadigmID];
            ParadigmName.text = $"{activeParadigm.Name} <color=white><size=17>PARADIGM</size></color>";
            List<int> ParadigmBranches = activeParadigm.Branches;
            int[] currentDisValues = new int[]{ 0, 0, 0 };

            //Loop through paradigm branches and setup their sprites in the wheel
            for (int x = 0; x < Paradigm.maxActiveBranches; x++)
            {
                //Retrieve which branch we are referencing by
                //converting branchNum [1-5] to branchID [1-12]
                int branchID = ParadigmBranches[x];

                //Offset player lvl for calculations, b/c player lvl 0 has no active branches
                int playerLevel = PlayerLevel - 1;
                Branches[branchID].gameObject.SetActive(true);

                //Update list of paradigm's upgrades
                Upgrade u = Upgrades[branchID];

                //Remove when icons are in <--------------------------------------
                if (upgradeIcons.Length > 0)
                {
                    ParadigmUpgradeIcons[x].sprite = upgradeIcons[x];
                }
                ParadigmUpgradeNames[x].text = u.ShortName;

                //Check if branch should be a solid line
                if (playerLevel >= x)
                {
                    //Remove when icons are in <--------------------------------------
                    if (normalBranchSprites.Length > 0)
                    {
                        Branches[branchID].sprite = normalBranchSprites[branchID];
                    }
                    //Iterate through each disposition and calculate new dis values
                    for (int i = 0; i <= (int)Disposition.VirtualRanger; i++)
                    {
                        currentDisValues[i] += u.DispositionValues[i];
                    }
                }
                else
                {
                    //Make upgrade list element transparent
                    ParadigmUpgradeIcons[x].color = ColorWithVariedTransparency(ParadigmUpgradeIcons[x].color, .5f);
                    ParadigmUpgradeNames[x].color = ColorWithVariedTransparency(ParadigmUpgradeNames[x].color, .5f);

                    //Check if branch should be a dotted line
                    if (x - 1 == playerLevel)
                    {
                        //Remove when icons are in <--------------------------------------
                        if (dottedBranchSprites.Length > 0)
                        {
                            Branches[branchID].sprite = dottedBranchSprites[branchID];
                        }
                    }
                    //Check if branch should be a faded solid line
                    else
                    {
                        //Remove when icons are in <--------------------------------------
                        if (normalBranchSprites.Length > 0)
                        {
                            Branches[branchID].sprite = normalBranchSprites[branchID];
                        }
                        Branches[branchID].color = ColorWithVariedTransparency(Branches[branchID].color, .5f);
                    }
                }
            }

            //Update bonus modifier text
            for (int z = 0; z < DispositionAbreviation.Length - 1; z++)
            {
                //If no bonus, hide text
                if (currentDisValues[z] == 0)
                {
                    DispositionModValues[z].text = DispositionAbreviation[3];
                }
                //Otherwise update bonus text accordingly
                else
                {
                    DispositionModValues[z].text =
                        $"+{currentDisValues[z]}{DispositionAbreviation[z]}<color=white> : +{currentDisValues[z]}{DispositionMod[z]}";
                }
            }

            //Calculate the dominant color based on disposition values
            AdjustUIColors(currentDisValues);
        }
        #endregion
    }

    /// <summary>
    /// Updates the color of every element depending on the dominant disposition
    /// </summary>
    /// <param name="disValues"></param>
    private void AdjustUIColors(int[] disValues)
    {
        Color dominantCol = Color.white;

        #region Color Logic
        //If RP > CS
        if (disValues[0] > disValues[1])
        {
            //If RP > VR
            if (disValues[0] > disValues[2])
            {
                dominantCol = DispositionColors[(int)Disposition.RougeProgram];
            }
            //Else if RP < VR
            else if (disValues[0] < disValues[2])
            {
                dominantCol = DispositionColors[(int)Disposition.VirtualRanger];
            }
            //Else RP == VR
            else
            {
                dominantCol = Color.red;
            }
        }
        //Else if RP < CS
        else if (disValues[0] < disValues[1])
        {
            //If CS > VR
            if (disValues[1] > disValues[2])
            {
                dominantCol = DispositionColors[(int)Disposition.CoreSentinel];
            }
            //Else if CS < VR
            else if (disValues[1] < disValues[2])
            {
                dominantCol = DispositionColors[(int)Disposition.VirtualRanger];
            }
            //Else CS == VR
            else
            {
                dominantCol = Color.green;
            }
        }
        //Else RP == CS
        else
        {
            //If RP/CS > VR
            if (disValues[1] > disValues[2])
            {
                dominantCol = Color.blue;
            }
            //Else if RP/CS < VR
            else if (disValues[1] < disValues[2])
            {
                dominantCol = DispositionColors[(int)Disposition.VirtualRanger];
            }
            //Else RP == CS == VR
            else
            {
                dominantCol = Color.white;
            }
        }
        #endregion

        //Update branch and node colors to match dominant dispositon color
        for (int i = 0; i < Branches.Count; i++)
        {
            Nodes[i].color = dominantCol;
            Branches[i].color = dominantCol;
        }

        //If color is white, everything in the Dispositions Section should already be active
        if (dominantCol != Color.white)
        {
            //Loop through each set of items in the Dispositions Section
            for (int x = 0; x <= (int)Disposition.VirtualRanger; x++)
            {
                Disposition d = (Disposition)x;
                //Check cases for disposition color
                if (((dominantCol == DispositionColors[(int)Disposition.RougeProgram] || dominantCol == Color.red || dominantCol == Color.blue) && d == Disposition.RougeProgram) ||
                    ((dominantCol == DispositionColors[(int)Disposition.CoreSentinel] || dominantCol == Color.green || dominantCol == Color.blue) && d == Disposition.CoreSentinel) ||
                    ((dominantCol == DispositionColors[(int)Disposition.VirtualRanger] || dominantCol == Color.red || dominantCol == Color.green) && d == Disposition.VirtualRanger)  )
                {
                    //Set header text color to black
                    DispositionHeaders[x].color = Color.black;

                    //Hide color background
                    IconBackgrounds[x].color = ColorWithVariedTransparency(IconBackgrounds[x].color, 1);

                    //Set icon color to black
                    Icons[x].color = Color.black;
                }
            }
        }

        //Update paradigm name color to match dominant disposition
        ParadigmName.color = dominantCol;
        Equipped.color = dominantCol;
        WheelCenter.color = dominantCol;
        SelectedUpgradeIcon.color = dominantCol;
        //OverlayHeader.color = dominantCol;
        //OverlayHeaderIcon.color = dominantCol;

        for (int i = 0; i< Paradigm.maxActiveBranches; i++)
        {
            ParadigmUpgradeNames[i].color = dominantCol;
            ParadigmUpgradeIcons[i].color = dominantCol;
        }
    }

    /// <summary>
    /// Quick helper function to change a color's transparency
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private Color ColorWithVariedTransparency(Color c, float transparency)
    {
        return new Color(c.r, c.g, c.b, transparency);
    }

    /// <summary>
    /// Cooldown for how long it takes for the cursor to unsnap
    /// </summary>
    /// <returns></returns>
    private IEnumerator unlockReticle()
    {
        float timer = .5f;
        yield return new WaitForSeconds(timer);
        lockReticle = false;
    }

    /// <summary>
    /// Cooldown for when a player holds down trigger to swap paradigms
    /// </summary>
    /// <returns></returns>
    private IEnumerator canSwapParadigms()
    {
        float timer = .18f;
        yield return new WaitForSeconds(timer);
        lockParadigmSwap = false;
    }
}
