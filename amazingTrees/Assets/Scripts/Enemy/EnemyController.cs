using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public LayerMask levelLayers;
    public bool isGrounded;
    private Transform player;
    private NavMeshAgent nav;
    private Animator anim;
    public float desiredDistance; //Distance the enemy wants to stay away from the player.
    public float stopDistance;
    public float charSpeed;
    private float startMoving;
    private EnemyHealth enemyHealth;
    public float dropFrequency;
    public List<WeightedObject> drops;
    private GameObject drop;

    private EnemyDirector enemyDirector;

    private float assignRandomTimer;
    [HideInInspector] public Vector3 damageOrigin;
    public float knockUpVelocity;
    public float gravity = 14f;
    private Rigidbody rb;
    private CapsuleCollider col;
    private BoxCollider combatZone;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyDirector = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyDirector>();
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        combatZone = transform.parent.GetComponent<BoxCollider>();
        float margin = 2f;
        minX = combatZone.transform.position.x - ((combatZone.size.x/2f) - margin);
        maxX = combatZone.transform.position.x + ((combatZone.size.x/2f) - margin);
        minZ = combatZone.transform.position.z - ((combatZone.size.z/2f) - margin);
        maxZ = combatZone.transform.position.z + ((combatZone.size.z/2f) - margin);

        enemyDirector.AddEnemy(this.gameObject);
        

        nav.destination = FindRandomPoint();
    }

    void FixedUpdate()
    {
        isGrounded = checkGrounded();

        if (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("KnockUp"))
        {
            Vector3 knockUpVector = new Vector3(0f, knockUpVelocity * Time.deltaTime, 0f);
            
            if ((isGrounded) && (knockUpVelocity <= 0f)) { knockUpVector = Vector3.zero; }
            float position = transform.position.y + (knockUpVector.y);
            //position.y += 1f*Time.deltaTime;
            transform.position = new Vector3(transform.position.x, position, transform.position.z);

        }
    }

    void Update()
    {
        if ((!enemyHealth.isDead))
            {

            if ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("KnockBack")))
            {
                assignRandomTimer = 0f;
                nav.updatePosition = false;
                nav.updateRotation = false;
                nav.nextPosition = transform.position;
                rb.isKinematic = false;
                faceDamageOrigin();
            }

            else if ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("KnockUp")))
            {
                assignRandomTimer = 0f;
                faceDamageOrigin();
                nav.updatePosition = false;
                nav.updateRotation = false;
                nav.nextPosition = anim.rootPosition;
                rb.isKinematic = true;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Attack"))
            {
                
                assignRandomTimer = 0f;
                lookAt();
                isAttacking();
                rb.isKinematic = true;
            }

            
            else
            {
                nav.updatePosition = true;
                nav.updateRotation = true;
                rb.isKinematic = true;

                if (assignRandomTimer == 0f)
                {
                    nav.destination = FindRandomPoint();
                    
                }
                assignRandomTimer += Time.deltaTime;
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
                    startMoving = Time.time + Random.Range(2f, 5f);

                }

                Debug.DrawLine(transform.position, nav.destination, Color.red);



                lookAt();

                //Applies animation for locomotion. The parameter "Speed" is 0=idle and 1=running. A damp time was added to smooth out the animation.
                anim.SetFloat("Speed", Mathf.Clamp(nav.speed, 0f, 1f), .1f, Time.deltaTime);

                


            }
    
        }
        else
        {
            isDead();
        }
        //bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (col.bounds.extents.y) + 0.1f);
        //Debug.DrawLine(transform.position, (transform.position + Vector3.down) * ((col.bounds.extents.y) + 0.1f));
        
        
        anim.applyRootMotion = ((anim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Attack"))|| (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Hit")));
        anim.SetBool("isGrounded", isGrounded);

        anim.SetBool("Falling", (((knockUpVelocity <= 0f) && (!isGrounded))||isGrounded));
        knockUpVelocity -= gravity * Time.deltaTime;

    }

    private Vector3 FindRandomPoint()
    {
        float angle = Random.Range(0f, 360f);
        //And then let's do some trig... Remember SOHCAHTOA?

        
        float x = player.transform.position.x + desiredDistance * Mathf.Sin(angle);
        float z = player.transform.position.z + desiredDistance * Mathf.Cos(angle);
        x = Mathf.Clamp(x, minX, maxX);
        z = Mathf.Clamp(z, minZ, maxZ);

        

        return new Vector3(x, 0, z);
        

        


    }

    void isDead()
    {
        nav.speed = 0f;
        nav.destination = transform.position;
        enemyDirector.RemoveEnemy(this.gameObject);
        StartCoroutine(DestroyEnemy());
        

    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);//destroy the dead enemy after 5 secondsz

        if (Random.Range(0f, .99f) < dropFrequency)
        {
            GameObject gameObjectToCreate = selectWeightedObject();
            Instantiate(gameObjectToCreate, transform.position, transform.rotation);
        }

        //Instantiate(drop, transform.position, drop.transform.rotation);
        //Destroy(this.gameObject);
    }

    void isAttacking()
    {
        //nav.speed = 0f;
        //nav.destination = transform.position;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.nextPosition = anim.rootPosition;
        startMoving = Time.time + Random.Range(.125f, .125f);

    }

    void lookAt()
    {
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
        
    }

    void faceDamageOrigin()
    {
        Vector3 direction = damageOrigin - this.transform.position;
        direction.y = 0;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.25f);
    }

    void OnAnimatorMove()
    {
        

        if ((anim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Attack")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Hit")))
        {
            //Debug.Log("AnimatorMove");
            Vector3 position = anim.rootPosition;
            position.y = nav.nextPosition.y;
            transform.position = position;
            transform.rotation = anim.rootRotation;
        }

    }

    private bool checkGrounded()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + Vector3.up * (col.height);

        Debug.DrawLine(p1, p1 + Vector3.down * (col.height+1f), Color.green);
        //if(Physics.SphereCast(p1, col.radius, Vector3.down, out hit, col.height))
        if(Physics.Raycast(transform.position + Vector3.up*(col.height),Vector3.down, out hit, col.height + .125f,levelLayers))
        {
                return true;   
        }
        return false;
    }


    private GameObject selectWeightedObject()
    {
        GameObject selected = null;
        float maxChoice = SumOfWeights;
        float randChoice = Random.Range(0, maxChoice);
        float weightSum = 0;

        foreach (WeightedObject weightedObject in drops)
        {
            weightSum += weightedObject.weight;
            if (randChoice <= weightSum)
            {
                selected = weightedObject.gameObject;
                break;
            }
        }

        return selected;
    }

    private float SumOfWeights
    {
        get
        {
            float sumOfWeights = 0;
            foreach (WeightedObject weightedObject in drops)
            {
                sumOfWeights += weightedObject.weight;
            }

            return sumOfWeights;
        }
    }


}
