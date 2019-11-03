using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController;
    public float maxHealth = 100f;
    public float currentHealth;
    public bool canDamage;
    public AudioClip Dashing;
    public AudioClip[] hits;
    public AudioClip dead;
    public float resetAfterDeathTime = 5f;

    private bool deathPlayed;
    private Animator anim;
    private PlayerMovement playerMovement;
    [HideInInspector] public bool playerDead;
    private AudioSource audioVoice;
    private AudioSource audioSound;
    private bool dash;

    private UIHealthBar healthBar;
    private float gameOverTime;

    private CameraShake cameraShake;

    private float invincibilityTime; // An anti-spam function that prevents the enemy from dealing too much damage after an attack.

    [HideInInspector] public float damageTaken;

    private PlayerAttack playerAttack;

    public GameObject invinciblityDebug;

    private TimeManager timeManager;
    private ParticleController particleController;

    private AudioClipController audioClipController;


    private float damageReduction; //1 = all damage taken is taken at full damage, 0 = invulnerable.

    [HideInInspector] public Animator HUDparent;

    private float deadTime;

    public void SummonHero()
    {
        anim = playerController.anim;
        hits = playerController.hero.injuredSfx;
        dead = playerController.hero.deathSfx;
        damageReduction = playerController.hero.damageReduction;

    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
       
        playerMovement = GetComponent<PlayerMovement>();
        audioVoice = transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = transform.Find("HeroSound").GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<UIHealthBar>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        particleController = GameObject.FindGameObjectWithTag("ParticleController").GetComponent<ParticleController>();
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
        HUDparent = GameObject.FindGameObjectWithTag("HUDParent").GetComponent<Animator>();
    }

    void Update()
    {
        if (anim != null)
        {

            if (invinciblityDebug != null)
            {
                invinciblityDebug.SetActive(Time.time < playerMovement.dashDuration);
            }


            
            if (currentHealth <= 0f)
            {
                playerDead = true;
                anim.SetBool("isDead", true);

                if (deathPlayed == false)
                {
                    deathPlayed = true;
                    AudioClip clip = dead;
                    audioSound.PlayOneShot(clip, 1f);
                }
                if(deadTime<5f) { deadTime += Time.deltaTime; }
                else { playerController.TrySummonHero(); }
                playerAttack.mana = 0f;
            }
            else
            {
                playerDead = false;
                anim.SetBool("isDead", false);
                deathPlayed = false;
                deadTime = 0f;
            }

            if(playerController.allHeroesDead == false)
            {
                gameOverTime = Time.time + 5f;
            }




            if (Time.time > gameOverTime)
            {
                GameOver();
            }

            if ((anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
            {
                invincibilityTime = Time.time + .5f;
            }

            //Set canDamage
            canDamage = ((!playerDead) && ((Time.time > invincibilityTime) && (Time.time > playerMovement.dashDuration) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp"))));

        }

    }

    public void TakeDamage(float damageValue, string effect, Vector3 hitOrigin, GameObject[] hitVFX, AudioClip[] hitSFX)
    {
        damageValue = damageValue * damageReduction;
        if (anim != null)
        {
            anim.SetBool("Hit", false);
            anim.SetBool("Stun", false);
            anim.SetBool("KnockUp", false);
            anim.SetBool("KnockDown", false);
            anim.SetBool("KnockBack", false);

            if (!canDamage)
            {
                currentHealth -= 0;

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

                if ((hitVFX == null) || (hitVFX.Length==0))
                {
                    particleController.CreateEnemyParticle(transform.position + Vector3.up, damageValue);
                    
                }
                else
                {
                    particleController.CreateEnemyParticleSpecific(transform.position + Vector3.up, hitVFX);
                }

                if ((hitSFX == null) || (hitSFX.Length == 0))
                {
                    audioClipController.PlayHit(transform.position + Vector3.up);

                }
                else
                {
                    audioClipController.PlayHitSpecific(transform.position + Vector3.up, hitSFX);
                }

                PlayHits(transform.position);

                HUDparent.SetTrigger("Hit");
                //playerMovement.stutterTime = Time.time + .125f;
            }
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

    public void CheckDodge(Vector3 attackLocation)
    {
        float range = 2.5f;
        if (Time.time <= playerMovement.dashDuration)
        {
            if (Vector3.Distance(attackLocation, transform.position) < range)
            {
                AudioSource.PlayClipAtPoint(Dashing, transform.position);
                timeManager.DoSlowmotion();
                invincibilityTime = Time.time + 1f;
            }
        }
    }

    public void PlayHits(Vector3 position)
    {
        if (hits.Length > 0) { AudioClip clip = hits[Random.Range(0, hits.Length)]; audioSound.PlayOneShot(clip, 1f); }        
        
        //AudioSource.PlayClipAtPoint(clip, position);
    }
}
