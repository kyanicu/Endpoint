﻿using System.Collections.Generic;
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
<<<<<<< HEAD
=======
    public TextMeshProUGUI OverlayHeader;
    public Image OverlayHeaderIcon;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
    public List<WheelUpgrade> UpgradeGameObjects;
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
    public TextMeshProUGUI Level;
    public TextMeshProUGUI ParadigmName;
    public TextMeshProUGUI Equipped;
    public Image WheelCenter;
    public TextMeshProUGUI[] ParadigmUpgradeNames;
    public Image[] ParadigmUpgradeIcons;
    public Transform ParadigmTreeGroup;
    private Sprite[] normalBranchSprites;
    private Sprite[] dottedBranchSprites;
<<<<<<< HEAD
    private Sprite[] upgradeIconsSelected;
    private Sprite[] upgradeIconsUnselected;
=======
    private Sprite[] upgradeIcons;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
    public int PlayerLevel = 0;
=======
    public static int PlayerLevel = 5;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
        //Commenting out as functionality will now be done with terminals
        //for (int x = 0; x < 10; x++)
            //UnlockedParadigms.Add(new Paradigm());
        Upgrades = new List<Upgrade>();
        Upgrade u = new Upgrade("Stealth Identity", "STEALTH.ID", "After using the hack target ability, enemies can not detect you for a few seconds.", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Fortification Chipset", "FTFY.CHIP", "Using the hack target ability on enemies heals them.", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Scorched Earth", "SCORCH.ZIP", "Completing a hack into a new chassis causes the old chassis to receive damage.", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Tactical Trojan", "TROJ.TAC", "Cancelling the hack target ability causes the enemy to receive damage.", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Force Compensator", "FORCE.COMP", "You gain a passive shield which reduces damage from the front, but you take more damage from the back.", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Defense Module", "DEF.MOD", "While reloading, you are rendered invulnerable.", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("Salvage Scanner", "SALV~GE.SCAN", "When picking up ammo, you gain a small amount of HP, but you gain less ammo than usual.", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Active Protection Module", "PROTECT.MOD", "You gain a passive shield which completely absorbs a single shot from any source before having to recharge.", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Tactical Protocol", "TAC_MAG.PRO", "You get larger magazines, but your reloads are slower.", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("Reverse Engineering", "REVERSI.ENG", "When you get shot and damaged by an enemy, you receive ammo proportional to the damage sustained.", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("Vampire Subroutine", "VAMPIRE.SUB", "When you deal damage to an enemy, you regain HP proportional to the damage sustained.", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        u = new Upgrade("Reload Optimizer", "RLD.OPTIMIZE", "Reloading on an empty magazine is much faster than normal, while reloading on a partial magazine is slower.", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        ///--------------------------------------------------------------------------------------------------------------
        normalBranchSprites = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/wheel-lines/solid");
        dottedBranchSprites = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/wheel-lines/dashed");

        // Unselected sprites for upgrades
        upgradeIconsUnselected = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/icons-unselected");
        // Selected sprites for upgrades
        upgradeIconsSelected = Resources.LoadAll<Sprite>("Images/UI/Overlay/upgrades/icons-selected");

=======
        for (int x = 0; x < 10; x++)
            UnlockedParadigms.Add(new Paradigm());
        Upgrades = new List<Upgrade>();
        Upgrade u = new Upgrade("UpgradeA", "UPA", "Temporary Upgrade A", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeB", "UPB", "Temporary Upgrade B", new int[] { 2, 0, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeC", "UPC", "Temporary Upgrade C", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeD", "UPD", "Temporary Upgrade D", new int[] { 1, 1, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeE", "UPE", "Temporary Upgrade E", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeF", "UPF", "Temporary Upgrade F", new int[] { 0, 2, 0 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeG", "UPG", "Temporary Upgrade G", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeH", "UPH", "Temporary Upgrade H", new int[] { 0, 1, 1 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeI", "UPI", "Temporary Upgrade I", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeJ", "UPJ", "Temporary Upgrade J", new int[] { 0, 0, 2 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeK", "UPK", "Temporary Upgrade K", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        u = new Upgrade("UpgradeL", "UPL", "Temporary Upgrade L", new int[] { 1, 0, 1 });
        Upgrades.Add(u);
        ///--------------------------------------------------------------------------------------------------------------
        normalBranchSprites = Resources.LoadAll<Sprite>("Images/UI/UpgradesOverlay/NormalBranches");
        dottedBranchSprites = Resources.LoadAll<Sprite>("Images/UI/UpgradesOverlay/DottedBranches");
        upgradeIcons = Resources.LoadAll<Sprite>("Images/UI/UpgradesOverlay/UpgradeIcons");
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
    }

<<<<<<< HEAD
    private void Start()
    {
        EquipNewParadigm(equippedParadigmID);
    }

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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

<<<<<<< HEAD
        // Set icon to use currently selected icon sprite
        SelectedUpgradeIcon.sprite = upgradeIconsUnselected[nodeID];

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        for (int index = 0; index < u.DispositionValues.Length; index++)
        {
            SelectedDispositionValues[index].text = $"+{u.DispositionValues[index]}{DispositionAbreviation[index]}";
        }
    }

<<<<<<< HEAD
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
        //Don't run functionality if we don't have any paradigms
        if (UnlockedParadigms.Count == 0) 
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

    public void SetLevel(int level)
    {
        PlayerLevel = level;
    }

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD

            
        }

        for (int i = 0; i < 12; i++)
        {
            // Set corresponding icon sprites to unselected.
            Nodes[i].sprite = upgradeIconsUnselected[i];
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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

<<<<<<< HEAD
                if (upgradeIconsUnselected.Length > 0)
                {
                    ParadigmUpgradeIcons[x].sprite = upgradeIconsUnselected[x];
=======
                //Remove when icons are in <--------------------------------------
                if (upgradeIcons.Length > 0)
                {
                    ParadigmUpgradeIcons[x].sprite = upgradeIcons[x];
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
                }
                ParadigmUpgradeNames[x].text = u.ShortName;

                //Check if branch should be a solid line
                if (playerLevel >= x)
                {
<<<<<<< HEAD
                    if (normalBranchSprites.Length > 0)
                    {
                        Branches[branchID].sprite = normalBranchSprites[branchID];

                        // Set corresponding icon sprites to selected.
                        Nodes[branchID].sprite = upgradeIconsSelected[branchID];
                    }

=======
                    //Remove when icons are in <--------------------------------------
                    if (normalBranchSprites.Length > 0)
                    {
                        Branches[branchID].sprite = normalBranchSprites[x];
                    }
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
                    //Iterate through each disposition and calculate new dis values
                    for (int i = 0; i <= (int)Disposition.VirtualRanger; i++)
                    {
                        currentDisValues[i] += u.DispositionValues[i];
                    }
                }
                else
                {
                    //Make upgrade list element transparent
<<<<<<< HEAD
                    ParadigmUpgradeIcons[x].color = ColorWithVariedTransparency(ParadigmUpgradeIcons[x].color, .2f);
                    ParadigmUpgradeNames[x].color = ColorWithVariedTransparency(ParadigmUpgradeNames[x].color, .2f);
=======
                    ParadigmUpgradeIcons[x].color = ColorWithVariedTransparency(ParadigmUpgradeIcons[x].color, .5f);
                    ParadigmUpgradeNames[x].color = ColorWithVariedTransparency(ParadigmUpgradeNames[x].color, .5f);
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

                    //Check if branch should be a dotted line
                    if (x - 1 == playerLevel)
                    {
                        //Remove when icons are in <--------------------------------------
                        if (dottedBranchSprites.Length > 0)
                        {
<<<<<<< HEAD
                            Branches[branchID].sprite = dottedBranchSprites[branchID];
=======
                            Branches[branchID].sprite = dottedBranchSprites[x];
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
                        }
                    }
                    //Check if branch should be a faded solid line
                    else
                    {
                        //Remove when icons are in <--------------------------------------
                        if (normalBranchSprites.Length > 0)
                        {
<<<<<<< HEAD
                            Branches[branchID].sprite = normalBranchSprites[branchID];
                        }
                        Branches[branchID].color = ColorWithVariedTransparency(ParadigmUpgradeNames[x].color, .2f);
=======
                            Branches[branchID].sprite = normalBranchSprites[x];
                        }
                        Branches[branchID].color = ColorWithVariedTransparency(Branches[branchID].color, .5f);
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
            //Branches[i].color = dominantCol;
            Branches[i].color = ColorWithVariedTransparency(dominantCol, Branches[i].color.a);
=======
            Branches[i].color = dominantCol;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
        //OverlayHeader.color = dominantCol;
        //OverlayHeaderIcon.color = dominantCol;
=======
        OverlayHeader.color = dominantCol;
        OverlayHeaderIcon.color = dominantCol;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

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
<<<<<<< HEAD

    /// <summary>
    /// Add Paradigm to the UnlockedParadigm list
    /// </summary>
    /// <returns></returns>
    public static void AddParadigm(Paradigm paradigm)
    {
        UnlockedParadigms.Add(paradigm);
    }
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
}
