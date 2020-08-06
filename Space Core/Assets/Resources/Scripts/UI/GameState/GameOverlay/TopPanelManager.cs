using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TopPanelManager : MonoBehaviour
{
    public TextMeshProUGUI textClock;

    public Image TopPanelC1;
    public Image TopPanelC3;
    public Image TopPanelC4;

    [SerializeField]
    private Sprite[] TopPanelC4Images;

    private static TopPanelManager _instance;
    public static TopPanelManager instance { get { return _instance; } }

    private void OnEnable()
    {
        // Run Top Panel icon animations.
        TriggerTopPanelCAnimations();
    }

    public void TriggerTopPanelCAnimations()
    {
        // Run animation functions for various top panel items.
        StartCoroutine(AnimateC1());
        StartCoroutine(AnimateC4());
    }

    private IEnumerator AnimateC1()
    {
        // Set a lower and upper bound for random times within glitch.
        float lowerTime = 0.05f;
        float upperTime = 0.5f;

        // Change icon color.
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < 0.5f)
        {
            TopPanelC1.color = new Color(1f, 1f, 1f, 1f);
        } 
        else if (randomNumber < 0.9f)
        {
            TopPanelC1.color = new Color32(0x00, 0xff, 0x00, 0xff);
        }
        else
        {
            TopPanelC1.color = new Color32(0xff, 0x00, 0x00, 0xff);
        }

        // Wait for a random amount of time.
        yield return new WaitForSeconds(UnityEngine.Random.Range(lowerTime, upperTime));

        // Start next animation cycle.
        StartCoroutine(AnimateC1());
    }

    private IEnumerator AnimateC4()
    {
        // Set a lower and upper bound for random times within glitch.
        float lowerTime = 0.5f;
        float upperTime = 0.6f;

        // Change icon color.
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < 0.4f)
        {
            TopPanelC4.sprite = TopPanelC4Images[0];
        }
        else if (randomNumber < 0.7f)
        {
            TopPanelC4.sprite = TopPanelC4Images[1];
        } 
        else
        {
            TopPanelC4.sprite = TopPanelC4Images[2];
        }

        // Wait for a random amount of time.
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(lowerTime, upperTime));

        // Start next animation cycle.
        StartCoroutine(AnimateC4());
    }

    void Update()
    {
        string day = System.DateTime.Now.ToString("MMMM dd");
        int year = Int32.Parse(System.DateTime.Now.ToString("yyyy"));
        DateTime time = DateTime.Now;
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Minute);
        string second = LeadingZero(time.Second);
        textClock.text = $"[EARTHTIME] {day} {year + 135} {hour}:{minute}:{second}";
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
