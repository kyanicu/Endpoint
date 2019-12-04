using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class LoadDialogue
{
    public static Dictionary<string, DialogueItem> DialogueItems = new Dictionary<string, DialogueItem>();

    public enum DialogueLineID
    {
        SpeakerName,
        IconPath,
        Content
    }

    [Serializable]
    public struct DialogueItem
    {
        public string SpeakerName;
        public string IconPath;
        public List<string> Content;

        //Basic constructor for a dialogue item
        public DialogueItem(string speaker, string iconPath, List<string> content)
        {
            SpeakerName = speaker;
            IconPath = "Images/Icons/" + iconPath;
            Content = content;
        }
    }

    /// <summary>
    /// Retrieves all Diagloue items from dialogue txt files
    /// </summary>
    public static void LoadDialogueItems()
    {
        //Get each text file in the primary objectives folder
        TextAsset[] DialogueList = Resources.LoadAll<TextAsset>("Text/Dialogue");

        //Loop through each file in folder
        foreach (TextAsset txt in DialogueList)
        {
            DialogueItem newDialogue = LoadDialogueFromText(txt);
            DialogueItems.Add(txt.name, newDialogue);
        }
    }

    /// <summary>
    /// Reads and loads an objective given a text file
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private static DialogueItem LoadDialogueFromText(TextAsset txt)
    {
        //Split text file lines into an array
        string[] fLines = txt.text.Split("\n"[0]);

        int index = fLines[(int)DialogueLineID.SpeakerName].IndexOf(": ") + 2;
        string speaker = fLines[(int)DialogueLineID.SpeakerName].Substring(index);

        index = fLines[(int)DialogueLineID.IconPath].IndexOf(": ") + 2;
        string iconType = fLines[(int)DialogueLineID.IconPath].Substring(index);

        List<string> content = new List<string>();
        for (int x = (int)DialogueLineID.Content; x < fLines.Length; x++)
        {
            content.Add(fLines[x]);
        }

        // public Objective(string name, string descr, string subdescr, int ntc, string iconType, bool iuso)
        return new DialogueItem(speaker, iconType, content);
    }
}
