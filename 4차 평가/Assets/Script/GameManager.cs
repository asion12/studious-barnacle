using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager combo = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (combo == null)
        {
            combo = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (combo != this)
                Destroy(gameObject);
        }
    }
    public float mycombo = 1;
 
}
