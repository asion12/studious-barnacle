using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padong : MonoBehaviour
{

    public Transform tras = null;
    public GameObject pa = null;

    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("fds", 5f, 3f);

    }

    // Update is called once per frame
    void Update()
    {
      
        aa();
    }
    void fds()
    {
     
        Vector3 vecSpawn = new Vector3(tras.position.x, tras.position.y, tras.position.z);

        //생성할 오브젝트
        GameObject a = Instantiate(pa, vecSpawn, Quaternion.identity);

    }
    void aa()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
       
    }
}
