using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillEffectCtrl : MonoBehaviour
{
    public float radius = 5f;
    public float power = 200f;
    public float flyingSize = 3f;
    // Start is called before the first frame update
    private void Start()
    {
        Vector3 poSkillEffect = transform.position;
        Collider[] colliders = Physics.OverlapSphere(poSkillEffect, radius);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("PlayerAtk") == true)
            {
                continue;
            }
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                //rigidbody 안에 있ㄴㄴ 폭발 기능을 사용
                rigidbody.AddExplosionForce(power, poSkillEffect, radius, flyingSize);
            }
        }
    }
}
    
