using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private PlayerTargetting playerTargetting;
    private GameObject lastHitEnemy;
    public float lightAttackChargeTime;
    public float heavyAttackChargeTime;
    private float lightAttackCharge;
    private float heavyAttackCharge;
    private AudioClipController audioClipController;
    public float mana;
    public float manaMax;
    public float manaRegenTime;
    

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


    void Awake()
    {
        anim = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        playerTargetting = GetComponent<PlayerTargetting>();
        cameraController = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraController>();
        playerMovement = GetComponent<PlayerMovement>();
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();

        comboCounter = GameObject.FindGameObjectWithTag("ComboCounter").GetComponent<Animator>();
        comboNumber = GameObject.FindGameObjectWithTag("ComboCounter").transform.Find("ComboNumber").GetComponent<Text>();
        comboComment = GameObject.FindGameObjectWithTag("ComboCounter").transform.Find("ComboComment").GetComponent<Text>();
        audioClipController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioClipController>();
    }

    
    void Update()
    {

        comboCounter.SetBool("ShowCombo", Time.time < comboChainReset);
        if (comboChain > 0)
        {
            comboNumber.text = comboChain + " Hits";

            if(comboChain>100) { comboComment.text = "KWEEEEN!!!"; }
            else if ((comboChain > 90) && (comboChain <= 100)) { comboComment.text = "R.I.P.!!"; }
            else if ((comboChain > 80) && (comboChain <= 90)) { comboComment.text = "Wicked!!"; }
            else if ((comboChain > 69) && (comboChain <= 80)) { comboComment.text = "Freaky!!"; }
            else if (comboChain == 69) { comboComment.text = "nice."; }
            else if ((comboChain > 60) && (comboChain <= 68)) { comboComment.text = "OMG!!"; }
            else if ((comboChain > 50) && (comboChain <= 60)) { comboComment.text = "Killer!!"; }
            else if ((comboChain > 40) && (comboChain <=50)) { comboComment.text = "Witchin'!!"; }
            else if ((comboChain > 30) && (comboChain <= 40)) { comboComment.text = "Slay!!"; }
            else if ((comboChain > 20) && (comboChain <= 30)) { comboComment.text = "Yaass!"; }
            else if ((comboChain > 10) && (comboChain <= 20)) { comboComment.text = "Beautiful!"; }
            else { comboComment.text = "Cool!"; }
        }


        if((Time.time>manaRegenTime) && (enemyDirector.enemies.Count>0))
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

        if(Time.time>comboChainReset)
        {
            comboChain = 0f;
        }

        attackOrigin = transform.position + Vector3.up;

        anim.SetBool("LightAttack", false);
        anim.SetBool("HeavyAttack", false);
        anim.SetBool("ChLightAttack", false);
        anim.SetBool("ChHeavyAttack", false);


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

        //Light Attack Charge
        if ((!Input.GetButton("LightAttack")) && (lightAttackCharge >= lightAttackChargeTime))
        {
            //Unleash attack
            lightAttackCharge = 0f;

            cooldownTime = Time.time + lightAttackRate + .5f;
            durationTime = Time.time + lightAttackRate;

            anim.SetTrigger("ChLightAttack");
        }
        if (Input.GetButton("LightAttack")&& (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit"))&& (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
        {
            lightAttackCharge += Time.deltaTime;
        }
        else
        {
            lightAttackCharge = 0f;
        }



        //Heavy Attack Charge
        if ((!Input.GetButton("HeavyAttack")) && (heavyAttackCharge >= heavyAttackChargeTime))
        {
            //Unleash attack
            heavyAttackCharge = 0f;

            cooldownTime = Time.time + heavyAttackRate + .5f;
            durationTime = Time.time + heavyAttackRate;

            anim.SetTrigger("ChHeavyAttack");
        }
        if (Input.GetButton("HeavyAttack") && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
        {
            heavyAttackCharge += Time.deltaTime;
        }
        else
        {
            heavyAttackCharge = 0f;
        }


        anim.SetBool("Melee", Time.time< cooldownTime);
        anim.SetBool("Charging", (heavyAttackCharge> heavyAttackChargeTime/2f) ||(lightAttackCharge> lightAttackChargeTime / 2f));
        if(anim.GetBool("Charging"))
        {
            AttackCancel();
        }

        if ((anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("Hit"))|| (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
        {
            AttackCancel();
        }




            if (lastHitEnemy != null)
        {
            Vector3 targetDir = lastHitEnemy.transform.position - transform.position;

            if ((Vector3.Distance(lastHitEnemy.transform.position, transform.position) > 3f) || (Vector3.Angle(targetDir, transform.forward) > 45f))
            {
                playerTargetting.overrideEnemy = null;
            }
        }

    }

    public void Melee(string property)
    {

        string[] propertyArray = property.Split(char.Parse("/"));
        float damage = float.Parse(propertyArray[0]);
        string effect = propertyArray[1];


        //Debug.Log("Attack");
        Collider[] targets = Physics.OverlapSphere(attackOrigin, attackRange, affectedLayers);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 targetDir = targets[i].transform.position - attackOrigin;
            if(Vector3.Angle(targetDir,transform.forward)<attackAngle)
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

                    enemyHealth.TakeDamage(damage,effect,transform.position);
                    
                }

                

                targets[i].attachedRigidbody.AddForce(targetDir*damage*50f);

                
                if (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack"))
                {
                    //StartCoroutine(cameraShake.Shake(.1f, .005f * damage));
                    StartCoroutine(cameraShake.Shake(.1f, .25f));
                }
                else
                {
                    StartCoroutine(cameraShake.Shake(.1f, .00625f));
                }
                playerMovement.stutterTime = Time.time + .25f;
            }
            damageDealt += damage;
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
        //lightAttackCharge = 0f;
        //heavyAttackCharge = 0f;

        anim.SetBool("Melee", false);
        anim.SetBool("LightAttack", false);
        anim.SetBool("HeavyAttack", false);
        anim.SetBool("ChLightAttack", false);
        anim.SetBool("ChHeavyAttack", false);

    }

    void CameraFOV()
    {
        cameraController.desiredFOV = 60f;
        if (anim.GetBool("Charging"))
        {
            float chargeValue = Mathf.Min(1f,Mathf.Max(heavyAttackCharge, lightAttackCharge));
            cameraController.desiredFOV = 60f-(30f*chargeValue * chargeValue);
            StartCoroutine(cameraShake.Shake(.1f, .125f*chargeValue*chargeValue));
        }
        else if(comboChain>2)
        {
            cameraController.desiredFOV = 40f;
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
}
