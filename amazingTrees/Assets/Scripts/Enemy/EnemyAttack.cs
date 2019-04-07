using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float baseDamage;
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
        anim.SetBool("Attack", false);

        if (setAttack)
        {
            enemyController.desiredDistance = attackRange;
        }
        else
        {
            enemyController.desiredDistance = passiveRange;
        }

        if ((setAttack==true)&& (Vector3.Distance(transform.position, player.position)<=(attackRange+1f))&&(anim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("Attack")) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("KnockUp")))
        {
            

            anim.SetTrigger("Attack");
            
        }


        anim.SetBool("AttackCancel", ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("KnockUp"))));


        anim.enabled = (Time.time > stutterTime);

      

        hitscanTarget = Vector3.Lerp(hitscanTarget, player.position+Vector3.up, 2.5f * Time.deltaTime);
        //Debug.DrawLine(transform.position + Vector3.up, hitscanTarget, Color.green);

    }

    void Attack()
    {
        float appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);

        setAttack = false;

        if(ranged==false)
        {
            //Melee Unit
            if (Vector3.Distance(transform.position, player.position) < 2f)
            {
                Vector3 targetDir = player.position - transform.position;
                if (Vector3.Angle(targetDir, transform.forward) < 45f)
                {
                    playerHealth.TakeDamage(appliedDamage, "H", transform.position);
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

        /*setAttack = false;
        willAttack = false;
        float appliedDamage;

        appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);
        anim.SetTrigger("Attack");

        if(ranged == true)
        {
            if(hitscan == false)
            {
                float projectileHeight = col.height / 2f;
                GameObject projectileObj = Instantiate(projectile, transform.position + Vector3.up*projectileHeight, transform.rotation);
                projectileObj.GetComponent<EnemyProjectile>().damage = appliedDamage;
            }
            else
            {
                //Revise Hitscan attack.
                RaycastHit hit;
                Vector3 origin = transform.position + Vector3.up;
                Vector3 hitscanTarget = player.position + Vector3.up;

                Physics.Raycast(origin, (hitscanTarget - origin).normalized, out hit, Vector3.Distance(origin, hitscanTarget), affectedLayers);

            }
        }
        else
        {
            anim.SetTrigger("Melee");
        }*/
    }


}
