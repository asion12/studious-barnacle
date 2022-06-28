using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blink : MonoBehaviour
{

   public Color color;
    void Start()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = color;
    }

    void Update()
    {

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        color.a = (Mathf.Sin(Time.time) + 1) * 0.5f;
        color.r = (Mathf.Sin(Time.time)+1) * 0.3f;
        color.b = (Mathf.Cos(Time.time)+1) * 0.4f;
        color.g = (Mathf.Sin(Time.time)) * 0.5f;
        spr.color = color; 
    }
    private void FixedUpdate()
    {
        color.r = (Mathf.Sin(Random.Range(0f, 1f)*Time.time) + 1) * 0.3f;
        color.b = (Mathf.Cos(Time.time) + 1) * 0.4f;
        color.g = (Mathf.Sin(Time.time)) * 0.5f;
    }
}
