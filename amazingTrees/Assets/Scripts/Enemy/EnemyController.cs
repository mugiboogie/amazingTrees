using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    Transform player;
    NavMeshAgent nav;
    Animator anim;
    public float stopDistance;
    public float charSpeed;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Sets the current target to the player.
        //--Consider modifying when the enemy is not "Aggro"
        nav.SetDestination(player.position);

        //Rotation
        //--The enemy is always looking at the player.
        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        //Movement
        //By default, set the navigation speed to the charSpeed.
        nav.speed = charSpeed;

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
        }

        //Applies animation for locomotion. The parameter "Speed" is 0=idle and 1=running. A damp time was added to smooth out the animation.
        anim.SetFloat("Speed", Mathf.Clamp(nav.speed, 0f, 1f),.1f,Time.deltaTime);
    }

}
