using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSelect : MonoBehaviour
{
    public Recipe recipe;

    private static List<GameObject> itemObjects = new List<GameObject>();
    private static RecipeListManager rlm;

    private void Start()
    {
        for (int i = 0; i < 5; i++) itemObjects.Add(GameObject.Find("Item (" + i + ")"));
        rlm = GameObject.FindObjectOfType<RecipeListManager>();
    }

    public void ShowRecipe()
    {
        ClearRecipe();

        for (int i = 0; i < recipe.items.Length; i++)
        {
            Image sr = itemObjects[i].GetComponentsInChildren<Image>()[1];
            sr.sprite = recipe.items[i].sprite;
            sr.color = Color.white;
            itemObjects[i].GetComponentInChildren<Text>().text = "x" + recipe.amounts[i];
            rlm.SetBools(recipe.table, recipe.fire, recipe.water);
        }
    }

    public static void ClearRecipe()
    {
        for (int i = 0; i < 5; i++)
        {
            try
            {
                Image sr = itemObjects[i].GetComponentsInChildren<Image>()[1];
                sr.sprite = null;
                sr.color = Color.clear;
                itemObjects[i].GetComponentInChildren<Text>().text = null;
                rlm.SetBools(false, false, false);
            }
            catch
            {
                break;
            }
        }
    }
}