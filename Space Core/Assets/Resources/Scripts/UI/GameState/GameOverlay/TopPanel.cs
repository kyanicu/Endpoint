using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TopPanel : MonoBehaviour
{
    public Text textClock;

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
