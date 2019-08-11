using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float baseDamage;
    public float heavyAttackChance;
    public float damageVariance;
    public float attackRange;
    public float passiveRange;
    public bool ranged;
    public bool hitscan;
    public string attackEffect;
    public GameObject weakProjectile;
    public GameObject heavyProjectile;
    private GameObject currentProjectile;
    public bool setAttack;
    public float cooldownTime;
    private float nextAttack;
    private Animator anim;
    public LayerMask affectedLayers;
    public float tauntTime;
    private bool willAttack;
    private float beginAttack;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyController enemyController;
    private CapsuleCollider col;
    private float stutterTime;
    private CameraShake cameraShake;
    private Vector3 hitscanTarget;
    public Animator warningIndicator;
    private string status;
    public string[] attacks = new string[2];
    private AudioClipController audioClipController;
    private AudioSource audio;
    public AudioClip[] angery;
    public GameObject[] hitWeakVFX;
    public AudioClip[] hitWeakSFX;
    public GameObject[] hitHeavyVFX;
    public AudioClip[] hitHeavySFX;
    private int index;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        enemyController = GetComponent<EnemyController>();
        col = GetComponent<CapsuleCollider>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audio = GetComponent<AudioSource>();
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
    }

    void Update()
    {

        float chooseAttack = Random.value;
        anim.SetBool("Attack", false);

        if (setAttack)
        {
            enemyController.desiredDistance = attackRange;
        }
        else
        {
            enemyController.desiredDistance = passiveRange;
        }

        if ((setAttack==true) && (Vector3.Distance(transform.position, player.position)<=(attackRange+1f))&&(anim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("Attack")) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("KnockUp")))
        {
            if (chooseAttack < heavyAttackChance)
            {
                index = 1;
                currentProjectile = heavyProjectile;
            }
            else
            {
                index = 0;
                currentProjectile = weakProjectile;
            }

            anim.SetInteger("AttackSkill", index);

            PlayAngery(transform.position);
            warningIndicator.SetTrigger("Warning");
            string[] propertyArray = attacks[index].Split(char.Parse("/"));
            baseDamage = float.Parse(propertyArray[0]);
            status = propertyArray[1];
            anim.SetTrigger("Attack");
        }


        anim.SetBool("AttackCancel", ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("KnockUp"))));


        anim.enabled = (Time.time > stutterTime);

      

        hitscanTarget = Vector3.Lerp(hitscanTarget, player.position+Vector3.up, 2.5f * Time.deltaTime);
        //Debug.DrawLine(transform.position + Vector3.up, hitscanTarget, Color.green);

    }

    void FixedUpdate()
    {
        warningIndicator.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }

    void Attack()
    {
        playerHealth.CheckDodge(transform.position);

        float appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);

        setAttack = false;

        if(ranged==false)
        {
            //Melee Unit
            Collider[] hitcollider = Physics.OverlapSphere(transform.position + Vector3.up*col.height/2f + transform.forward*attackRange/2f, attackRange/2f);

            for (int i = 0; i < hitcollider.Length; i++)
            {
                if (hitcollider[i].gameObject.CompareTag("Player"))
                {
                    GameObject[] hitVFXSelected = (index == 0 ? hitWeakVFX : hitHeavyVFX);
                    AudioClip[] hitSFXSelected = (index == 0 ? hitWeakSFX : hitHeavySFX);
                    playerHealth.TakeDamage(appliedDamage, status, transform.position, hitVFXSelected, hitSFXSelected);
                    StartCoroutine(cameraShake.Shake(.1f, .005f * appliedDamage));
                    stutterTime = Time.time + .125f;
                }

            }
            
        }
        else
        {
            if (hitscan == false)
            {
                float projectileHeight = col.height / 2f;
                GameObject projectileObj = Instantiate(currentProjectile, transform.position + Vector3.up * projectileHeight, transform.rotation);
                projectileObj.GetComponent<EnemyProjectile>().damage = appliedDamage;
                projectileObj.GetComponent<EnemyProjectile>().effect = status;
            }
            else
            {
                RaycastHit hit;
                Vector3 origin = transform.position + Vector3.up;
                

                if(Physics.Raycast(origin, (hitscanTarget - origin).normalized, out hit, Mathf.Infinity, affectedLayers))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        GameObject[] hitVFXSelected = (index == 0 ? hitWeakVFX : hitHeavyVFX);
                        playerHealth.TakeDamage(appliedDamage, status, transform.position, hitVFXSelected);
                        StartCoroutine(cameraShake.Shake(.1f, .005f * appliedDamage));
                        stutterTime = Time.time + .125f;
                    }
                }
            }
            
        }
    }

    public void PlayAngery(Vector3 position)
    {
        AudioClip clip = angery[Random.Range(0, angery.Length)];
        //audio.PlayOneShot(clip, 1f);
        AudioSource.PlayClipAtPoint(clip, position);
    }

}
