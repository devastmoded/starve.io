using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{
    private static Text text;
    private static int dur, lastCheckTime;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (Time.time == lastCheckTime + dur)
        {
            lastCheckTime = 0;
            text.text = "";
            dur = 0;
        }
    }

    public static void DisplayMsg(string message, int duration)
    {
        lastCheckTime = Mathf.RoundToInt(Time.time);
        text.text = message;
        dur = duration;
    }
}