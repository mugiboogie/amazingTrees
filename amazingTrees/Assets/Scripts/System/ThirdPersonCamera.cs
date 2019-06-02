using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;
    public float sensitivityX;
    public float sensitivityY;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private Camera cam;
    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public LayerMask levelLayers;


    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X")* sensitivityX;
        currentY -= Input.GetAxis("Mouse Y")* sensitivityY;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        distance = CalculateDistance();

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + (Vector3.up * 2f) + rotation * dir;
        camTransform.LookAt(lookAt.position);
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
        

        if(setDistance<defaultDistance)
        {
            return Mathf.Lerp(distance, setDistance, 10f * Time.deltaTime);
        }
        else
        {
            return Mathf.Lerp(distance, setDistance, 10f * Time.deltaTime);
        }
    }
}
