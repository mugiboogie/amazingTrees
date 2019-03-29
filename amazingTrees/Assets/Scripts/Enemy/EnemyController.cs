using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private Transform player;
    private NavMeshAgent nav;
    private Animator anim;
    public float desiredDistance; //Distance the enemy wants to stay away from the player.
    public float stopDistance;
    public float charSpeed;
    private float startMoving;

    private EnemyDirector enemyDirector;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();

        enemyDirector.AddEnemy(this.gameObject);

        nav.destination = FindRandomPoint();
    }

    void Update()
    {
        //Sets the current target to the player.
        //--Consider modifying when the enemy is not "Aggro"
        //nav.SetDestination(player.position);

        

        //Movement
        //By default, set the navigation speed to the charSpeed.
        if (Time.time > startMoving)
        {
            nav.speed = charSpeed;
        }

        //Autobraking is enabled.
        nav.autoBraking = true;

        //Calculate the remaining distance of the navigation path.
        float remainingDistance = Mathf.Clamp(nav.remainingDistance, 0f, 999f); //nav.remainingDistance will through infinity if the path requires a turn. Therefore, I clamped it to 999f.


        //Because of the infinity situation, we had to disable this function. We will revisit this idea later when we have an "Aggro" bool that triggers when the player is a certain distance away from the enemy.
        /*if (remainingDistance > 30)
        {
            nav.speed = 0f;
        }*/

        //Enemy stops when they are within the stopping distance.
        if (remainingDistance <= stopDistance)
        {
            nav.speed = 0f;
            if (Vector3.Distance(transform.position, player.transform.position) > desiredDistance)
            {
                nav.destination = FindRandomPoint();
            }
            startMoving = Time.time + Random.Range(2f,5f);
            
        }




        //Rotation
        //--The enemy is always looking at the player.
        Vector3 direction;

        if (Time.time < startMoving)
        {
            direction = player.position - this.transform.position;
        }
        else
        {
            direction = nav.steeringTarget - this.transform.position;
        }
        direction.y = 0;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        //Applies animation for locomotion. The parameter "Speed" is 0=idle and 1=running. A damp time was added to smooth out the animation.
        anim.SetFloat("Speed", Mathf.Clamp(nav.speed, 0f, 1f),.1f,Time.deltaTime);
    }

    private Vector3 FindRandomPoint()
    {
        float angle = Random.Range(0f, 360f);
        //And then let's do some trig... Remember SOHCAHTOA?

        float x = player.transform.position.x + desiredDistance * Mathf.Sin(angle);
        float z = player.transform.position.z + desiredDistance * Mathf.Cos(angle);

        return new Vector3(x, 0, z);

    }

}
