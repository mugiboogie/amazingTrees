using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablesHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject destroyedObj;

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
            if(!playerDead)
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
}
