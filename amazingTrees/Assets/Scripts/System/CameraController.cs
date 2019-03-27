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

    private Vector3 lookAt;
    private Vector3 setPosition;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
    }
}
