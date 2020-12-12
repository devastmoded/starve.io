using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BarManager : MonoBehaviour
{
    public int statInterval = 5;
    public enum Value { Health, Food, Temp, Water }

    private static Animator bodyAni;
    private RectTransform health, food, temp, water;
    private static Text autoFeedText;
    private float lastCheckTime, fullBar;
    private static float healthVal = 200, foodVal = 200, tempVal = 200, waterVal = 200;
    private static int armourLvl;
    private static bool night, autoFeed;

    void Start()
    {
        bodyAni = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        health  = GameObject.Find("Health Bar").GetComponent<RectTransform>();
        food    = GameObject.Find("Food Bar").GetComponent<RectTransform>();
        temp    = GameObject.Find("Temp Bar").GetComponent<RectTransform>();
        water   = GameObject.Find("Water Bar").GetComponent<RectTransform>();

        autoFeedText = GameObject.Find("Auto Feed").GetComponent<Text>();
        lastCheckTime = Time.time;
        fullBar = (int) health.rect.width;
    }

    void FixedUpdate()
    {
        if (Time.time == lastCheckTime + statInterval)
        {
            lastCheckTime = Time.time;

            if (foodVal > 70 && tempVal > 70 && waterVal > 70) ChangeValue(Value.Health, 6);
            else
            {
                if (foodVal == 0) ChangeValue(Value.Health, -40);
                if (tempVal == 0) ChangeValue(Value.Health, -20);
                if (waterVal == 0) ChangeValue(Value.Health, -30);
            }

            ChangeValue(Value.Food, -3);
            if (CraftingManager.fire) ChangeValue(Value.Temp, 25);
            else if (night) ChangeValue(Value.Temp, -18);
            else ChangeValue(Value.Temp, -2);
            if (CraftingManager.water) ChangeValue(Value.Water, 40f);
            else ChangeValue(Value.Water, -2f);

            if (healthVal < 50) health.GetComponent<Animator>().Play("Pulse");
            else health.GetComponent<Animator>().Play("Nothing");
            if (foodVal < 50) food.GetComponent<Animator>().Play("Pulse");
            else food.GetComponent<Animator>().Play("Nothing");
            if (tempVal < 50) temp.GetComponent<Animator>().Play("Pulse");
            else temp.GetComponent<Animator>().Play("Nothing");
            if (waterVal < 50) water.GetComponent<Animator>().Play("Pulse");
            else water.GetComponent<Animator>().Play("Nothing");

            if (foodVal < 45 && autoFeed) if (InvManager.TryConsume(Item.Type.Food)) food.GetComponent<Animator>().Play("Nothing");
            if (waterVal < 45 && autoFeed) if (InvManager.TryConsume(Item.Type.Drink)) water.GetComponent<Animator>().Play("Nothing");

            print(healthVal);
            if (healthVal == 0) SceneManager.LoadScene("MenuScene");
        }
        float divider = 200.0f;

        health.sizeDelta = new Vector2(health.rect.width + Mathf.Clamp(healthVal / divider * fullBar - health.rect.width, -1, 1), health.rect.height);
        food.sizeDelta   = new Vector2(food.rect.width + Mathf.Clamp(foodVal / divider * fullBar - food.rect.width, -1, 1), food.rect.height);
        temp.sizeDelta   = new Vector2(temp.rect.width + Mathf.Clamp(tempVal / divider * fullBar - temp.rect.width, -1, 1), temp.rect.height);
        water.sizeDelta  = new Vector2(water.rect.width + Mathf.Clamp(waterVal / divider * fullBar - water.rect.width, -1, 1), water.rect.height);
    }

    public static void HitPlayer(float amount)
    {
        float damage = amount * (100 - armourLvl) / 100.0f;

        ChangeValue(Value.Health, -1);
    }

    public static void ChangeValue(Value a, float amount)
    {
        switch (a)
        {
            case Value.Health:
                healthVal += amount;
                healthVal = Mathf.Clamp(healthVal, 0, 200);
                if (amount < 0) bodyAni.Play("Damage");
                break;
            case Value.Food:
                foodVal += amount;
                foodVal = Mathf.Clamp(foodVal, 0, 200);
                break;
            case Value.Temp:
                tempVal += amount;
                tempVal = Mathf.Clamp(tempVal, 0, 200);
                break;
            case Value.Water:
                waterVal += amount;
                waterVal = Mathf.Clamp(waterVal, 0, 200);
                break;
        }
    }

    public static void SetArmourLevel(int lvl)
    {
        armourLvl = lvl;
    }

    public static void SetNight(bool night)
    {
        BarManager.night = night;
    }

    public static void ChangeAutoFeed()
    {
        autoFeed = !autoFeed;
        autoFeedText.enabled = autoFeed;
    }
}