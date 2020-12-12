using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public GameObject craftPrefab, progressBarParent;
    public Color inCraftColor = Color.gray;
    [Space]
    public int craftablesPerRow = 5;
    public Item[] craftables;

    private static List<GameObject> CraftObjects;
    private static GameObject progressBarFull;
    private static RectTransform progressBarFront;
    public static Item itemToCraft;
    private static Color normalColor, craftColor;
    private static int craftTime, craftTimeLeft;
    public static bool table, fire, water;

    private void Start()
    {
        CraftObjects = new List<GameObject>();
        progressBarFull = progressBarParent;
        progressBarFront = progressBarParent.GetComponentsInChildren<RectTransform>()[2];
        normalColor = craftPrefab.GetComponent<Image>().color;
        craftColor = inCraftColor;
    }

    private void FixedUpdate()
    {
        if (craftTimeLeft == 1) EndCraft();
        if (craftTimeLeft > 0)
        {
            craftTimeLeft--;
            float xScale = 1.0f - ((float) craftTimeLeft / craftTime);
            progressBarFront.localScale = new Vector2(xScale, 1);
        }
    }

    public void UpdateRecipes()
    {
        foreach (GameObject g in CraftObjects) Destroy(g);
        CraftObjects.Clear();

        foreach (Item item in craftables)
        {
            if (item.recipe.table) if (!table) continue;
            if (item.recipe.fire) if (!fire) continue;
            if (item.recipe.water) if (!water) continue;

            if (InvManager.Contains(item.recipe))
            {
                GameObject newCraft = Instantiate(craftPrefab, transform);
                int x = CraftObjects.Count, y = 0;
                while (x >= craftablesPerRow)
                {
                    x -= craftablesPerRow;
                    y++;
                }
                newCraft.GetComponent<RectTransform>().localPosition = new Vector2(110 * x, -110 * y);
                newCraft.GetComponentsInChildren<Image>()[1].sprite = item.sprite;
                newCraft.GetComponent<Crafter>().itemToCraft = item;

                CraftObjects.Add(newCraft);
            }
        }
    }

    public static void StartCraft(Item toCraft)
    {
        itemToCraft = toCraft;
        craftTime = 50 * toCraft.recipe.craftTime + 1;
        craftTimeLeft = craftTime;
        foreach (GameObject crafter in CraftObjects) crafter.GetComponent<Image>().color = craftColor;
        progressBarFull.SetActive(true);
    }

    private static void EndCraft()
    {
        Recipe r = itemToCraft.recipe;
        foreach (Item item in r.items) InvManager.UpdateSlot(item, -r.amounts[System.Array.IndexOf(r.items, item)]);
        if (InvSelect.toolItem != null && !InvManager.Contains(1, InvSelect.toolItem)) new InvSelect().UpdateTool();

        foreach (GameObject crafter in CraftObjects) crafter.GetComponent<Image>().color = normalColor;
        if (itemToCraft.itemName.Equals("Bag")) GameObject.Find("Inventory").GetComponent<InvManager>().EquipBag();
        else InvManager.UpdateSlot(itemToCraft, 1);
        progressBarFull.SetActive(false);
        itemToCraft = null;
    }
}