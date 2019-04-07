using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public string effect;
    public GameObject particleFx;
    private GameObject player;
    private PlayerHealth playerHealth;
    private Rigidbody rb;

    private float lifetime;

    private Vector3 origin;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
        lifetime = Time.time + 10f;
        origin = transform.position;
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        
        if(Time.time>lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Level"))
        {
            Destroy(this.gameObject);
        }
        else if(other.gameObject == player)
        {
           
            playerHealth.TakeDamage(damage, effect, origin);

            if (playerHealth.canDamage == true)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnDestroy()
    {
        Instantiate(particleFx, transform.position, transform.rotation);
    }
}
