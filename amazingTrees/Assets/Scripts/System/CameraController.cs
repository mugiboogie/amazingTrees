using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;
    public float sensitivityX;
    public float sensitivityY;

    public Transform target;
    //public Vector3 targetOffset = new Vector3(0f,1f,0f);
    //public float turnSpeed = 5f;
    public float followSpeed = 5f;
    public bool follow;
    public Transform anchor;
    public float desiredFOV;
    private float currentFOV;

    private Vector3 setPosition;
   
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private Camera cam;
    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        follow = (anchor==null);
        
        if(follow)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            LateUpdate();
        }
        else
        {
            setPosition = anchor.position;
        }

        transform.position = Vector3.Lerp(transform.position, setPosition, followSpeed * Time.deltaTime);

        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, 1f * Time.deltaTime);
        cam.fieldOfView = currentFOV;

    }

    //private void Update()
    //{
    //currentX += Input.GetAxis("Mouse X") * sensitivityX;
    //currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

    // currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    //}

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
