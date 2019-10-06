using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViviBeamRifle : MonoBehaviour
{
    private CapsuleCollider collider;
    private PlayerAttack playerAttack;
    private Animator bishoujoEyes;
    private AudioSource audio;
    private TimeManager timeManager;
    private Animator playerAnim;
    private float endTime;
    private EnemyDirector enemyDirector;


    public LayerMask levelLayers;
    public Transform player;
    public AudioClip spellSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public GameObject particleHit;
    public GameObject particleEffect;

    void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().anim;
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();

        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position + Vector3.up;
        transform.rotation = player.transform.rotation;
    }

    void Update()
    {
        if ((Input.GetButtonDown("SpellAttack")) && (playerAttack.mana >= playerAttack.manaMax))
        {
            bishoujoEyes = GameObject.FindGameObjectWithTag("PlayerEffects/BishoujoEyes").GetComponent<Animator>();
            //AudioSource.PlayClipAtPoint(attackSound, playerAttack.transform.position);
            audio.PlayOneShot(spellSound, 1f);
            audio.PlayOneShot(attackSound, 1f);
            bishoujoEyes.SetTrigger("Activate");
            timeManager.DoSlowmotion();
            playerAnim.SetTrigger("Spell");
            playerAttack.mana = 0f;
            endTime = Time.time + 5f;
            StartCoroutine(BeamRifle());

            Vector3 origin = transform.position;
            float radius = collider.radius;
            Vector3 targetDir = transform.forward;
            float defaultDistance = 9999f;
            RaycastHit hit;

            if (Physics.SphereCast(origin, radius, targetDir.normalized, out hit, defaultDistance, levelLayers))
            {
                collider.height = hit.distance;
            }

            collider.center = new Vector3(0, 0, collider.height / 2f);
        }            
    }

    IEnumerator BeamRifle()
    {
        while (Time.time < endTime)
        {
            if (enemyDirector.enemies.Count > 0)
            {
                bool soundPlayed = false;
                for (int i = 0; i < enemyDirector.enemies.Count; i++)
                {
                    if (Vector3.Distance(enemyDirector.enemies[i].transform.position + Vector3.up, playerAttack.transform.position + Vector3.up) < 3f)
                    {
                        enemyDirector.enemies[i].GetComponent<EnemyHealth>().TakeDamage(Random.Range(20f, 30f), "U", playerAttack.transform.position);
                        if (soundPlayed == false)
                        {
                            Instantiate(particleHit, playerAttack.transform.position, Quaternion.identity);
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
