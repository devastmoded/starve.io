using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryManager : MonoBehaviour
{
    private ResourceHolder rh;
    private SpriteRenderer[] berries = new SpriteRenderer[5];
    private int prevAmount;

    void Start()
    {
        rh = GetComponent<ResourceHolder>();
        for (int i = 0;i < 5; i++) berries[i] = GetComponentsInChildren<SpriteRenderer>()[i + 1];
    }

    void Update()
    {
        int amount = rh.GetAmount();
        if (prevAmount != amount)
        {
            prevAmount = amount;
            for (int i = 0;i < 5; i++)
            {
                if (i < amount) berries[i].enabled = true;
                else berries[i].enabled = false;
            }
        }
    }
}