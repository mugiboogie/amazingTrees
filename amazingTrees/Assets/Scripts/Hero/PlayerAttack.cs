using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;

    
    public float attackRange;
    public float attackAngle;
    private Vector3 attackOrigin;
    public LayerMask affectedLayers;

    public float lightAttackRate;
    public float heavyAttackRate;
    public float cooldownTime;
    private float durationTime;
    private PlayerTargetting playerTargetting;
    private GameObject lastHitEnemy;
    public float lightAttackChargeTime;
    public float heavyAttackChargeTime;
    private float lightAttackCharge;
    private float heavyAttackCharge;
    private AudioClipController audioClipController;
    private AudioSource audio;
    public float mana;
    public float manaMax;
    public float manaRegenTime;
    private bool attackReloadAfterAirborne;
    

    public float comboChain;
    public float comboChainReset;

    private Animator anim;

    private CameraController cameraController;
    public CameraShake cameraShake;
    private PlayerMovement playerMovement;

    public MeleeWeaponTrail trail1;
    public MeleeWeaponTrail trail2;

    [HideInInspector] public float damageDealt;

    private EnemyDirector enemyDirector;

    private Animator comboCounter;
    private Text comboNumber;
    private Text comboComment;

    public bool isCharging;
    private Animator chargeEffect;
    private bool chargeCompletePlayed;
    private bool chargeInitiatePlayed;
    private float chargePulseTime;
    public GameObject chargeCompleteEffect;
    public AudioClip chargeComplete;
    public AudioClip charging;
    public AudioClip chargingPulse;
    public AudioClip chargeAttackVoice;
    public AudioClip chargeAttackFoley;
    private float chargeAttackDuration;
    private float glitterCooldown;
    public GameObject glitterParticle;
    public GameObject shockWave;

    public bool activeHand; //For Vivi's guns. True = righthanded, False = lefthanded.

    public float weaponVisibleTime;
    private float weaponVisibleWeight;

    [HideInInspector] public Animator HUDparent;

    public LayerMask levelLayers;

    public void SummonHero()
    {
        anim = playerController.anim;
        chargeAttackVoice = playerController.hero.chargeAttackVoice;
        attackRange = playerController.hero.attackRange;
        attackAngle = playerController.hero.attackAngle;
        lightAttackRate = playerController.hero.lightAttackRate;
        heavyAttackRate = playerController.hero.heavyAttackRate;
        trail1 = playerController.avatar.GetComponent<PlayerAvatarDefinition>().EmitterStart;
        trail2 = playerController.avatar.GetComponent<PlayerAvatarDefinition>().EmitterEnd;
    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
        //anim.runtimeAnimatorController = AC;

        cameraShake = Camera.main.GetComponent<CameraShake>();
        playerTargetting = GetComponent<PlayerTargetting>();
        cameraController = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraController>();
        playerMovement = GetComponent<PlayerMovement>();
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();

        comboCounter = GameObject.FindGameObjectWithTag("ComboCounter").GetComponent<Animator>();
        comboNumber = GameObject.FindGameObjectWithTag("ComboCounter").transform.Find("ComboNumber").GetComponent<Text>();
        comboComment = GameObject.FindGameObjectWithTag("ComboCounter").transform.Find("ComboComment").GetComponent<Text>();
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
        audio = GetComponent<AudioSource>();

        chargeEffect = GameObject.FindGameObjectWithTag("PlayerEffects/ChargeEffect").GetComponent<Animator>();
        chargeEffect.transform.SetParent(transform, false);

        HUDparent = GameObject.FindGameObjectWithTag("HUDParent").GetComponent<Animator>();
    }

    
    void Update()
    {
        if (anim != null)
        {
            comboCounter.SetBool("ShowCombo", Time.time < comboChainReset);
            if (comboChain > 0)
            {
                comboNumber.text = comboChain + " Hits";

                if (comboChain > 100) { comboComment.text = "KWEEEEN!!!"; }
                else if ((comboChain > 90) && (comboChain <= 100)) { comboComment.text = "R.I.P.!!"; }
                else if ((comboChain > 80) && (comboChain <= 90)) { comboComment.text = "Wicked!!"; }
                else if ((comboChain > 69) && (comboChain <= 80)) { comboComment.text = "Freaky!!"; }
                else if (comboChain == 69) { comboComment.text = "nice."; }
                else if ((comboChain > 60) && (comboChain <= 68)) { comboComment.text = "OMG!!"; }
                else if ((comboChain > 50) && (comboChain <= 60)) { comboComment.text = "Killer!!"; }
                else if ((comboChain > 40) && (comboChain <= 50)) { comboComment.text = "Witchin'!!"; }
                else if ((comboChain > 30) && (comboChain <= 40)) { comboComment.text = "Slay!!"; }
                else if ((comboChain > 20) && (comboChain <= 30)) { comboComment.text = "Yaass!"; }
                else if ((comboChain > 10) && (comboChain <= 20)) { comboComment.text = "Beautiful!"; }
                else { comboComment.text = "Caca Girl!"; }
            }


            if ((Time.time > manaRegenTime) && (enemyDirector.enemies.Count > 0))
            {
                AddSpAttack(25f * Time.deltaTime);
            }

            bool emitTrail = ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Attack")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack")));

            if (trail1 != null)
            {
                trail1.Emit = emitTrail;
            }
            if (trail2 != null)
            {
                trail2.Emit = emitTrail;
            }

            CameraFOV();

            if (Time.time > comboChainReset)
            {
                comboChain = 0f;
            }

            attackOrigin = transform.position + Vector3.up;

            anim.SetBool("LightAttack", false);
            anim.SetBool("HeavyAttack", false);
            anim.SetBool("ChLightAttack", false);
            anim.SetBool("ChHeavyAttack", false);

            if (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack") && playerMovement.charCon.isGrounded == false)
            {
                attackReloadAfterAirborne = false;
            }
            if (playerMovement.charCon.isGrounded == true)
            {
                attackReloadAfterAirborne = true;
            }



            if (Input.GetButtonDown("LightAttack") && (Time.time > durationTime))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                weaponVisibleTime = Time.time + 2f;

                if ((anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("FinalAttack")))
                {
                    cooldownTime = Time.time + lightAttackRate + .25f;
                    durationTime = Time.time + lightAttackRate + .0625f;
                    anim.SetTrigger("LightAttack");
                }
                
                
            }

            if (Input.GetButtonDown("HeavyAttack") && (Time.time > durationTime))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                weaponVisibleTime = Time.time + 2f;

                if (anim.GetCurrentAnimatorStateInfo(1).tagHash != Animator.StringToHash("FinalAttack"))
                {
                    cooldownTime = Time.time + heavyAttackRate + .5f;
                    durationTime = Time.time + heavyAttackRate + .125f;
                    anim.SetTrigger("HeavyAttack");
                }
                
            }




            //Light Attack Charge
            if ((!Input.GetButton("LightAttack")) && (lightAttackCharge >= lightAttackChargeTime) && (playerMovement.charCon.isGrounded))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                //else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                //Unleash attack
                lightAttackCharge = 0f;

                cooldownTime = Time.time + lightAttackRate + .5f;
                durationTime = Time.time + lightAttackRate;
                weaponVisibleTime = Time.time + 2f;

                audio.PlayOneShot(chargeAttackVoice);
                audio.PlayOneShot(chargeAttackFoley);
                chargeAttackDuration = Time.time + .5f;
                Instantiate(shockWave, transform.position + Vector3.up, transform.rotation);
                anim.SetTrigger("ChLightAttack");
            }
            if (Input.GetButton("LightAttack") && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                //else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                weaponVisibleTime = Time.time + 2f;
                lightAttackCharge += Time.deltaTime;
            }
            else
            {
                lightAttackCharge = 0f;
            }



            //Heavy Attack Charge
            if ((!Input.GetButton("HeavyAttack")) && (heavyAttackCharge >= heavyAttackChargeTime) && (playerMovement.charCon.isGrounded))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                //else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                //Unleash attack
                heavyAttackCharge = 0f;

                cooldownTime = Time.time + heavyAttackRate + .5f;
                durationTime = Time.time + heavyAttackRate;
                weaponVisibleTime = Time.time + 2f;

                audio.PlayOneShot(chargeAttackVoice);
                audio.PlayOneShot(chargeAttackFoley);
                chargeAttackDuration = Time.time + .5f;
                Instantiate(shockWave, transform.position + Vector3.up, transform.rotation);
                anim.SetTrigger("ChHeavyAttack");
            }
            if (Input.GetButton("HeavyAttack") && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
            {
                if (playerTargetting.lockedOn) { transform.rotation = lookAtTarget(playerTargetting.enemyTarget.transform); }
                //else { if (playerTargetting.forwardEnemyFromPlayer != null) { transform.rotation = lookAtTarget(playerTargetting.forwardEnemyFromPlayer.transform); } }
                weaponVisibleTime = Time.time + 2f;
                heavyAttackCharge += Time.deltaTime;
            }
            else
            {
                heavyAttackCharge = 0f;
            }

            float chargeValue = Mathf.Max(lightAttackCharge / lightAttackChargeTime, heavyAttackCharge / heavyAttackChargeTime);
            chargeEffect.SetFloat("ChargeValue", chargeValue);

            isCharging = chargeValue > 0f;

            if (chargeValue > .25f)
            {
                if (chargeInitiatePlayed == false)
                {
                    //audio.PlayOneShot(charging);
                    chargeInitiatePlayed = true;
                }
                if (chargeValue > .9f)
                {
                    if (chargeCompletePlayed == false)
                    {
                        Instantiate(chargeCompleteEffect, transform.position, transform.rotation);
                        chargeCompletePlayed = true;
                        audio.PlayOneShot(chargeComplete);
                        chargePulseTime = Time.time + 1f;
                    }
                    if (Time.time > chargePulseTime)
                    {
                        Instantiate(chargeCompleteEffect, transform.position, transform.rotation);
                        chargePulseTime = Time.time + 1f;
                        audio.PlayOneShot(chargingPulse);
                    }
                }
            }
            else
            {
                chargeInitiatePlayed = false;
                chargeCompletePlayed = false;
            }

            if (Time.time < chargeAttackDuration)
            {
                if (Time.time > glitterCooldown)
                {
                    glitterCooldown = Time.time + .25f;
                    Instantiate(glitterParticle, transform.position, transform.rotation);
                }
            }


            anim.SetBool("Melee", Time.time < cooldownTime);
            anim.SetBool("Charging", (heavyAttackCharge > heavyAttackChargeTime / 2f) || (lightAttackCharge > lightAttackChargeTime / 2f));
            if (anim.GetBool("Charging"))
            {
                AttackCancel();
            }

            if ((anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
            {
                AttackCancel();
            }

            if(Time.time > weaponVisibleTime)
            {
                weaponVisibleWeight = Mathf.Lerp(weaponVisibleWeight, 1f, 10f * Time.deltaTime);
                
            }

            else
            {
                weaponVisibleWeight = 0f;
            }

            anim.SetLayerWeight(anim.GetLayerIndex("Unarmed"), weaponVisibleWeight);

            if (lastHitEnemy != null)
            {
                Vector3 targetDir = lastHitEnemy.transform.position - transform.position;

                if ((Vector3.Distance(lastHitEnemy.transform.position, transform.position) > 3f) || (Vector3.Angle(targetDir, transform.forward) > 45f))
                {
                    playerTargetting.overrideEnemy = null;
                }
            }
        }
    }

    public void Melee(string property, bool isShooting)
    {

        string[] propertyArray = property.Split(char.Parse("/"));
        float damage = float.Parse(propertyArray[0]);
        string effect = propertyArray[1];


        //Debug.Log("Attack");

        Collider[] targets; 
        if(isShooting)
        {

            float radius = 3f;
            Vector3 origin = transform.position + Vector3.up*(radius) - transform.forward*(radius);
            
            Vector3 targetDir = transform.forward;
            float defaultDistance = 9999f;
                
            RaycastHit[] targetHits = Physics.SphereCastAll(origin, radius, targetDir, defaultDistance);
            targets = new Collider[targetHits.Length];
            for(int i=0; i<targets.Length; i++)
            {
                targets[i] = targetHits[i].collider;
            }
        }
        else
        {
            targets = Physics.OverlapSphere(attackOrigin, attackRange, affectedLayers);
        }
        

            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 targetDir = (targets[i].transform.position + Vector3.up) - attackOrigin;
                if (Vector3.Angle(targetDir, transform.forward) < attackAngle)
                {
                    //Debug.Log(targets[i]);

                    if (targets[i].CompareTag("Enemy"))
                    {
                        EnemyHealth enemyHealth = targets[i].GetComponent<EnemyHealth>();

                        if (!enemyHealth.isDead)
                        {
                            playerTargetting.overrideTime = Time.time + 2f;
                            lastHitEnemy = targets[i].gameObject;
                            playerTargetting.overrideEnemy = lastHitEnemy;

                            comboChain++;
                            comboChainReset = Time.time + 3f;
                        }

                        enemyHealth.TakeDamage(damage, effect, transform.position);

                    }

                    if (targets[i].CompareTag("Destructables"))
                    {
                        DestructablesHealth destructablesHealth = targets[i].GetComponent<DestructablesHealth>();

                        if (!destructablesHealth.isDead)
                        {
                            playerTargetting.overrideTime = Time.time + 2f;
                            lastHitEnemy = targets[i].gameObject;
                            playerTargetting.overrideEnemy = lastHitEnemy;

                            comboChain++;
                            comboChainReset = Time.time + 3f;
                        }

                        destructablesHealth.TakeDamage(damage, effect, transform.position);

                    }
                    
                    if (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack"))
                    {
                        //StartCoroutine(cameraShake.Shake(.1f, .005f * damage));
                        StartCoroutine(cameraShake.Shake(.1f, .25f));
                    }
                    else
                    {
                        StartCoroutine(cameraShake.Shake(.1f, .00625f));
                    }
                    playerMovement.stutterTime = Time.time + Mathf.Min(.25f, (lightAttackRate * 1.5f));
                }
                damageDealt += damage;
            }
            StartCoroutine(ApplyForce(effect, damage, targets, isShooting));
        
       

        
    }

    IEnumerator ApplyForce(string effect, float damage, Collider[] targets, bool isShooting)
    {
        yield return new WaitForSeconds(.0625f);
        float impact = 50f;
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].attachedRigidbody != null)
                {
                    Vector3 targetDir = (targets[i].transform.position + Vector3.up) - attackOrigin;
                    if (isShooting)
                    {
                        targets[i].attachedRigidbody.AddForce(transform.forward * damage * impact);
                    }
                    else
                    {
                        if (Vector3.Angle(targetDir, transform.forward) < attackAngle)
                        {
                            switch (effect)
                            {
                                case "H":
                                    targets[i].attachedRigidbody.AddExplosionForce(damage * impact, transform.position + Vector3.up, attackRange * 2f);
                                    break;
                                case "S":
                                    targets[i].attachedRigidbody.AddExplosionForce(damage * impact, transform.position + Vector3.up, attackRange * 2f);
                                    break;
                                case "U":
                                    targets[i].attachedRigidbody.AddExplosionForce(damage * (impact * .5f), targets[i].transform.position + Vector3.down, attackRange * 2f);
                                    //targets[i].attachedRigidbody.AddForce(Vector3.up * damage * (impact * .5f));
                                    break;
                                case "D":
                                    targets[i].attachedRigidbody.AddForce(Vector3.down * damage * impact);
                                    break;
                                case "B":
                                    targets[i].attachedRigidbody.AddForce(transform.forward * damage * (impact * 2f) + Vector3.up);
                                    break;
                            }
                        }
                    }
                }
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
        if (anim != null)
        {
            cooldownTime = Time.time;
            durationTime = Time.time;
            //lightAttackCharge = 0f;
            //heavyAttackCharge = 0f;

            anim.SetBool("Melee", false);
            anim.SetBool("LightAttack", false);
            anim.SetBool("HeavyAttack", false);
            anim.SetBool("ChLightAttack", false);
            anim.SetBool("ChHeavyAttack", false);
        }
    }

    void CameraFOV()
    {
        if (anim != null)
        {
            cameraController.desiredFOV = 60f;
            if (anim.GetBool("Charging"))
            {
                float chargeValue = Mathf.Min(1f, Mathf.Max(heavyAttackCharge, lightAttackCharge));
                cameraController.desiredFOV = 60f - (30f * chargeValue * chargeValue);
                //StartCoroutine(cameraShake.Shake(.1f, .0125f));
                cameraShake.ShakeConstant(.0125f);
            }
            else if (comboChain > 2)
            {
                cameraController.desiredFOV = 45f;
            }
        }
    }

    public void AddSpAttack(float value)
    {
        mana += value;
    }

    public void Swing()
    {
        audioClipController.PlaySwing(transform.position);
    }

    private Quaternion lookAtTarget(Transform target)
    {
        Quaternion result = transform.rotation;
        Vector3 targetPosition = target.position;
        targetPosition.y = 0f;

        Vector3 fromPosition = transform.position;
        fromPosition.y = 0f;

        Vector3 targetDir = targetPosition - fromPosition;
        float currentX = -Vector3.SignedAngle(targetDir, Vector3.forward, Vector3.up);

        result = Quaternion.Euler(0, currentX, 0);

        return result;
    }


}
