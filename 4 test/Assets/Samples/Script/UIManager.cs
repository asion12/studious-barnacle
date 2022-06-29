using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { 
        get 
        { 
            return instance; 
        } 
    }

    [SerializeField]
    private Text monsterCountText;
    [SerializeField]
    private Text mycombo;
    [SerializeField]
    private Text dam;
    void Awake()
    {

        if(instance != null)
        {
            Debug.LogError("�Ŵ����� �Ѱ� �̻��̴�");
        }
        instance = this;
    }

    public void SetText(string s)
    {
        //StringBuilder sb = new StringBuilder();
        //sb.Append()
        monsterCountText.text = $"���� ���� ��: {s}";

    }
    public void SetTexst(string s)
    {
        mycombo.text= $"�޺� : {s}";
    }
    public void SetTexsts(string s)
    {
        dam.text = $"������ : {s}";
    }
}
