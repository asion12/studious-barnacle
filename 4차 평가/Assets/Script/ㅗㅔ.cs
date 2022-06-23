using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ㅗㅔ : MonoBehaviour
{
    public Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        uiUp();
    }
    public void uiUp()
    {
        text.text = "너 피: " + GameManager.instance.hp;

    }

}
