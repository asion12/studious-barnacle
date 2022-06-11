using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Holoville.HOTween;
public class Nerp : MonoBehaviour
{
    //�������ݾ�
    RaycastHit Hit;
    public float distanstHit = 3f;
    public Transform target;    
    public float angleRange = 30f;
    public float radius = 3f;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    bool isCollision = false;

    void Update()
    {
       
        if (Physics.Raycast(transform.position,transform.forward,out Hit,distanstHit))
        {
            //���� ���� �Ÿ�üũ
            Vector3 interV = Hit.transform.position - transform.position;
            
            //�Ÿ��� ���������� ������ 
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
                            Debug.Log("a");
                            HOTween.Init(true, true, true);

                            HOTween.To(this, 2f, new TweenParms()
                                .Prop("aniStr", "Hello World !! See You Nice Day")
                                .Loops(-1, LoopType.Yoyo));

                            HOTween.To(transform, 1, "position", new Vector3(transform.position.x, 0, transform.position.z + 10));

                            HOTween.To(this, 2f, new TweenParms()
                                .Prop("aniFloat", 10f)
                                .Loops(-1, LoopType.Restart));
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
     


    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }
}
