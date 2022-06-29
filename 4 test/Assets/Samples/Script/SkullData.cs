using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullData : MonoBehaviour
{
    //trigger�� ���ؼ� ã���� �ϴ� ��ǥ tag ����
    public string targetTag = string.Empty;
    //Trigger �ȿ� ���Գ� 
    private void OnTriggerEnter(Collider other)
    {
        //tag�� ã�� tag�� �³�?
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            gameObject.SendMessageUpwards("OnCKTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);

        }
        //�� ������Ʈ �Ÿ��˱�

    }
}
