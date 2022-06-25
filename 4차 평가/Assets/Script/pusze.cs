using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
public class pusze : MonoBehaviour
{
    public Image tuto = null;
    bool Imagesetup = false;
    void Start()
    {
        tuto.gameObject.SetActive(false);
    
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
                Time.timeScale = 0f;

            }

        }
    }

}
