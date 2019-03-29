using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float attackRange;
    public float attackAngle;
    private Vector3 attackOrigin;
    public LayerMask affectedLayers;

    public float attackRate;
    private float cooldownTime;
    private float durationTime;
    private int meleeChain;
    private float stutterTime;
    private PlayerTargetting playerTargetting;
    private GameObject lastHitEnemy;

    private Animator anim;

    public CameraShake cameraShake;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        playerTargetting = GetComponent<PlayerTargetting>();
    }

    
    void FixedUpdate()
    {
        attackOrigin = transform.position + Vector3.up;

        
        if(Input.GetButtonDown("LightAttack") && (Time.time> durationTime) && (meleeChain<3))
        {
            cooldownTime = Time.time + attackRate * 3f;
            durationTime = Time.time + attackRate;
            
            
                meleeChain++;

            
        }
        if(Time.time>cooldownTime)
        {
            meleeChain = 0;
        }

        anim.SetInteger("MeleeChain", meleeChain);
        anim.SetBool("Melee", Time.time< cooldownTime);
        anim.applyRootMotion = (anim.GetCurrentAnimatorStateInfo(0).tagHash==Animator.StringToHash("Attack"));

        anim.enabled = (Time.time > stutterTime);


        Vector3 targetDir = lastHitEnemy.transform.position - transform.position;

        if ((Vector3.Distance(lastHitEnemy.transform.position, transform.position) > 3f) || (Vector3.Angle(targetDir, transform.forward) > 45f))
        {
            playerTargetting.overrideEnemy = null;
        }

    }

    public void Melee(float damage)
    {
        
        Debug.Log("Attack");
        Collider[] targets = Physics.OverlapSphere(attackOrigin, attackRange, affectedLayers);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 targetDir = targets[i].transform.position - attackOrigin;
            if(Vector3.Angle(targetDir,transform.forward)<attackAngle)
            {
                Debug.Log(targets[i]);

                if (targets[i].CompareTag("Enemy"))
                {
                    playerTargetting.overrideTime = Time.time + 2f;
                    lastHitEnemy = targets[i].gameObject;
                    playerTargetting.overrideEnemy = lastHitEnemy;
                    
                }

                targets[i].attachedRigidbody.AddForce(targetDir*damage*50f);
                StartCoroutine(cameraShake.Shake(.01f*damage,.005f*damage));
                stutterTime = Time.time + .125f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackOrigin, attackRange);
    }
}
