using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyWave
{
public List<GameObject> enemies;
}

public class CombatZoneController : MonoBehaviour
{
    public Transform cameraAnchor;

    private GameObject player;
    private CameraController camController;
    public List<GameObject> walls;
    public List<EnemyWave> waves;
    //public List<GameObject> enemies;
    public bool completed;
    public bool inCombat;
    public float combatTimer;
    private EnemyDirector enemyDirector;
    private Scoreboard scoreboard;
    private Animator AreaClearImage;
    private PlayerAttack playerAttack;

    private TimeManager timeManager;
    private bool encountered;
    public int currentWave;
    

    void Awake()
    {
        camController = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraController>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();
        AreaClearImage = GameObject.FindGameObjectWithTag("AreaClearImage").GetComponent<Animator>();
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

        /*for(int i=0; i<waves.Count; i++)
        {
             for (int j = 0; j < waves[i].enemies.Count; j++)
            {

            }
        }*/
    }

    void Update()
    {
        if(inCombat)
        {
            combatTimer += Time.deltaTime;

            if (playerAttack.spellWeaponVisibleTime < Time.time)
            {
                playerAttack.weaponVisibleTime = Time.time + 2f;
            }

            if ((enemyDirector.enemies.Count==0) && (currentWave < waves.Count))
            {
                currentWave++;
                SpawnWave(currentWave);
            }
        }

        if((combatTimer > 5f) && (currentWave>=waves.Count) && (enemyDirector.enemies.Count==0) && (completed == false))
        {
            EndBattle();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //camController.anchor = cameraAnchor;
            //camController.combatZone = this;
            if((completed==false) && (encountered == false))
            {
                encountered = true;
                BeginBattle();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (completed == true)
            {
                //camController.combatZone = null;
            }
            //camController.anchor = null;
        }
    }

    void BeginBattle()
    {
        

        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].SetActive(true);
        }

        SpawnWave(0);
        inCombat = true;

    }   

    void EndBattle()
    {
        inCombat = false;
        completed = true;
        AreaClearImage.SetTrigger("Activate");
        timeManager.DoSlowmotion();
        

        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].SetActive(false);
        }
    }

    void SpawnWave(int waveToSpawn)
    {
        if (waveToSpawn < waves.Count)
        {
            for (int j = 0; j < waves[waveToSpawn].enemies.Count; j++)
            {
                waves[waveToSpawn].enemies[j].SetActive(true);

            }
        }
    }

}
