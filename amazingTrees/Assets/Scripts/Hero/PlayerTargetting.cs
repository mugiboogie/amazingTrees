using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerTargetting : MonoBehaviour
{
    public GameObject enemyTarget;
    public RectTransform enemyTargetIndicator;
    public GameObject forwardEnemy;
    public GameObject forwardEnemyFromPlayer;
    public GameObject closestEnemy;
    private EnemyDirector enemyDirector;
    private List<GameObject> enemies;
    public float overrideTime; //
    public GameObject overrideEnemy;
    public bool lockedOn;
    private Transform camera;

    private AudioSource audioVoice;
    private AudioSource audioSound;
    public AudioClip lockOnSound;
    public AudioClip lockOffSound;
    private bool lockOnPlayed;

    private List<GameObject> sortedEnemies;
    PlayerControls controls;
    private bool buttonLockOn;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        enemyTargetIndicator = GameObject.FindGameObjectWithTag("UI").transform.Find("EnemyTarget").GetComponent<RectTransform>();
        enemies = enemyDirector.enemies;
        camera = Camera.main.transform;
        audioVoice = transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = transform.Find("HeroSound").GetComponent<AudioSource>();
        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.LockOn.performed += ctx => StartCoroutine(inputLockOn());
    }

    IEnumerator inputLockOn()
    {
        buttonLockOn = true;
        yield return new WaitForEndOfFrame();
        buttonLockOn = false;
    }

    void Update()
    {

        forwardEnemyFromPlayer = findForwardEnemyFromPlayer();

        if (buttonLockOn && (Time.time > Time.deltaTime) || (enemyTarget == null))
        {
            /*if ((Time.time > overrideTime)||(overrideEnemy == null))
            {
                enemyTarget = findForwardEnemy();
            }
            else if(overrideEnemy!=null)
            {
                enemyTarget = overrideEnemy;
            }

            if (enemyTarget != null)
            {
                if (Vector3.Distance(transform.position, enemyTarget.transform.position) > 10f)
                {
                    enemyTarget = null;
                }

            }*/
            sortedEnemies = enemies.OrderBy(x => Vector3.Angle((new Vector3(x.transform.position.x, 0f, x.transform.position.z) - new Vector3(camera.position.x, 0f, camera.position.z)), camera.forward)).ToList();

            //enemyTarget = findForwardEnemy();
            enemyTarget = (sortedEnemies.Count>0?sortedEnemies[0]:null);

            lockedOn = false;

            if (lockOnPlayed == true)
            {
                audioSound.PlayOneShot(lockOffSound);
                lockOnPlayed = false;
            }
        }

        else if(enemyTarget!=null)
        {
            lockedOn = true;

            if (lockOnPlayed == false)
            {
                audioSound.PlayOneShot(lockOnSound);
                lockOnPlayed = true;

            }

            if(enemyTarget.GetComponent<EnemyHealth>().currentHealth<=0f)
            {
                lockedOn = false;
                lockOnPlayed = false;
                enemyTarget = null;
            }


        }

        else
        {
            lockedOn = false;
        }

        if (enemyTarget != null)
        {
            enemyTargetIndicator.position = Camera.main.WorldToScreenPoint(enemyTarget.transform.position + Vector3.up);
        }
        else
        {
            enemyTargetIndicator.position = new Vector3(0f, 0f, 0f);
        }
    }


    public GameObject findForwardEnemy()
    {
        GameObject result = null;
        float angle = 180f;

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 enemyPosition = enemies[i].transform.position;
            enemyPosition.y = 0f;
            Vector3 cameraPosition = camera.position;
            cameraPosition.y = 0f;
            Vector3 targetDir = enemyPosition - cameraPosition;
            if (Vector3.Angle(targetDir, camera.forward) < angle)
            {
                angle = Vector3.Angle(targetDir, camera.forward);
                result = enemies[i];
            }
        }
        

        return result;
    }

    private void sortEnemies()
    {
        
        
    }
    public GameObject findForwardEnemyFromPlayer()
    {
        GameObject result = null;
        float angle = 180f;

        for (int i=0; i< enemies.Count; i++)
        {
            Vector3 enemyPosition = enemies[i].transform.position;
            enemyPosition.y = 0f;
            Vector3 playerPosition = transform.position;
            playerPosition.y = 0f;
            Vector3 targetDir = enemyPosition - playerPosition;
            if (Vector3.Angle(targetDir, transform.forward) < angle)
            {
                angle = Vector3.Angle(targetDir, transform.forward);
                result = enemies[i];
            }

        }
        return result;
    }


}
