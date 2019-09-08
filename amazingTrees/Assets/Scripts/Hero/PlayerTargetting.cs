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
    private transform camera;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        enemyTargetIndicator = GameObject.FindGameObjectWithTag("UI").transform.Find("EnemyTarget").GetComponent<RectTransform>();
        enemies = enemyDirector.enemies;
        camera = Camera.main.transform;
    }

    void Update()
    {

        enemyTarget = findForwardEnemy();

        if ((!Input.GetButton("LockOn")) && (enemyTarget != null))
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

            lockedOn = false;
        }

        else
        {
            lockedOn = true;
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
            Vector3 targetDir = enemies[i].transform.position - camera.position;
            if (Vector3.Angle(targetDir, camera.forward) < angle)
            {
                angle = Vector3.Angle(targetDir, camera.forward);
                result = enemies[i];
            }
        }

        return result;
    }
    


}
