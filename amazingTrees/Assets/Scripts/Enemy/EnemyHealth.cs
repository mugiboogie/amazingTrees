﻿using System.Collections;
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

    private Animator anim;
    private EnemyController enemyController ;
    private AudioSource audio;
    private Rigidbody rb;
    private PlayerAttack playerAttack;
    private float healthBarBurnTime;
     
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        currentHealth = maxHealth;
        
        
    }

    void Update()
    {
         if(currentHealth <= 0f)
         {
            anim.SetBool("isDead", true);
            isDead = true;
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
            }
        }
    }




}
