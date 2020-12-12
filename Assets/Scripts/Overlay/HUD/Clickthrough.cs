using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickthrough : MonoBehaviour, IPointerDownHandler
{
    private Animator handsAni;

    void Start()
    {
        handsAni = GameObject.Find("Hands").GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handsAni.SetBool("clicking", true);
    }
}