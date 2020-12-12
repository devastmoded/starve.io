using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : MonoBehaviour
{
    public Item resource;
    public int refillInterval, maxAmount, level;

    private float lastCheckTime = 0;
    private int amount;

    void Start()
    {
        lastCheckTime = Time.time;
        amount = maxAmount;
    }

    void FixedUpdate()
    {
        if (Time.time == lastCheckTime + refillInterval)
        {
            lastCheckTime = Time.time;
            if (amount < maxAmount) amount++;
        }
    }

    public int GetAmount()
    {
        return amount;
    }

    public int GetResource(int toolLevel)
    {
        if (level == -1)
        {
            if (amount > 0)
            {
                amount--;
                return 1;
            }
            else return 0;
        }
        else if (toolLevel > resource.level)
        {
            int take = 0;
            for (int i = 0; i < toolLevel - resource.level; i++)
            {
                if (amount > 0)
                {
                    amount--;
                    take++;
                }
            }
            return take;
        }
        else
        {
            Messages.DisplayMsg("Tool too weak", 3);
            return -1;
        }
    }
}