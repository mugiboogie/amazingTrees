using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;

    private Animator anim;
    private EnemyController enemyController ;
    private bool enemyDead;
    private AudioSource audio;
     
    
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
     if(currentHealth <= 0f)
        {
            if(!enemyDead)
            {
                EnemyDying();
            }
            else
            {
                EnemyRemove();
            }
        }
    }

    public void TakeDamage(float damageValue, string effect)
    {
        if(!canDamage)
        {
            currentHealth -= 0;
        }
        else
        {
           currentHealth -= damageValue;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }


    void EnemyDying()
    {
        enemyDead = true;
        anim.SetBool("isDead", true);
    }

    void EnemyRemove()
    {
        if (anim.GetBool("isDead"))
        {
            anim.SetBool("isDead", false);
        }

        //EnemyController.enabled = false;
    }

}
