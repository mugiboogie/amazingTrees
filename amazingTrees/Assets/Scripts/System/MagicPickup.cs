using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    public AudioClip magicPickup;
    public float magicBonus = 15f;

    private GameObject player;
    private PlayerAttack magic;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        magic = FindObjectOfType<PlayerAttack>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            AudioSource.PlayClipAtPoint(magicPickup, transform.position);
            Destroy(gameObject);
            magic.mana = magic.mana + magicBonus;
        }

    }
}
