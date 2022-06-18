using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopingItem : MonoBehaviour
{
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Buy1();
    }
    public void Buy1()
    {
      
            if (GameManager.instance.Gold > 10000)
            {
                GameManager.instance.Gold -= 10000;
                GameManager.instance.damge += 50;
                GameManager.instance.sword += 1;
                
            }
        
    
    }
    public void Buy2()
    {
        if (GameManager.instance.Gold > 50000)
        {
            GameManager.instance.damge += 300;
            GameManager.instance.sword += 10;
            GameManager.instance.maxcombo += 100;
        }
    }
    public void Buy3()
    {
        if (GameManager.instance.Gold > 100000)
        {
            GameManager.instance.damge += 10000;
            GameManager.instance.sword += 20;
            GameManager.instance.maxcombo += 500;
        }
    }

}
