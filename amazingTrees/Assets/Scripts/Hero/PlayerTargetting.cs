using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetting : MonoBehaviour
{
    public GameObject enemyTarget;
    public RectTransform enemyTargetIndicator;
    public GameObject forwardEnemy;
    public GameObject closestEnemy;
    private EnemyDirector enemyDirector;
    private List<GameObject> enemies;
    public float overrideTime; //
    public GameObject overrideEnemy;
    public bool lockedOn;
    private Transform camera;

    private AudioSource audio;
    public AudioClip lockOnSound;
    public AudioClip lockOffSound;
    private bool lockOnPlayed;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        enemyTargetIndicator = GameObject.FindGameObjectWithTag("UI").transform.Find("EnemyTarget").GetComponent<RectTransform>();
        enemies = enemyDirector.enemies;
        camera = Camera.main.transform;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {

        

        if ((!Input.GetButton("LockOn")) || (enemyTarget == null))
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

            enemyTarget = findForwardEnemy();
            lockedOn = false;

            if (lockOnPlayed == true)
            {
                audio.PlayOneShot(lockOffSound);
                lockOnPlayed = false;
            }
        }

        else if(enemyTarget!=null)
        {
            lockedOn = true;

            if (lockOnPlayed == false)
            {
                audio.PlayOneShot(lockOnSound);
                lockOnPlayed = true;

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
    


}
