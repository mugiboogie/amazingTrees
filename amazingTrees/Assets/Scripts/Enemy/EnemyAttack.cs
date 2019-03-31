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
                    
                }
            }
        }
    }

}
