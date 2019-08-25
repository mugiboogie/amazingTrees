using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip healthPickup;
    public float healthBonus = 15f;

    private GameObject player;
    private PlayerHealth health;
    private PlayerController playerController;
    private bool allMax;

    private float expirationTime;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        health = FindObjectOfType<PlayerHealth>();
        expirationTime = Time.time + 30f;
    }

    private void FixedUpdate()
    {
        if (allMax == false)
        {
            followPlayer();
        }
    }

    private void Update()
    {
        if (Time.time > expirationTime) { Destroy(gameObject); }
        allMax = true;
        for (int i = 0; i < playerController.heroHealth.Length; i++)
        {
            if(playerController.heroHealth[i] < health.maxHealth)
            {
                allMax = false;
            }

        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (allMax == false)
        {
            if (other.gameObject == player)
            {
                AudioSource.PlayClipAtPoint(healthPickup, transform.position);
                Destroy(gameObject);
                for(int i=0; i<playerController.heroHealth.Length; i++)
                {
                    playerController.heroHealth[i] = playerController.heroHealth[i] + healthBonus;
                    if(playerController.heroHealth[i] + healthBonus > health.maxHealth)
                    {
                        playerController.heroHealth[i] = health.maxHealth;
                    }
                }
                health.currentHealth = health.currentHealth + healthBonus;

                if(health.currentHealth > health.maxHealth)
                {
                    health.currentHealth = health.maxHealth;
                }

                health.HUDparent.SetTrigger("HPUp");
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
