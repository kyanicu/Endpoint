using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LD = LoadDialogue;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI Content;
    public Image SpeakerIcon;

    private const float typeTimer = .1f;
    private const float timeBetweenDialogue = 1.5f;

    /// <summary>
    /// Called by HUD manager and passed a key to load a dialogue item from LoadDialogue
    /// </summary>
    /// <param name="key"></param>
    public void LoadDialogue(string key)
    {
        //Retrieve the dialogue item given the key
        LD.DialogueItem dialogue = LD.DialogueItems[key];

        try
        {

            SpeakerIcon.sprite = Resources.Load<Sprite>(dialogue.IconPath);
        }
        catch(System.Exception e)
        {
            SpeakerIcon.sprite = null;
        }
        SpeakerName.text = dialogue.SpeakerName;
        Content.text = "";

        StartCoroutine(loadText(dialogue));
    }

    /// <summary>
    /// Coroutine responsible for typing out 1 char at a time to dialogue window
    /// </summary>
    /// <param name="dialogue"></param>
    /// <returns></returns>
    private IEnumerator loadText(LD.DialogueItem dialogue)
    {
        //Loop through each line of dialogue's content
        foreach(string line in dialogue.Content)
        {
            //reset content text
            Content.text = "";

            //Loop through each character in line
            int counter = 0;
            while(counter < line.Length)
            {
                //Add the character to the text element
                Content.text += line[counter];
                counter++;

                //Wait the brief time amount before typing next letter
                yield return new WaitForSeconds(typeTimer);
            }
            yield return new WaitForSeconds(timeBetweenDialogue);
        }
        yield return new WaitForSeconds(timeBetweenDialogue);

        //Hide dialogue window after all text has been displayed
        gameObject.SetActive(false);
    }
}
