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
            Debug.LogError("매니저가 한개 이상이다");
        }
        instance = this;
    }

    public void SetText(string s)
    {
        //StringBuilder sb = new StringBuilder();
        //sb.Append()
        monsterCountText.text = $"남은 몬스터 수: {s}";

    }
    public void SetTexst(string s)
    {
        mycombo.text= $"콤보 : {s}";
    }
    public void SetTexsts(string s)
    {
        dam.text = $"데미지 : {s}";
    }
}
