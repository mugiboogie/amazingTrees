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

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        enemyTargetIndicator = GameObject.FindGameObjectWithTag("UI").transform.Find("EnemyTarget").GetComponent<RectTransform>();
        enemies = enemyDirector.enemies;
    }

    void Update()
    {

        if (!Input.GetButton("LockOn"))
        {
            if ((Time.time > overrideTime)||(overrideEnemy == null))
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
            }
        }

        if (enemyTarget != null)
        {
            enemyTargetIndicator.position = Camera.main.WorldToScreenPoint(enemyTarget.transform.position + Vector3.up);
        }
    }


    public GameObject findForwardEnemy()
    {
        GameObject result = null;
        float angle = 180f;

        for (int i = 0; i < enemies.Count/2; i++)
        {
            Vector3 targetDir = enemies[i].transform.position - transform.position;
            if (Vector3.Angle(targetDir, transform.forward) < angle)
            {
                angle = Vector3.Angle(targetDir, transform.forward);
                result = enemies[i];
            }
        }

        return result;
    }
    


}
