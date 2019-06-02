using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip healthPickup;
    public float healthBonus = 15f;

    private GameObject player;
    private PlayerHealth health;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = FindObjectOfType<PlayerHealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            AudioSource.PlayClipAtPoint(healthPickup, transform.position);
            Destroy(gameObject);
            health.currentHealth = health.currentHealth + healthBonus;
        }

    }
}
