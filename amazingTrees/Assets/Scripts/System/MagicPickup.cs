using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    public AudioClip magicPickup;
    public float magicBonus = 15f;

    private GameObject player;
    private PlayerAttack magic;

    private float expirationTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        magic = FindObjectOfType<PlayerAttack>();
        expirationTime = Time.time + 30f;
    }

    private void FixedUpdate()
    {
        if (magic.mana < magic.manaMax)
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
        if(magic.mana < magic.manaMax)
        {
            if (other.gameObject == player)
            {
                AudioSource.PlayClipAtPoint(magicPickup, transform.position);
                Destroy(gameObject);
                magic.mana = magic.mana + magicBonus;

                if(magic.mana > magic.manaMax)
                {
                    magic.mana = magic.manaMax;
                }

                magic.HUDparent.SetTrigger("MPUp");
            }
        }
    }

    private void followPlayer()
    {
        float distanceNeeded = 5f;
        if(Vector3.Distance(transform.position,player.transform.position)<distanceNeeded)
        {
            transform.position += (player.transform.position - transform.position) * 12f * Time.deltaTime;
        }
    }
}
