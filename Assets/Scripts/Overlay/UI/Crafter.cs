using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    public Item itemToCraft;

    public void Craft()
    {
        if (CraftingManager.itemToCraft == null) CraftingManager.StartCraft(itemToCraft);
    }
}