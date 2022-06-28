using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiUp : MonoBehaviour
{
    public Text text = null;
    public Text text2 = null;


    // Update is called once per frame
    void Update()
    {
        uiUp();
        
    }
    public void uiUp()
    {
        text.text = "현재가지고있는 제화: " + GameManager.instance.Gold;
       
    }
 

}
