using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Item build;
    private static GameObject builder;
    private static BoxCollider2D hitbox;
    private static SpriteRenderer preview;
    private int touchCount;

    private void Start()
    {
        builder = gameObject;
        preview = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && build != null)
        {
            if (touchCount == 0) Build();
        }
    }

    public static void PreviewBuild(Item building)
    {
        preview.sprite = building.building.GetComponent<SpriteRenderer>().sprite;
        hitbox = (BoxCollider2D) builder.AddComponent(typeof(BoxCollider2D));
        hitbox.isTrigger = true;
        build = building;
    }

    public static void HideBuild()
    {
        preview.sprite = null;
        Destroy(hitbox);
        build = null;
    }

    private void Build()
    {
        Instantiate(build.building, builder.transform.position, builder.transform.rotation, GameObject.Find("Buildings").transform);
        InvManager.UpdateSlot(build, -1);
        preview.sprite = null;
        Destroy(hitbox);
        build = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        touchCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchCount--;
    }
}