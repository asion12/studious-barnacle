using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject eff = null;
    
    public static GameManager instance = null;
    public float mycombo = 1;
    public int hp = 100;
    public int Gold = 1000000;
    public int damge = 50;
    public int sword = 0;
    public int maxcombo = 900;
    public int skullhp = 100;

    public int Item1 = 0;
    public int Item2 = 0;
    public int Item3 = 0;
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
  public void a()
    {
        if (Item1>0)
        {
            
                Debug.Log("tlqkf?");
               eff.gameObject.SetActive(true);
            

        }
    }
   
}
