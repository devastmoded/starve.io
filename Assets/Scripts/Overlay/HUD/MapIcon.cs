using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    public Sprite customIcon;
    [Range(0, 30)] public float IconSizeMultiplier = 1;
    public bool flipSprite, stationary;

    private GameObject Icon;
    private int rotation;

    void Start()
    {
        float parentScale = transform.localScale.x;
        if (flipSprite) rotation = 180;

        Icon = new GameObject("Map Icon");
        Icon.layer = 10;
        Icon.transform.parent = transform;
        Icon.transform.localPosition = Vector3.zero;
        Icon.transform.localScale = IconSizeMultiplier * parentScale * Vector2.one;
        Icon.transform.rotation = Quaternion.Euler(0, 0, rotation);

        SpriteRenderer sr = Icon.AddComponent<SpriteRenderer>();
        if (customIcon == null) sr.sprite = GetComponent<SpriteRenderer>().sprite;
        else sr.sprite = customIcon;
        if (name.Equals("Player")) sr.sortingOrder = 20;
        else if (tag.Equals("Water")) sr.sortingOrder = 0;
        else sr.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        if (stationary) Destroy(this);
    }

    void Update()
    {
        Icon.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}