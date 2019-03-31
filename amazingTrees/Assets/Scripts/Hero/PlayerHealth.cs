using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    public AudioClip Dashing;
    public float resetAfterDeathTime = 5f;

    private Animator anim;
    private PlayerMovement playerMovement;
    private bool playerDead;
    private AudioSource audio;
    //private SceneFadeInOut sceneFadeInOut;
    private float timer;
    private bool dash;



    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        //sceneFadeInOut = GameObject.FindGameObjectsWithTag(tag.fader).GetComponent<sceneFadeInOut>;
        currentHealth = maxHealth;
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
           if (anim.GetBool("Dashing"))
            {
                AudioSource.PlayClipAtPoint(Dashing, transform.position);
            }
            else 
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

        //playerMovement.enabled = false;
        //audio.Stop();
    }

    void LevelReset()
    {
        timer += Time.deltaTime;

        if (timer >= resetAfterDeathTime)
        {
            //sceneFadeInOut.EndScene();
        }
    }
}
