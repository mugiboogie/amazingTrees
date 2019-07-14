using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexWhispCluster : MonoBehaviour
{
    public DexWhispBullet[] whispBullets;
    private EnemyDirector enemyDirector;
    private PlayerAttack playerAttack;
    private Animator playerAnim;
    private AudioSource audio;

    public AudioClip spellSound;
    public AudioClip attackSound;
    private Animator bishoujoEyes;
    private TimeManager timeManager;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        bishoujoEyes = GameObject.FindGameObjectWithTag("PlayerEffects/BishoujoEyes").GetComponent<Animator>();
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
    }

    void Update()
    {
        if (enemyDirector.enemies.Count > 0)
        {
            AssignTargets();
        }

        if((Input.GetButtonDown("SpellAttack"))&&(playerAttack.mana>=playerAttack.manaMax))
        {
            //AudioSource.PlayClipAtPoint(attackSound, playerAttack.transform.position);
            audio.PlayOneShot(spellSound, 1f);
            audio.PlayOneShot(attackSound, 1f);
            bishoujoEyes.SetTrigger("Activate");
            timeManager.DoSlowmotion();
            playerAnim.SetTrigger("Spell");
            playerAttack.mana = 0f;
            BlastWhisp();
        }
    }

    void AssignTargets()
    {
        int j = 0;
        for(int i=0; i<whispBullets.Length; i++)
        {
            if(j>enemyDirector.enemies.Count-1)
            {
                j = 0;
            }

            if ((whispBullets[i].target == null) || ((whispBullets[i].target!=null)&&(!enemyDirector.enemies.Contains(whispBullets[i].target.gameObject))))
            {
                whispBullets[i].target = enemyDirector.enemies[j].transform;
            }
            j++;
        }
    }

    void BlastWhisp()
    {
        for (int i = 0; i < whispBullets.Length; i++)
        {
            whispBullets[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < whispBullets.Length; i++)
        {
            whispBullets[i].gameObject.SetActive(true);
        }
    }
}
