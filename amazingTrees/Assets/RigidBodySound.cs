using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodySound : MonoBehaviour
{
    private Rigidbody rb;

    public AudioClip[] collisionSounds;

    float releaseTime;
    float cooldown;

    public bool canDealDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        releaseTime = Time.time + .125f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((Time.time > releaseTime) && (Time.time>cooldown))
        {
            cooldown = Time.time + .125f;
            AudioSource.PlayClipAtPoint(collisionSounds[Random.Range(0, collisionSounds.Length)], transform.position);
        }

        if ((canDealDamage) && (rb.velocity.magnitude>1f))
        {
            EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();
            //DestructablesHealth destructableHealth = collision.collider.GetComponent<DestructablesHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(rb.mass, "S", transform.position);
            }
            /*if (destructableHealth != null)
            {
                destructableHealth.TakeDamage(rb.mass, "S", transform.position);
            }*/
        }
    }
}
