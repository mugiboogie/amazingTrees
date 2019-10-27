using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPickup : MonoBehaviour
{
    public AudioClip strengthPickup;
    public float strengthDuration;
    private GameObject player;
    private PlayerAttack playerAttack;
    private float expirationTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = FindObjectOfType<PlayerAttack>();
        expirationTime = Time.time + 30f;
    }

    private void FixedUpdate()
    {
        if (Time.time > playerAttack.buffTime)
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
        if (Time.time > playerAttack.buffTime)
        { 
            if (other.gameObject == player)
            {
                AudioSource.PlayClipAtPoint(strengthPickup, transform.position);
                Destroy(gameObject);
                playerAttack.buffTime = Time.time + strengthDuration;
                playerAttack.HUDparent.SetTrigger("STRUp");
            }
        }
    }

    private void followPlayer()
    {
        float distanceNeeded = 5f;
        if (Vector3.Distance(transform.position, player.transform.position) < distanceNeeded)
        {
            transform.position += (player.transform.position - transform.position) * 12f * Time.deltaTime;
        }
    }
}
