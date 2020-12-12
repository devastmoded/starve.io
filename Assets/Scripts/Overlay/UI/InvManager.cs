using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public SpriteRenderer bagObject;
    [Space]
    public Item[] startingItems;

    private static CraftingManager cm;
    private static List<GameObject> slotObjects;
    public static List<Slot> slots;

    void Start()
    {
        cm = GameObject.FindObjectOfType<CraftingManager>();
        slotObjects = new List<GameObject>();
        slots = new List<Slot>();

        AddSlot(10);
        foreach (Item i in startingItems) UpdateSlot(i, 1);
    }

    public void AddSlot(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform);
            slotObjects.Add(newSlot);
            slots.Add(new Slot());
        }

        for (int i = 0; i < slotObjects.Count; i++)
        {
            slotObjects[i].GetComponent<RectTransform>().localPosition = new Vector2(52.5f * i - 52.5f * (slotObjects.Count - 1) / 2.0f, 0);
            slotObjects[i].GetComponent<InvSelect>().index = i;
        }
    }

    /// <summary>
    /// Finds a slot with item to change it by amount or add amount of item to an empty slot
    /// </summary>
    public static void UpdateSlot(Item item, int amount)
    {
        foreach (Slot slot in slots)
        {
            if (slot.GetItem() == item)
            {
                slot.ChangeAmount(amount);
                UpdateItem(slots.IndexOf(slot), item);
                amount = 0;
                break;
            }
        }

        if (amount > 0)
        {
            foreach (Slot slot in slots)
            {
                if (slot.GetItem() == null)
                {
                    slot.SetItem(item);
                    slot.ChangeAmount(amount);
                    UpdateItem(slots.IndexOf(slot), item);
                    amount = 0;
                    break;
                }
            }
        }

        if (amount > 0) Messages.DisplayMsg("Inventory full", 2);
        else cm.UpdateRecipes();
    }

    private static void UpdateItem(int index, Item item)
    {
        Image i = slotObjects[index].GetComponentsInChildren<Image>()[1];
        Text t = slotObjects[index].GetComponentInChildren<Text>();
        int amount = slots[index].GetAmount();

        if (amount < 2) t.text = "";
        else t.text = "x" + amount.ToString();

        if (slots[index].GetAmount() > 0)
        {
            i.sprite = item.sprite;
            i.color = Color.white;
        }
        else
        {
            i.sprite = null;
            i.color = Color.clear;
        }
    }

    public void EquipBag()
    {
        bagObject.GetComponent<SpriteRenderer>().enabled = true;
        AddSlot(5);
    }

    /// <summary>
    /// Returns true if the amount of Item item in the inventory is >= to int amount
    /// </summary>
    public static bool Contains(int amount, Item item)
    {
        foreach (Slot slot in slots) if (slot.GetItem() == item && slot.GetAmount() >= amount) return true;
        return false;
    }

    /// <summary>
    /// Returns true if there is enough of each item from the recipe in the inventory.
    /// </summary>
    public static bool Contains(Recipe r)
    {
        int foundItems = 0;
        foreach (Item item in r.items)
        {
            if (foundItems != System.Array.IndexOf(r.items, item)) return false;
            foreach (Slot slot in slots)
            {
                if (slot.GetItem() == item && slot.GetAmount() >= r.amounts[System.Array.IndexOf(r.items, item)])
                {
                    foundItems++;
                    break;
                }
            }
        }

        if (foundItems == r.items.Length) return true;
        else return false;
    }

    public static bool TryConsume(Item.Type t)
    {
        foreach (Slot s in slots)
        {
            Item i = s.GetItem();
            if (i != null) if (i.type == t)
                {
                    UpdateSlot(i, -1);
                    BarManager.ChangeValue(BarManager.Value.Food, i.level);
                    return true;
                }
        }

        return false;
    }
}