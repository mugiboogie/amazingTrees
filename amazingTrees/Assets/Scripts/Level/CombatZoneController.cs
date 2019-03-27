using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZoneController : MonoBehaviour
{
    public Transform cameraAnchor;

    private GameObject player;
    private CameraController camController;

    void Awake()
    {
        camController = Camera.main.GetComponent<CameraController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            camController.anchor = cameraAnchor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camController.anchor = null;
        }
    }
}
