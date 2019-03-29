using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> enemies;
    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;

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
    }

    public void AddEnemy(GameObject enemy)
    {
        if(!enemies.Contains(enemy))
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

}
