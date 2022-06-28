using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameractrl : MonoBehaviour
{
    //target
    public GameObject objTarget = null;
    private Transform cameraTransform;
    private Transform objTargetTransform = null;
    public enum Cameratypestate { first,second,third}
    public Cameratypestate camerastate = Cameratypestate.third;
    [Header("3인칭 카메라")]
    // 떨어진 거리
    public float distance = 6.0f;
    //추가 높이
    public float height = 1.75f;

    //smooth time
    public float heightDamp = 2.0f;
    public float rotationDampin=3f;

    //
    private void LateUpdate()
    {
        if (objTarget == null)
        {
            return;
        }
        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }
        switch (camerastate)
        {
            case Cameratypestate.first:
                ThirdCamera();
                break;
            case Cameratypestate.second:
                second();
                break;
            case Cameratypestate.third:
                first();
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        if (objTarget==null)
        {
            objTargetTransform = objTarget.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    void ThirdCamera()
    {
        //현제 타겟 y축 각도 값
        float objtargetRotationAngle = objTargetTransform.eulerAngles.y;
        //현제 타겟 높이 카메라가 위치한 높이 추가 높이
        float objHeight = objTargetTransform.position.y + height;

        float nowRotationAngle = cameraTransform.eulerAngles.y;
        float nowHeight = cameraTransform.position.y;

        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objtargetRotationAngle, rotationDampin*Time.deltaTime);

        //현제 높이에 대한 damp
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);

        //유니티 각도 조절
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle,0f);

        //카메라 위치 포지션 이동
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        //최종이동
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        cameraTransform.LookAt(objTargetTransform.position);
    }

    [Header("2인칭")]
    public float rotationSpd = 10f;

    void second()
    {
        cameraTransform.RotateAround(objTargetTransform.position,Vector3.up,rotationSpd*Time.deltaTime);
        cameraTransform.LookAt(objTargetTransform.position);
    }
    public float detailx = 5f;
    public float detaily = 5f;

    private float rotationx = 0f;
    private float rotationy = 0f;

    public Transform posfirstTarget = null;

    void first()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        rotationx = cameraTransform.localEulerAngles.y + mousex * detailx;
        rotationx = (rotationx > 180f) ? rotationx - 360f : rotationx;
        rotationy = rotationy + mousey * detaily;
        rotationy = (rotationy > 180f) ? rotationy - 360f : rotationy;

        cameraTransform.localEulerAngles = new Vector3(-rotationy, rotationx, 0f);
        cameraTransform.position = posfirstTarget.position;
    }
}
