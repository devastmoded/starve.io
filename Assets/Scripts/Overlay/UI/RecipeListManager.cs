using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeListManager : MonoBehaviour
{
    public GameObject recipePrefab;
    public GameObject recipeListMenu, sectionsPanel, recipesPanel;
    public GameObject tableText, fireText, waterText;
    public Text closeButtonText;

    private static List<GameObject> recipeObjects;
    private static GameObject msgs;
    private static Item[] craftables;
    private static bool on;

    private void Start()
    {
        recipeObjects = new List<GameObject>();
        msgs = GameObject.Find("Messages");
        craftables = GameObject.FindObjectOfType<CraftingManager>().craftables;
    }

    public void OnOff()
    {
        on = !on;
        recipeListMenu.SetActive(on);
        sectionsPanel.SetActive(on);
        msgs.SetActive(!on);
        if (on)
        {
            closeButtonText.text = "Close";
        }
        else
        {
            foreach (GameObject g in recipeObjects) Destroy(g);
            recipeObjects.Clear();
            RecipeSelect.ClearRecipe();
            recipesPanel.SetActive(false);
            SetBools(false, false, false);
        }
    }

    public void Back()
    {
        closeButtonText.text = "Close";
        sectionsPanel.SetActive(true);
        recipesPanel.SetActive(false);

        RecipeSelect.ClearRecipe();
        foreach (GameObject g in recipeObjects) Destroy(g);
        recipeObjects.Clear();
    }

    public void CloseButton()
    {
        switch (closeButtonText.text)
        {
            case "Close": OnOff(); return;
            case "Back": Back(); return;
        }
    }

    public void PlaceRecipes(int type)
    {
        closeButtonText.text = "Back";

        sectionsPanel.SetActive(false);
        recipesPanel.SetActive(true);
        foreach (Item i in craftables) if ((int)i.type == type) addRecipeButton(i);
        if (type == 1) foreach (Item i in craftables) if ((int)i.type == 2) addRecipeButton(i);
        if (type == 3) foreach (Item i in craftables) if ((int)i.type == 4) addRecipeButton(i);
    }

    private void addRecipeButton(Item item)
    {
        GameObject newRecipe = Instantiate(recipePrefab, recipesPanel.GetComponentsInChildren<Transform>()[1]);
        int x = recipeObjects.Count, y = 0;
        while (x >= 8)
        {
            x -= 8;
            y++;
        }
        RectTransform rt = newRecipe.GetComponent<RectTransform>();
        rt.localPosition = new Vector2(rt.localPosition.x + 75 * x, rt.localPosition.y - 75 * y);
        Image i = newRecipe.GetComponentsInChildren<Image>()[1];
        i.sprite = item.sprite;
        i.color = Color.white;

        recipeObjects.Add(newRecipe);
        newRecipe.GetComponent<RecipeSelect>().recipe = item.recipe;
    }

    public void SetBools(bool table, bool fire, bool water)
    {
        tableText.SetActive(table);
        fireText.SetActive(fire);
        waterText.SetActive(water);
    }
}