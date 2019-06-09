using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float baseDamage;
    public float damageVariance;
    public float attackRange;
    public float passiveRange;
    public bool ranged;
    public bool hitscan;
    public string attackEffect;
    public GameObject projectile;
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

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        enemyController = GetComponent<EnemyController>();
        col = GetComponent<CapsuleCollider>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
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

            warningIndicator.SetTrigger("Warning");
            if(chooseAttack > 0.75f)//25% chance for heavy attack
            {
                baseDamage = 30f;
                status = "S";
                anim.SetTrigger("Attack");
            }
            else
            {
                baseDamage = 15f;
                status = "H";
                anim.SetTrigger("Attack");
            }

            
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
                    playerHealth.TakeDamage(appliedDamage, status, transform.position);
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
                GameObject projectileObj = Instantiate(projectile, transform.position + Vector3.up * projectileHeight, transform.rotation);
                projectileObj.GetComponent<EnemyProjectile>().damage = appliedDamage;
            }
            else
            {
                RaycastHit hit;
                Vector3 origin = transform.position + Vector3.up;
                

                if(Physics.Raycast(origin, (hitscanTarget - origin).normalized, out hit, Mathf.Infinity, affectedLayers))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        playerHealth.TakeDamage(appliedDamage, "H", transform.position);
                        stutterTime = Time.time + .125f;
                    }
                }
            }
            
        }
    }


}
