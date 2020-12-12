using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvSelect : MonoBehaviour
{
    public PolygonCollider2D sword, spear, pickaxe, hammer;

    private static CircleCollider2D handCollider;
    private static PolygonCollider2D toolCollider;
    private static SpriteRenderer armourSprite, toolSprite;
    public static Item armourItem, toolItem, selectedItem;
    [HideInInspector] public int index;

    private void Start()
    {
        handCollider = GameObject.Find("Right Hand").GetComponent<CircleCollider2D>();
        toolCollider = GameObject.Find("Tool").GetComponent<PolygonCollider2D>();
        armourSprite = GameObject.Find("Armour").GetComponent<SpriteRenderer>();
        toolSprite = GameObject.Find("Tool").GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        selectedItem = InvManager.slots[index].GetItem();
        if (selectedItem == null) return;

        switch (selectedItem.type)
        {
            case Item.Type.Food:
                BarManager.ChangeValue(BarManager.Value.Food, selectedItem.level);
                InvManager.UpdateSlot(selectedItem, -1);
                break;
            case Item.Type.Drink:
                BarManager.ChangeValue(BarManager.Value.Water, selectedItem.level);
                InvManager.UpdateSlot(selectedItem, -1);
                break;
            case Item.Type.Hammer:
                UpdateTool();
                break;
            case Item.Type.Tool:
                UpdateTool();
                break;
            case Item.Type.Weapon:
                UpdateTool();
                break;
            case Item.Type.Armour:
                if (armourItem == null)
                {
                    armourItem = selectedItem;
                    armourSprite.GetComponent<SpriteRenderer>().sprite = armourItem.sprite;
                    BarManager.SetArmourLevel(armourItem.level);
                }
                else
                {
                    armourItem = null;
                    armourSprite.GetComponent<SpriteRenderer>().sprite = null;
                    BarManager.SetArmourLevel(0);
                }
                break;
            case Item.Type.Building:
                if (Builder.build != null) Builder.HideBuild();
                else Builder.PreviewBuild(selectedItem);
                break;
            case Item.Type.Wall:
                if (Builder.build != null) Builder.HideBuild();
                else Builder.PreviewBuild(selectedItem);
                break;
        }
    }

    public void UpdateTool()
    {
        if (toolItem == null)
        {
            toolItem = selectedItem;
            toolSprite.sprite = toolItem.sprite;
            if (toolItem.itemName.Contains("Sword")) EquipTool(sword);
            else if (toolItem.itemName.Contains("Spear")) EquipTool(spear);
            else if (toolItem.itemName.Contains("Pick")) EquipTool(pickaxe);
            else if (toolItem.itemName.Contains("Hammer")) EquipTool(hammer);
            if (selectedItem.type == Item.Type.Weapon) PlayerController.ChangeWeight(1);
            else PlayerController.ChangeWeight(0.5f);
            Collector.SetLevel(toolItem.level);
        }
        else
        {
            toolItem = null;
            toolSprite.sprite = null;
            if (selectedItem.type == Item.Type.Weapon) PlayerController.ChangeWeight(-1);
            else PlayerController.ChangeWeight(-0.5f);
            Collector.SetLevel(1);
        }

        toolCollider.GetComponent<Shadow>().UpdateShadow();
    }

    private void EquipTool(PolygonCollider2D tool)
    {
        toolCollider.points = tool.points;
        toolCollider.transform.localPosition = tool.transform.position;
        toolCollider.transform.localRotation = tool.transform.rotation;
    }
}