using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTerminal : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string PopupText;
    public string QuestName;

    public bool AlreadyPressed { private get; set; }

    private void Awake()
    {
        functionalityText = PopupText;
    }

    /// <summary>
    /// Unlocks the lore entry provided through inspector
    /// </summary>
    public override void ActivateFunctionality()
    {
        if (!AlreadyPressed)
        {
            LoadObjectives.ProgressObjective(QuestName);

            AlreadyPressed = true;
        }
    }
}
