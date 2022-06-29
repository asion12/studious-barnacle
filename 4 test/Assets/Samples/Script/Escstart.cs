using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escstart : MonoBehaviour
{
    public GameObject gameObjects = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        esc();
    }
    public void esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObjects.SetActive(true);
            Time.timeScale = 0;
        }
    }
   public void Quits()
    {
        Application.Quit();

    }
    public void rerode()
    {
        Time.timeScale = 1;
        gameObjects.SetActive(false);
    }
}
