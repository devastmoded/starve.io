using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainPanel, skinPanel;
    public Sprite head, hand;

    private static bool on;

    public void Skins()
    {
        mainPanel.SetActive(on);
        on = !on;
        skinPanel.SetActive(on);
    }

    public void ChangeSkin(int index)
    {
        head = GameObject.Find("Skin" + index).GetComponent<Image>().sprite;
        hand = GameObject.Find("Hand" + index).GetComponent<Image>().sprite;
    }

    public void StartGame()
    {
        PlayerController.head = head;
        PlayerController.hand = hand;
        SceneManager.LoadScene("GameScene");
    }
}