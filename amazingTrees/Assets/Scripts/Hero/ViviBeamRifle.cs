using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViviBeamRifle : MonoBehaviour
{
    private CapsuleCollider collider;
    public LayerMask levelLayers;
    public Transform player;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position + Vector3.up;
        transform.rotation = player.transform.rotation;
    }

    void Update()
    {
        Vector3 origin = transform.position;
        float radius = collider.radius;
        Vector3 targetDir = transform.forward;
        float defaultDistance = 9999f;
        RaycastHit hit;

        if (Physics.SphereCast(origin, radius, targetDir.normalized, out hit, defaultDistance, levelLayers))
        {
            collider.height = hit.distance / 2f;
        }

        collider.center = new Vector3(0, 0, collider.height / 2f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            enemyHealth.TakeDamage(9999, "S", transform.position);

        }
    }
}
