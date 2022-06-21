using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopingItem : MonoBehaviour
{
    public Image ons = null;
    public GameObject eff = null;
    bool ByItem = false;

    // Start is called before the first frame update
    void Start()
    {
        Buy1();
      

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Buy1()
    {
        ByItem = true;
        if (ByItem==true)
        {
            if (GameManager.instance.Gold >= 10000)
            {
                GameManager.instance.Gold -= 10000;
                GameManager.instance.damge += 50;
                GameManager.instance.sword += 1;
                GameManager.instance.skullhp += 100;
                GameManager.instance.Item1 += 1;
                GameManager.instance.a();
                ByItem = false;

            }else
            {
                ons.gameObject.SetActive(true);
               
                
            }
        }
        
        
    
    }
    
    public void Buy2()
    {
        if (GameManager.instance.Gold > 50000)
        {
            GameManager.instance.damge += 300;
            GameManager.instance.sword += 10;
            GameManager.instance.maxcombo += 100;
            GameManager.instance.skullhp += 500;
        }
    }
    public void Buy3()
    {//¹Ì»ç¿ë
        if (GameManager.instance.Gold > 100000)
        {
            GameManager.instance.damge += 10000;
            GameManager.instance.sword += 20;
            GameManager.instance.maxcombo += 500;
            GameManager.instance.skullhp += 10000;
        }
    }

}
