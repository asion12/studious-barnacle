using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopingItem : MonoBehaviour
{
    private
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Buy1()
    {
        if (GameManager.instance.Gold > 10000)
        {
            GameManager.instance.damge += 50;
        }
    }
}
