using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DexWhispCluster : MonoBehaviour
{
    public DexWhispBullet[] whispBullets;
    private EnemyDirector enemyDirector;
    private PlayerAttack playerAttack;
    private Animator playerAnim;
    private AudioSource audioVoice;
    private AudioSource audioSound;

    public AudioClip spellSound;
    public AudioClip attackSound;
    private Animator bishoujoEyes;
    private TimeManager timeManager;
    PlayerControls controls;
    private bool buttonSpell;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().anim;
        audioVoice = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroSound").GetComponent<AudioSource>();
        
        
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();

    }

    void OnEnable()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.Spell.performed += ctx => StartCoroutine(inputSpell());
    }

    void OnDisable()
    {
        controls.Disable();
        controls.Gameplay.Spell.performed -= ctx => StartCoroutine(inputSpell());
    }

    IEnumerator inputSpell()
    {
        buttonSpell = true;
        yield return new WaitForEndOfFrame();
        buttonSpell = false;
    }

    void Update()
    {
        playerAttack.spellWeaponVisibleTime = 0f;

        if (enemyDirector.enemies.Count > 0)
        {
            AssignTargets();
        }

        if(buttonSpell && (playerAttack.mana>=playerAttack.manaMax))
        {
            bishoujoEyes = GameObject.FindGameObjectWithTag("PlayerEffects/BishoujoEyes").GetComponent<Animator>();
            //AudioSource.PlayClipAtPoint(attackSound, playerAttack.transform.position);
            audioSound.PlayOneShot(spellSound, 1f);
            audioVoice.PlayOneShot(attackSound, 1f);
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
