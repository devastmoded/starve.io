using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed = 5f;
    [Space]
    [Range(0.1f, 1)] public float waterSpeedMultiplier = 0.5f;

    private Rigidbody2D rb;
    private static Animator handsAni, mapAni;
    private static CraftingManager cm;
    public static Sprite head, hand;
    private Vector2 move, mousePos;
    private static float weight;
    public static bool mapUp;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(11, 12);
        Physics2D.IgnoreLayerCollision(11, 11);
        handsAni = GameObject.Find("Hands").GetComponent<Animator>();
        mapAni = GameObject.Find("Map").GetComponent<Animator>();
        cm = GameObject.FindObjectOfType<CraftingManager>();

        Bounds map = new Bounds(Vector3.zero, new Vector3(45, 45, 0));
        transform.position = new Vector3(Random.Range(map.min.x, map.max.x), Random.Range(map.min.y, map.max.y));

        if (head != null)
        {
            GetComponent<SpriteRenderer>().sprite = head;
            GameObject.Find("Left Hand").GetComponent<SpriteRenderer>().sprite = hand;
            GameObject.Find("Right Hand").GetComponent<SpriteRenderer>().sprite = hand;
        }
    }

    private void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) handsAni.SetBool("clicking", true);
        else if (Input.GetMouseButtonUp(0)) handsAni.SetBool("clicking", false);

        if (Input.GetKeyDown(KeyCode.M)) OpenCloseMap();
        if (Input.GetKeyDown(KeyCode.R)) BarManager.ChangeAutoFeed();
    }

    private void FixedUpdate()
    {
        float moveMagnitude = moveSpeed - weight;
        if (CraftingManager.water) moveMagnitude *= waterSpeedMultiplier;
        rb.MovePosition(rb.position + moveMagnitude * move.normalized * Time.fixedDeltaTime);
        transform.up = mousePos - rb.position;
    }

    public static void ChangeWeight(float amount)
    {
        weight = amount;
    }

    void OpenCloseMap()
    {
        if (mapUp)
        {
            mapAni.Play("CloseMap");
        }
        else
        {
            mapAni.Play("OpenMap");
        }
        mapUp = !mapUp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Table")) CraftingManager.table = true;
        else if (other.tag.Equals("Fire")) CraftingManager.fire = true;
        else if (other.tag.Equals("Water")) CraftingManager.water = true;
        if (other.tag.Equals("Table") || other.tag.Equals("Fire") || other.tag.Equals("Water")) cm.UpdateRecipes();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Table")) CraftingManager.table = false;
        else if (other.tag.Equals("Fire")) CraftingManager.fire = false;
        else if (other.tag.Equals("Water")) CraftingManager.water = false;
        if (other.tag.Equals("Table") || other.tag.Equals("Fire") || other.tag.Equals("Water")) cm.UpdateRecipes();
    }
}