using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pusze : MonoBehaviour
{
    public Image tuto = null;
    public Image tuto2 = null;
    public Image tuto3 = null;
    public Image tuto4 = null;
    bool Imagesetup = false;
    void Start()
    {
        tuto.gameObject.SetActive(false);
        tuto2.gameObject.SetActive(false);
        tuto3.gameObject.SetActive(false);
        tuto4.gameObject.SetActive(false);
    

    }

    // Update is called once per frame
    void Update()
    {
        Imagedsetup();
      
    }
    void Imagedsetup()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Imagesetup = true;
            if (Imagesetup == true)
            {
                Debug.Log("¿ÖÁö");
                tuto.gameObject.SetActive(true);
                tuto2.gameObject.SetActive(true);
                tuto3.gameObject.SetActive(true);
                tuto4.gameObject.SetActive(true);
            
              
                Time.timeScale = 0f;

            }

        }
    }

}
