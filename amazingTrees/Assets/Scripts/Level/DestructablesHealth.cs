using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablesHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject destroyedObj;
    public AudioClip[] hits;
    public AudioClip[] dead;

    private bool deathPlayed;
    private Animator anim;
    private PlayerMovement playerMovement;
    private bool playerDead;
    private AudioSource audio;
    private float timer;



    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();

    }

    void Update()
    {
     if(currentHealth <= 0f)
        {
            if (deathPlayed == false)
            {
                deathPlayed = true;
                PlayDead(transform.position);
            }

            if (!playerDead)
            {
                ObjDying();
            }
            else
            {
                Dead();
            }
        }
    }

    public void TakeDamage(float damageValue, string effect)
    {
            currentHealth -= damageValue;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            PlayHits(transform.position);
    }

    void ObjDying()
    {
        playerDead = true;
        anim.SetBool("isDead", true);
    }

    void Dead()
    {
        if (anim.GetBool("isDead"))
        {
            anim.SetBool("isDead", false);
        }
        Destroy(gameObject);
        Instantiate(destroyedObj, transform.position, transform.rotation);
    }

    public void PlayDead(Vector3 position)
    {
        AudioClip clip = dead[Random.Range(0, hits.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayHits(Vector3 position)
    {
        AudioClip clip = hits[Random.Range(0, hits.Length)];
        //audio.PlayOneShot(clip, 1f);
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
