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
                //rigidbody �ȿ� �֤��� ���� ����� ���
                rigidbody.AddExplosionForce(power, poSkillEffect, radius, flyingSize);
            }
        }
    }
}
    
