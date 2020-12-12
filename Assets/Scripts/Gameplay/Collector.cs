using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private static Collider2D hand, tool;
    private static int level = 1;

    private void Start()
    {
        hand = GameObject.Find("Right Hand").GetComponent<CircleCollider2D>();
        tool = GameObject.Find("Tool").GetComponentInChildren<PolygonCollider2D>();
    }

    public static void SetLevel(int lvl)
    {
        level = lvl;
    }

    public void CanMine()
    {
        if (InvSelect.toolItem == null) hand.enabled = true;
        else tool.enabled = true;
    }

    public void CantMine()
    {
        if (InvSelect.toolItem == null) hand.enabled = false;
        else tool.enabled = false;
    }

    private void Mine(Collider2D other)
    {
        Item resource = other.GetComponent<ResourceHolder>().resource;
        int amount = other.GetComponent<ResourceHolder>().GetResource(level);

        if (amount > 0) InvManager.UpdateSlot(resource, amount);
        else if (amount == 0) Messages.DisplayMsg("Resource is empty", 3);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Resource"))
        {
            if (InvSelect.toolItem == null || InvSelect.toolItem.type == Item.Type.Tool) Mine(other);
            else Messages.DisplayMsg("Wrong tool", 3);
        }
        else if (other.tag.Equals("Mob")) other.GetComponent<MobBehavior>().TakeDamage(level);
        else if (other.tag.Equals("Building") && InvSelect.toolItem.type == Item.Type.Hammer) Destroy(other.gameObject);
    }
}