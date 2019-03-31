using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float baseDamage;
    public float damageVariance;
    public string effect;
    public GameObject particleFx;
    private GameObject player;
    //private PlayerHealth playerHealth;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Level"))
        {
            Destroy(this.gameObject);
        }
        else if(other.gameObject == player)
        {
            float appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);
            //playerHealth.Damage(appliedDamage, effect);
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        Instantiate(particleFx, transform.position, transform.rotation);
    }
}
