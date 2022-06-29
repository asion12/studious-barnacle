using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullData : MonoBehaviour
{
    //trigger를 통해서 찾고자 하는 목표 tag 설정
    public string targetTag = string.Empty;
    //Trigger 안에 들어왔냐 
    private void OnTriggerEnter(Collider other)
    {
        //tag가 찾던 tag가 맞냐?
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            gameObject.SendMessageUpwards("OnCKTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);

        }
        //두 오브젝트 거리알기

    }
}
