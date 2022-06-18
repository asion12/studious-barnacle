using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text text = null;    
    public static GameManager instance = null;
    public float mycombo = 1;
    public int hp = 100;
    public int Gold = 1000000;
    public int damge = 50;
    public int sword = 0;
    public int maxcombo = 900;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
   public void uiUp()
    {
        text.text = "현재가지고있는 제화: " + GameManager.instance.Gold;
    }
}
