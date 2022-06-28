using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class youdie : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        adfdsa2();  
        adfdsa();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void adfdsa()
    {
        SceneManager.LoadScene("tuto");
    }
    public void adfdsa2()
    {
        SceneManager.LoadScene("main");
    }
}
