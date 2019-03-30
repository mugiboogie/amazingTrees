﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float attackRange;
    public float attackAngle;
    private Vector3 attackOrigin;
    public LayerMask affectedLayers;

    public float lightAttackRate;
    public float heavyAttackRate;
    public float cooldownTime;
    private float durationTime;
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

        anim.SetBool("LightAttack", false);
        anim.SetBool("HeavyAttack", false);


        if (Input.GetButtonDown("LightAttack") && (Time.time> durationTime) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("FinalAttack")))
        {
            cooldownTime = Time.time + lightAttackRate + .25f;
            durationTime = Time.time + lightAttackRate;

            anim.SetTrigger("LightAttack");
        }

        if (Input.GetButtonDown("HeavyAttack") && (Time.time > durationTime) && (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("FinalAttack")))
        {
            cooldownTime = Time.time + heavyAttackRate + .5f;
            durationTime = Time.time + heavyAttackRate;

            anim.SetTrigger("HeavyAttack");
        }


        anim.SetBool("Melee", Time.time< cooldownTime);
        anim.applyRootMotion = ((anim.GetCurrentAnimatorStateInfo(1).tagHash==Animator.StringToHash("Attack"))|| (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack")));

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

                StartCoroutine(cameraShake.Shake(.1f,.005f*damage));
                stutterTime = Time.time + .125f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackOrigin, attackRange);
    }

    public void AttackCancel()
    {
        cooldownTime = Time.time;
        durationTime = Time.time;
        anim.SetBool("Melee", false);
        anim.SetBool("LightAttack", false);
        anim.SetBool("HeavyAttack", false);

    }
}