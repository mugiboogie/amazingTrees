using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    PlayerControls controls;
    Vector2 cameraMove;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controls.Enable();
        controls.Gameplay.CameraMove.performed += ctx => cameraMove = ctx.ReadValue<Vector2>();
        controls.Gameplay.CameraMove.canceled += ctx => cameraMove = Vector2.zero;
    }

    private void Update()
    {
        Freeform();
    }

    private void Freeform()
    {
        pivot = Vector3.Lerp(pivot,player.position + offset,5f*Time.deltaTime);

        pitch -= cameraMove.y * sensitivityY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        yaw += cameraMove.x * sensitivityX;
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
        setDistanceFinal = Mathf.Lerp(setDistanceFinal, setDistance, setDistanceFinal < setDistance ? (1f * Time.deltaTime):1f); 

        //Debug.DrawLine(pivot, pivot + targetDir);

        //transform.position = Vector3.Lerp(transform.position, pivot-(targetDir*setDistance), 5f * Time.deltaTime);
        transform.position = pivot - (targetDir * setDistanceFinal);
        //transform.eulerAngles = Vector3.Lerp(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f),new Vector3(pitch, yaw, 0f),5f*Time.deltaTime);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        //transform.LookAt(pivot);

    }



}
