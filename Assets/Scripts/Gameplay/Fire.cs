using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int lifeTime = 120;

    private int startTime;

    void Start()
    {
        startTime = (int) Time.time;
    }

    void Update()
    {
        if ((int)Time.time == startTime + lifeTime) Destroy(gameObject);
    }
}