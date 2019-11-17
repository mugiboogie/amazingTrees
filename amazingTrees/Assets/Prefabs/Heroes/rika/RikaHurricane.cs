using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikaHurricane : MonoBehaviour
{
    private EnemyDirector enemyDirector;
    private PlayerAttack playerAttack;
    private Animator playerAnim;
    private AudioSource audioVoice;
    private AudioSource audioSound;

    public AudioClip spellSound;
    public AudioClip attackSound;
    public AudioClip hitSound;

    private Animator bishoujoEyes;
    private TimeManager timeManager;

    public GameObject particleEffect;
    public GameObject particleHit;


    private float endTime;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().anim;
        audioVoice = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroSound").GetComponent<AudioSource>();


        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
    }

    void Update()
    {
        playerAttack.spellWeaponVisibleTime = 0f;

        if ((Input.GetButtonDown("SpellAttack")) && (playerAttack.mana >= playerAttack.manaMax))
        {
            bishoujoEyes = GameObject.FindGameObjectWithTag("PlayerEffects/BishoujoEyes").GetComponent<Animator>();
            //AudioSource.PlayClipAtPoint(attackSound, playerAttack.transform.position);
            audioSound.PlayOneShot(spellSound, 1f);
            audioVoice.PlayOneShot(attackSound, 1f);
            bishoujoEyes.SetTrigger("Activate");
            timeManager.DoSlowmotion();
            playerAnim.SetTrigger("Spell");
            playerAttack.mana = 0f;
            endTime = Time.time + 5f;
            StartCoroutine(BlastHurricane());
        }
    }

    

    IEnumerator BlastHurricane()
    {
        while (Time.time < endTime)
        {
            if (enemyDirector.enemies.Count > 0)
            {
                bool soundPlayed = false;
                for(int i=0; i<enemyDirector.enemies.Count; i++)
                {
                    if(Vector3.Distance(enemyDirector.enemies[i].transform.position + Vector3.up,playerAttack.transform.position + Vector3.up)<3f)
                    {
                        enemyDirector.enemies[i].GetComponent<EnemyHealth>().TakeDamage(Random.Range(20f,30f), "U", playerAttack.transform.position);
                        if (soundPlayed == false)
                        {
                            Instantiate(particleHit,playerAttack.transform.position,Quaternion.identity);
                            audio.PlayOneShot(hitSound, 1f);
                            soundPlayed = true;
                        }
                    }
                }
            }
            Instantiate(particleEffect, playerAttack.transform, false);
            yield return new WaitForSeconds(.25f);
        }
    }
}
