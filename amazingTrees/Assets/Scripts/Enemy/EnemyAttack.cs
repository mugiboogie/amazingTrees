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
    private Vector3 hitscanTarget;
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

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        enemyController = GetComponent<EnemyController>();

    }

    void Update()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Taunt", false);

        if(Vector3.Distance(transform.position, player.position)<attackRange)
        {
            if((setAttack) && (Time.time>nextAttack))
            {
                nextAttack = Time.time + cooldownTime;
                if(ranged == true)
                {
                    //RAYCAST
                    //Set up origin of raycast and destination of raycast.
                    Vector3 origin = transform.position + Vector3.up;
                    Vector3 destination = player.position + Vector3.up;

                    //Store a RaycastHit
                    RaycastHit hit;
                    //Notation is: if(Physics.Raycast(Vector3 start, Vector3 direction, RaycastHit, Distance, LayerMask)
                    if(!Physics.Raycast(origin,(destination-origin).normalized,out hit,Vector3.Distance(origin,destination)-2f,affectedLayers))
                    {
                        //Logic
                        anim.SetTrigger("Taunt");
                        beginAttack = Time.time + tauntTime;
                    }
                    else
                    {
                        anim.SetTrigger("Taunt");
                        beginAttack = Time.time + tauntTime;
                    }
                    //RAYCAST
                }
                
                if((Time.time>beginAttack)&&(willAttack == true))
                {
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        float appliedDamage;

        appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);
        anim.SetTrigger("Attack");

        if(ranged == true)
        {
            if(hitscan == false)
            {
                GameObject projectile = Instantiate(projectile, transform.position, transform.rotation);
            }
            else
            {
                RaycastHit hit;
                Vector3 origin = transform.position + Vector3.up;
                Vector3 hitscanTarget = player.position + Vector3.up;

                Physics.Raycast(origin, (hitscanTarget - origin).normalized, out hit, Vector3.Distance(origin, hitscanTarget), affectedLayers);

            }
        }
        else
        {
            anim.SetTrigger("Melee");
        }
    }

    void Melee()
    {
        float appliedDamage = baseDamage + Random.Range(-damageVariance, damageVariance);



    }

}
