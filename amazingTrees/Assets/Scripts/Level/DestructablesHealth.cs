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
    public bool canDamage;
    [HideInInspector] public bool isDead = false;

    private AudioClipController audioClipController;
    private bool deathPlayed;
    private bool playerDead;
    private AudioSource audio;
    private float timer;

    private ParticleController particleController;

    private Transform player;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
        particleController = GameObject.FindGameObjectWithTag("ParticleController").GetComponent<ParticleController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

    public void TakeDamage(float damageValue, string effect, Vector3 origin)
    {
       
        if (!canDamage)
        {
            currentHealth -= 0;
        }
        else
        {
            if (!isDead)
            {
                currentHealth -= damageValue;
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            }
        }

        particleController.CreateDestructableParticle(transform.position);
        audioClipController.PlayHit(transform.position);
        PlayHits(transform.position);
    }

    void ObjDying()
    {
        playerDead = true;
    }

    void Dead()
    {
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
