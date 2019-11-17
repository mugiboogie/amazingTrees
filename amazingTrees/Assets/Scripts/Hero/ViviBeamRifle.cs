using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViviBeamRifle : MonoBehaviour
{
    private CapsuleCollider collider;
    private PlayerAttack playerAttack;
    private Animator bishoujoEyes;
    private AudioSource audioVoice;
    private AudioSource audioSound;
    private TimeManager timeManager;
    private Animator playerAnim;
    private float endTime;
    private EnemyDirector enemyDirector;
    [SerializeField] private List<EnemyHealth> enemyTargets;

    public LayerMask levelLayers;
    public Transform player;
    public AudioClip spellSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public GameObject particleHit;
    public GameObject particleEffect;
    public GameObject beamRifleLaser;
    public GameObject visual;
    private float animValue;

    private LineRenderer lineRenderer;

    void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        audioVoice = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroSound").GetComponent<AudioSource>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().anim;
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        lineRenderer = GetComponent<LineRenderer>();

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
            audioSound.PlayOneShot(spellSound, 1f);
            audioVoice.PlayOneShot(attackSound, 1f);
            bishoujoEyes.SetTrigger("Activate");
            timeManager.DoSlowmotion();
            playerAnim.SetTrigger("Spell");
            playerAttack.mana = 0f;
            endTime = Time.time + 5f;
            StartCoroutine(BeamRifle());

            
        }

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

        lineRenderer.SetPosition(0, transform.position + transform.forward * 1.25f + transform.up*.5f);
        lineRenderer.SetPosition(1, transform.position + transform.forward * collider.height + transform.up * .5f);

        lineRenderer.enabled = (Time.time < endTime);
        

        visual.SetActive(Time.time < endTime);
        animValue = Mathf.Lerp(animValue, Time.time < endTime ? 1f : 0f, 10f * Time.deltaTime);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("UsingSpell"), animValue);

        float flicker = Random.Range(.5f, .6f);
        lineRenderer.startWidth = animValue*flicker; lineRenderer.endWidth = animValue * flicker;
    }

    IEnumerator BeamRifle()
    {
        while (Time.time < endTime)
        {
            if (enemyTargets.Count > 0)
            {
                for (int i = 0; i < enemyTargets.Count; i++)
                {
                    EnemyHealth enemyHealth = enemyTargets[i].GetComponent<EnemyHealth>();

                    enemyHealth.TakeDamage(Random.Range(7f, 15f), "S", transform.position);
                }
            }
            playerAttack.durationTime = Time.time + .125f;
            playerAttack.spellWeaponVisibleTime = Time.time + .125f;
            playerAttack.weaponVisibleTime = 0f;
            yield return new WaitForSeconds(.0625f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyToAdd = other.GetComponent<EnemyHealth>();
            if (enemyToAdd != null)
            {
                if (!enemyTargets.Contains(enemyToAdd))
                {
                    enemyTargets.Add(enemyToAdd);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyToDelete = other.GetComponent<EnemyHealth>();
            if (enemyToDelete != null)
            {
                if (enemyTargets.Contains(enemyToDelete))
                {
                    enemyTargets.Remove(enemyToDelete);
                }
            }
        }
    }
}
