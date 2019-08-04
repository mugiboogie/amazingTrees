﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    private Vector3 lookAtPosition;
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
    public float distance = 7.0f;

    private Vector3 setPosition;
    private Vector3 targetPosition;
   
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public LayerMask levelLayers;


    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        distance = CalculateDistance();

        follow = (anchor==null);
        
        if(follow)
        {
            lookAt = target;
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            setPosition = lookAt.position + (Vector3.up*2f) + rotation * dir;
            lookAtPosition = lookAt.position + (Vector3.up * 1f);
            //camTransform.LookAt(lookAt.position);

        }
        else
        {
            //setPosition = anchor.position;

            float distance = Vector3.Distance(new Vector3(target.position.x, 0f, target.position.z), new Vector3(transform.position.x, 0f, transform.position.z));
            if (distance > 10f)
            {
                setPosition += (target.position - transform.position) * Time.deltaTime;
               
            }

            setPosition.y = (target.position.y +1f) - transform.forward.y*6f;
            
            


            lookAt = target;

            
                lookAtPosition = lookAt.position;
            
            //camTransform.LookAt(lookAt.position);
        }

        transform.position = Vector3.Lerp(transform.position, setPosition, 5f * Time.unscaledDeltaTime);

        targetPosition = Vector3.Lerp(targetPosition, lookAtPosition, 4f * Time.unscaledDeltaTime);
        camTransform.LookAt(targetPosition);


        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, 4f * Time.unscaledDeltaTime);
        cam.fieldOfView = currentFOV;


    }


    private float CalculateDistance()
    {
        float defaultDistance = 5f;
        float setDistance = defaultDistance;

        Vector3 origin = lookAt.position + Vector3.up * 2f;
        RaycastHit hit;

        float radius = .3f;
        Vector3 targetDir = -camTransform.forward;

        Debug.DrawLine(origin, origin + targetDir);

        if (Physics.SphereCast(origin, radius, targetDir.normalized, out hit, defaultDistance, levelLayers))
        {
            setDistance = hit.distance;
        }


        if (setDistance < defaultDistance)
        {
            return Mathf.Lerp(distance, setDistance, 10f * Time.unscaledDeltaTime);
        }
        else
        {
            return Mathf.Lerp(distance, setDistance, 10f * Time.unscaledDeltaTime);
        }
    }

}
