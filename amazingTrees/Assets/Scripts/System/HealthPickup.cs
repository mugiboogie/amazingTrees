using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip healthPickup;
    public float healthBonus = 15f;

    private GameObject player;
    private PlayerHealth health;

    private float expirationTime;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = FindObjectOfType<PlayerHealth>();
        expirationTime = Time.time + 30f;
    }

    private void FixedUpdate()
    {
        if (health.currentHealth < health.maxHealth)
        {
            followPlayer();
        }
    }

    private void Update()
    {
        if (Time.time > expirationTime) { Destroy(gameObject); }
    }


    void OnTriggerEnter(Collider other)
    {
        if (health.currentHealth < health.maxHealth)
        {
            if (other.gameObject == player)
            {
                AudioSource.PlayClipAtPoint(healthPickup, transform.position);
                Destroy(gameObject);
                health.currentHealth = health.currentHealth + healthBonus;

                if(health.currentHealth > health.maxHealth)
                {
                    health.currentHealth = health.maxHealth;
                }
            }
        }

    }

    private void followPlayer()
    {
        float distanceNeeded = 5f;
        if (Vector3.Distance(transform.position, player.transform.position) < distanceNeeded)
        {
            transform.position += (player.transform.position - transform.position) * 8f * Time.deltaTime;
        }
    }
}
