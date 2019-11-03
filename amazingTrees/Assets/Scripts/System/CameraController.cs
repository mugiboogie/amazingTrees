using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float sensitivityY;
    public float sensitivityX;
    public LayerMask levelLayers;

    private Transform player;
    private float pitch;
    private float yaw;
    private Vector3 pivot;
    private float setDistanceFinal;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Freeform();
    }

    private void Freeform()
    {
        pivot = Vector3.Lerp(pivot,player.position + offset,5f*Time.deltaTime);

        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        if(yaw>360f) { yaw = 0f; }

        Vector3 targetDir = Vector3.forward;
        targetDir = Quaternion.Euler(pitch, yaw, 0f) * targetDir;

        float desiredDistance = 7f;
        float radius = .0125f;
        float setDistance = desiredDistance - radius;
        RaycastHit hit;

        if (Physics.SphereCast(pivot, radius, -targetDir.normalized, out hit, desiredDistance, levelLayers))
        {
            setDistance = hit.distance - radius;
        }

        float setDistanceSpeed = setDistanceFinal < setDistance ? 1f : 10f;
        setDistanceFinal = Mathf.Lerp(setDistanceFinal, setDistance, setDistanceSpeed * Time.deltaTime); 

        //Debug.DrawLine(pivot, pivot + targetDir);

        //transform.position = Vector3.Lerp(transform.position, pivot-(targetDir*setDistance), 5f * Time.deltaTime);
        transform.position = pivot - (targetDir * setDistanceFinal);
        //transform.eulerAngles = Vector3.Lerp(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f),new Vector3(pitch, yaw, 0f),5f*Time.deltaTime);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        //transform.LookAt(pivot);

    }



}
