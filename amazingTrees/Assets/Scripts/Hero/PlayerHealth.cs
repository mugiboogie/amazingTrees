using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    public AudioClip dodge;
    public float resetAfterDeathTime = 5f;

    private Animator anim;
    private PlayerMovement playerMovement;
    private bool playerDead;
    private AudioSource audio;
    private SceneFadeInOut sceneFadeInOut;
    private float timer;


    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        sceneFadeInOut = GameObject.FindGameObjectsWithTag(tag.fader).GetComponent<sceneFadeInOut>;

    }

    void Update()
    {
     if(currentHealth <= 0f)
        {
            if(!playerDead)
            {
                PlayerDying();
            }
            else
            {
                GameOver();
                LevelReset();
            }
        }
    }

    public void TakeDamage(float damageValue, string effect)
    {
        if(!canDamage)
        {
            currentHealth -= 0;
            if ("Dodging")
            {
                AudioSource.PlayClipAtPoint(dodge, transform.position);
            }
            //else ("Stun", "Stagger", "Knockdown", "Knockup", "Knockback"); (this line is giving me issues)
            {
                
            }
        }
        else
        {
           currentHealth -= damageValue;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public void Heal(float healValue)
    {
        healValue = Mathf.RoundToInt(maxHealth / 3);
        currentHealth = currentHealth += healValue;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    void PlayerDying()
    {
        playerDead = true;
        anim.SetBool("isDead", true);
    }

    void GameOver()
    {
        if (anim.GetBool("isDead"))
        {
            anim.SetBool("isDead", false);
        }

        playerMovement.enabled = false;
        audio.Stop();
    }

    void LevelReset()
    {
        timer += Time.deltaTime;

        if (timer >= resetAfterDeathTime)
        {
            sceneFadeInOut.EndScene();
        }
    }
}
