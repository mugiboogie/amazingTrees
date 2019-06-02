using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexWhispBullet : MonoBehaviour
{
    public Transform target;

    private float lifeTime;
    private float attackTime;
    private Transform player;
    private Vector3 destination;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lifeTime = Time.time + 3f;
        rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        
        transform.position = player.position + Vector3.up;
        transform.rotation = Random.rotation;
        lifeTime = Time.time + 10f + Random.Range(.25f,2f);
        attackTime = Time.time + .25f;
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * 20f);

    }
    void FixedUpdate()
    {

        float forceValue = 100f;

        if (target != null)
        {
            if (Time.time > attackTime)
            {
                transform.LookAt(target.position + Vector3.up);
            }

            forceValue -= (Vector3.Angle(((target.position + Vector3.up) - transform.position), transform.forward) / 180f) * 100f;


           // rb.AddForce(((target.position + Vector3.up) - transform.position).normalized * forceValue);

        }
        //else
        //{

            rb.AddForce(transform.forward * forceValue);
        //}
        

        


        
        
        

        

        if(Time.time>lifeTime)
        {
            target = null;
            this.gameObject.SetActive(false);
            transform.position = player.position + Vector3.up;
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (Time.time > attackTime)
        {
            if (other.CompareTag("Enemy"))
            {
                target = null;
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(Random.Range(15f, 25f), "S", transform.position);
                this.gameObject.SetActive(false);
            }
        }
    }
}
