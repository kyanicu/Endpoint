using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using oteTag = GameManager.OneTimeEventTags;

public class ParadigmTerminal : InteractableEnv
{
    public bool ParadigmGranted { private get; set; }

    private void Awake()
    {
        functionalityText = "unlock a new Paradigm.";
    }

    /// <summary>
    /// Unlocks a new paradigm provided through the Overlay Manager
    /// </summary>
    public override void ActivateFunctionality()
    {
        if (!ParadigmGranted)
        {
            //Add a new Paradigm to the terminal, add to OneTimeEvent
            GameManager.OneTimeEvents.Add(name, oteTag.Console);
            UpgradesOverlayManager.AddParadigm(new Paradigm());
            ParadigmGranted = true;
        }
    }
}
