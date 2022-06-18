using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dd : MonoBehaviour
{
    public Transform tras = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aa();
    }
    void aa()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

    }
}
