using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public enum Type { Resource, Food, Drink, Hammer, Tool, Weapon, Armour, Building, Wall }

    public GameObject building;
    public Recipe recipe;
    public Sprite sprite;
    public string itemName;
    public Type type;
    public int level;
}