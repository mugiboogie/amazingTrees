using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    public Image healthBar;
    public Image healthBarBurn;
    [HideInInspector] public bool isDead = false;
    public AudioClip[] hits;
    public AudioClip[] dead;
    public GameObject deathParticle;
    public AudioClip deathPSound;

    private bool deathPlayed;
    private Animator anim;
    private EnemyController enemyController ;
    private ParticleController particleController;
    private AudioClipController audioClipController;
    private AudioSource audio;
    private Rigidbody rb;
    private PlayerAttack playerAttack;
    private float healthBarBurnTime;
    private CapsuleCollider CapCol;


     
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        currentHealth = maxHealth;
        particleController = GameObject.FindGameObjectWithTag("ParticleController").GetComponent<ParticleController>();
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
        CapCol = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
         if(currentHealth <= 0f)
         {
            anim.SetBool("isDead", true);
            isDead = true;

            if (deathPlayed == false)
            {
                deathPlayed = true;
                PlayDead(transform.position);
            }

        }
         else
        {
            isDead = false;
        }

        healthBar.transform.parent.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);

        if(Time.time>healthBarBurnTime)
        {
            //Set the HealthBarBurn to be the same value as health.
            healthBarBurn.fillAmount = Mathf.Lerp(healthBarBurn.fillAmount,healthBar.fillAmount,5f*Time.deltaTime);
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
                if (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("KnockUp"))
                {
                    switch (effect)
                    {
                        case "H":
                            anim.SetTrigger("Hit");
                            break;
                        case "S":
                            anim.SetTrigger("Stun");
                            break;
                        case "U":
                            anim.SetTrigger("KnockUp");
                            //playerMovement.KnockUp(10f);
                            enemyController.knockUpVelocity = 10f;
                            //rb.AddForce(Vector3.up * 10f);
                            break;
                        case "D":
                            anim.SetTrigger("KnockDown");
                            break;
                        case "B":
                            anim.SetTrigger("KnockBack");
                            break;
                    }
                }
                else
                {
                    enemyController.knockUpVelocity = 2f;
                }

                currentHealth -= damageValue;
                healthBar.fillAmount = currentHealth / maxHealth;
                healthBarBurnTime = Time.time + .25f;
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
                playerAttack.AddSpAttack(damageValue);
                enemyController.damageOrigin = origin;

                particleController.CreateParticle(transform.position + Vector3.up, damageValue);
                audioClipController.PlayHit(transform.position);
                PlayHits(transform.position);
            }
        }
    }

    public void PlayDead(Vector3 position)
    {
        AudioClip clip = dead[Random.Range(0, hits.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
        CapCol.enabled = false;Instantiate(deathParticle, transform.position + Vector3.up, transform.rotation);
        AudioSource.PlayClipAtPoint(deathPSound, position);
    }

    public void PlayHits(Vector3 position)
    {
        AudioClip clip = hits[Random.Range(0, hits.Length)];
        //audio.PlayOneShot(clip, 1f);
        AudioSource.PlayClipAtPoint(clip, position);
    }


}
