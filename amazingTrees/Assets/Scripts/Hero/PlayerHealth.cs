using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    public AudioClip Dashing;
    public float resetAfterDeathTime = 5f;

    private Animator anim;
    private PlayerMovement playerMovement;
    [HideInInspector] public bool playerDead;
    private AudioSource audio;
    private bool dash;

    private UIHealthBar healthBar;
    private float gameOverTime;

    private CameraShake cameraShake;

    private float invincibilityTime; // An anti-spam function that prevents the enemy from dealing too much damage after an attack.

    [HideInInspector] public float damageTaken;

    private PlayerAttack playerAttack;

    public GameObject invinciblityDebug;
    


    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<UIHealthBar>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    void Update()
    {
        
        
            if(invinciblityDebug != null)
            {
                invinciblityDebug.SetActive(Time.time < playerMovement.dashDuration);
            }
        

        if (currentHealth <= 0f)
        {
            playerDead = true;
            anim.SetBool("isDead", true);
        }
        else
        {
            gameOverTime = Time.time + 5f;
        }

        if(Time.time>gameOverTime)
        {
            GameOver();
        }

        if((anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
        {
            invincibilityTime = Time.time + .5f;
        }

        //Set canDamage
        canDamage = ((!playerDead) && ((Time.time>invincibilityTime) && (Time.time > playerMovement.dashDuration) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp"))));
        


    }

    public void TakeDamage(float damageValue, string effect, Vector3 hitOrigin)
    {
        anim.SetBool("Hit", false);
        anim.SetBool("Stun", false);
        anim.SetBool("KnockUp", false);
        anim.SetBool("KnockDown", false);
        anim.SetBool("KnockBack", false);

        if (!canDamage)
        {
            currentHealth -= 0;
           if (Time.time <= playerMovement.dashDuration)
            {
                AudioSource.PlayClipAtPoint(Dashing, transform.position);
                
            }
            else 
            {
              
            }
        }
        else
        {
            playerAttack.comboChainReset = Time.time;
            playerAttack.manaRegenTime = Time.time + 2f;
            StartCoroutine(cameraShake.Shake(.1f, .005f * damageValue));
            healthBar.burnTime = Time.time + .5f;
           currentHealth -= damageValue;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerMovement.hitPosition = hitOrigin;

            switch(effect)
            {
                case "H":
                    anim.SetTrigger("Hit");
                    break;
                case "S":
                    anim.SetTrigger("Stun");
                    break;
                case "U":
                    anim.SetTrigger("KnockUp");
                    playerMovement.KnockUp(10f);
                    break;
                case "D":
                    anim.SetTrigger("KnockDown");
                    break;
                case "B":
                    anim.SetTrigger("KnockBack");
                    break;
            }


            damageTaken += damageValue;

            //playerMovement.stutterTime = Time.time + .125f;
        }
    }

    public void Heal(float healValue)
    {
        healValue = Mathf.RoundToInt(maxHealth / 3);
        currentHealth = currentHealth += healValue;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }


    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
