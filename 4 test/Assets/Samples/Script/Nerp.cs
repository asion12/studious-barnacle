  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Holoville.HOTween;
using DG.Tweening;
using UnityEngine.UI;
public class Nerp : MonoBehaviour
{
    public Text text = null;
    //지정이잖아
    RaycastHit Hit;
    public float distanstHit = 3f;
    public Transform target;    
    public float angleRange = 30f;
    public float radius = 3f;
    public float dashRange = 15f; //대시 거리
    public float dashTime = 1f; //대시 시간
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
    bool coms = false;
    bool isCollision = false;
    GameObject VgameObject = null;
    PlayerControl vari = null;
    private void Start()
    {
        VgameObject = GameObject.Find("Variables 컴포넌트가 부착된 오브젝트");
        vari = gameObject.GetComponent<PlayerControl>();
    }
    void Update()
    {
        yesgood();


        if (Physics.Raycast(transform.position,transform.forward,out Hit,distanstHit))
        {
            //나와 적의 거리체크
            Vector3 interV = Hit.transform.position - transform.position;
            
            //거리가 반지름보다 작으면 
            if (interV.magnitude <= radius)
            {

                float dot = Vector3.Dot(interV.normalized, transform.forward);

                float theta = Mathf.Acos(dot);

                float degree = Mathf.Rad2Deg * theta;


                if (degree <= angleRange / 2f)
                {
                   
                       isCollision = true;
                       if (isCollision == true) 
                      {
                        
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            coms = true;
                            if (coms)
                            {
                                if (GameManager.instance.mycombo > 100)
                                {
                                    GameManager.instance.mycombo-=100;
                                    if (Physics.Raycast(transform.position, transform.forward, out Hit, distanstHit))
                                    {
                                        GameManager.instance.hp += 4;
                                        transform.DOMove(transform.position + transform.forward * dashRange, dashTime);
                                    }

                                        coms = false;
                                }

                            }
                        }
                        
                        
                      }
                }

                else
                    isCollision = false;
                
            }
            else
                isCollision = false;
        }
    }
     
  
    void yesgood()
    {
        if (isCollision == true)
        {
            text.text = "스킬사용이 가능합니다";
        }
        else
        {
            text.text = "스킬사용이 불가능합니다";
        }
       
    }
    //private void OnDrawGizmos()
    //{
    //    Handles.color = isCollision ? _red : _blue;
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    //}
}
