using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    private Item item;
    private int amount;

    public Slot()
    {
        item = null;
        amount = 0;
    }

    public Item GetItem()
    {
        return item;
    }
    public int GetAmount()
    {
        return amount;
    }

    public void SetItem(Item i)
    {
        item = i;
    }

    public void ChangeAmount(int a)
    {
        amount += a;
        if (amount == 0) item = null;
    }
}
