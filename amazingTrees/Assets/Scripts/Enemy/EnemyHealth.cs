using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    [HideInInspector] public bool isDead = false;

    private Animator anim;
    private EnemyController enemyController ;
    private AudioSource audio;
    private Rigidbody rb;
    private PlayerAttack playerAttack;
     
    
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
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
                playerAttack.AddSpAttack(damageValue);
                enemyController.damageOrigin = origin;
            }
        }
    }




}
