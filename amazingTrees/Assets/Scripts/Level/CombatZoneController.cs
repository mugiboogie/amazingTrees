using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZoneController : MonoBehaviour
{
    public Transform cameraAnchor;

    private GameObject player;
    private CameraController camController;
    public List<GameObject> walls;
    public List<GameObject> enemies;
    public bool completed;
    private bool inCombat;
    public float combatTimer;
    private EnemyDirector enemyDirector;
    private Scoreboard scoreboard;
    private Animator AreaClearImage;
    private PlayerAttack playerAttack;

    private TimeManager timeManager;
    private bool encountered;
    

    void Awake()
    {
        camController = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraController>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();
        AreaClearImage = GameObject.FindGameObjectWithTag("AreaClearImage").GetComponent<Animator>();
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if(inCombat)
        {
            combatTimer += Time.deltaTime;
            playerAttack.weaponVisibleTime = Time.time + 2f;
        }

        if((combatTimer > 5f) && (enemyDirector.enemies.Count==0) && (completed == false))
        {
            EndBattle();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            camController.anchor = cameraAnchor;
            camController.combatZone = this;
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
                camController.combatZone = null;
            }
            camController.anchor = null;
        }
    }

    void BeginBattle()
    {
        inCombat = true;

        for(int i=0; i<walls.Count; i++)
        {
            walls[i].SetActive(true);
        }

        for (int j = 0; j < enemies.Count; j++)
        {
                enemies[j].SetActive(true);
            
        }
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

}
