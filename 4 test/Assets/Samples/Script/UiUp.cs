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
        text.text = "���簡�����ִ� ��ȭ: " + GameManager.instance.Gold;
       
    }
 

}
