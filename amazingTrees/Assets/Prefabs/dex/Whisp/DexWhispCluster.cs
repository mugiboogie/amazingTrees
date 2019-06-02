using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexWhispCluster : MonoBehaviour
{
    public DexWhispBullet[] whispBullets;
    private EnemyDirector enemyDirector;
    private PlayerAttack playerAttack;
    private Animator playerAnim;

    void Awake()
    {
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyDirector.enemies.Count > 0)
        {
            AssignTargets();
        }

        if((Input.GetButtonDown("SpellAttack"))&&(playerAttack.mana>=playerAttack.manaMax))
        {
            playerAnim.SetTrigger("Spell");
            playerAttack.mana = 0f;
            BlastWhisp();
        }
    }

    void AssignTargets()
    {
        int j = 0;
        for(int i=0; i<whispBullets.Length; i++)
        {
            if(j>enemyDirector.enemies.Count-1)
            {
                j = 0;
            }

            if ((whispBullets[i].target == null) || ((whispBullets[i].target!=null)&&(!enemyDirector.enemies.Contains(whispBullets[i].target.gameObject))))
            {
                whispBullets[i].target = enemyDirector.enemies[j].transform;
            }
            j++;
        }
    }

    void BlastWhisp()
    {
        for (int i = 0; i < whispBullets.Length; i++)
        {
            whispBullets[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < whispBullets.Length; i++)
        {
            whispBullets[i].gameObject.SetActive(true);
        }
    }
}
