using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedObject
{
    public GameObject gameObject;
    public float weight;
}


public class EnemyDirector : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> enemies;
    public float aggroCooldown;
    private float nextAggro;

    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        nextAggro = Time.time + aggroCooldown;
    }

    void Update()
    {
        enemies.Sort((t1, t2) =>
        {
            float t1Distance = Vector3.Distance(player.transform.position, t1.transform.position);
            float t2Distance = Vector3.Distance(player.transform.position, t2.transform.position);

            if (t1Distance < t2Distance)
            {
                return -1;
            }

            if (t1Distance > t2Distance)
            {
                return 1;
            }

            return 0;
        });

        if (Time.time > nextAggro)
        {
            nextAggro = Time.time + aggroCooldown;
            if (enemies.Count > 0)
            {
                int enemyIndex = Random.Range(0, enemies.Count);
                AggroEnemy(enemies[enemyIndex]);
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

    }

    void AggroEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyAttack>().setAttack = true;
    }

}
