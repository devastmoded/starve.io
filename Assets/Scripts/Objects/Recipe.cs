using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 2)]
public class Recipe : ScriptableObject
{
    public Item[] items;
    public int[] amounts;
    public int craftTime;
    public bool table, fire, water;
}
