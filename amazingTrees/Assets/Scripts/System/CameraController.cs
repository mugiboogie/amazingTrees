using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f,1f,0f);
    public float turnSpeed = 5f;
    public float followSpeed = 5f;
    public bool follow;
    public Transform anchor;
    public float desiredFOV;
    private float currentFOV;

    private Vector3 lookAt;
    private Vector3 setPosition;
    private Camera camera;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera = Camera.main.GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        lookAt = Vector3.Lerp(lookAt, target.position + targetOffset, Time.deltaTime * turnSpeed);
        transform.LookAt(lookAt);

        
        follow = (anchor==null);
        

        if(follow)
        {
            setPosition = target.position + (new Vector3(0f, 2f, 2f));
        }
        else
        {
            setPosition = anchor.position;
        }

        transform.position = Vector3.Lerp(transform.position, setPosition, followSpeed * Time.deltaTime);

        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, 1f * Time.deltaTime);
        camera.fieldOfView = currentFOV;

    }
}
