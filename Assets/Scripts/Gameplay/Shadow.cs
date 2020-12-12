using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public float offSet = 0.25f;
    public bool stationary = true;

    private GameObject shadow;
    private SpriteRenderer sr;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;
        shadow.transform.localScale = Vector3.one;
        shadow.transform.rotation = transform.rotation;
        shadow.transform.position = (Vector2)transform.position + new Vector2(0, -offSet);
        sr = shadow.AddComponent<SpriteRenderer>();
        sr.sprite = GetComponent<SpriteRenderer>().sprite;
        sr.color = new Color(0, 0, 0, 0.4f);
        sr.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

        if (stationary) Destroy(this);
    }

    void Update()
    {
        shadow.transform.position = (Vector2) transform.position + new Vector2(0, -offSet);
    }

    public void UpdateShadow()
    {
        sr.sprite = GetComponent<SpriteRenderer>().sprite;
    }
}