using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Sprite day, night;
    public Material tint;
    [Range(0, 0.9f)] public float tintMin;
    [Space]
    public int dayCycleSpeed = 1;

    private static Image clockBack;
    private static RectTransform clock;

    private void Start()
    {
        clockBack = GameObject.Find("Clock").GetComponent<Image>();
        clock = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        float colorMultiplier = 0.75f * dayCycleSpeed * Time.time + 90;
        while (colorMultiplier >= 360) colorMultiplier -= 360;
        if (colorMultiplier >= 180) colorMultiplier = 1 - ((colorMultiplier - 180) / 180.0f);
        else colorMultiplier /= 180.0f;
        colorMultiplier *= 2.0f;
        colorMultiplier = Mathf.Clamp(colorMultiplier, tintMin, 1);
        tint.color = new Color(colorMultiplier, colorMultiplier, colorMultiplier, 1);

        float rotation = 0.75f * dayCycleSpeed * Time.time;
        while (rotation >= 360) rotation -= 360;
        clock.rotation = Quaternion.Euler(0, 0, -rotation);

        if (180f - rotation < 1f)
        {
            clockBack.sprite = night;
            BarManager.SetNight(true);
        }
        else if (rotation < 1f)
        {
            clockBack.sprite = day;
            BarManager.SetNight(false);
        }
    }

    private void OnApplicationQuit()
    {
        tint.color = Color.white;
    }
}