using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllDieSetup : MonoBehaviour
{
   
    void Start()
    {
       
        }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (GameManager.instance.everyMonsters.Count <= 0)
            {
                SceneManager.LoadScene("Main");

                GameManager.instance.Gold = 0;
                GameManager.instance.Item1 = 0;
                GameManager.instance.Item2 = 0;
                GameManager.instance.Item3 = 0;
                GameManager.instance.hp = 100;
                GameManager.instance.damge = 50;
                GameManager.instance.mycombo = 100;

            }
        }


    }
}
