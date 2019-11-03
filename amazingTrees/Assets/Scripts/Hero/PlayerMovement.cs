using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    //References
    private PlayerController playerController;
    private Animator anim;
    private Rigidbody rb;
    public CharacterController charCon;
    private Transform camera;
    private PlayerTargetting playerTargetting;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private TimeManager timeManager;
    public GameObject dashTrail;
    public GameObject shockWave;
    private AudioSource audio;
    private float glitterCooldown;
    public GameObject glitterParticle;

    //Movement Parameters
    public float charSpeed = 5f; //Default speed the player traverses.
    public float turnSmoothing = 15f; //Default speed the player turns towards the direction they are moving to.
    public float gravity = 14f; //Default Gravity that is constantly applied to player.
    public float jumpForce = 10f; //Default Jump force that is applied when player jumps.
    private float verticalVelocity = 0f; //The value in which the jumpVector changes. (in Movement)
    private Quaternion targetRotation; //Rotation that the player aligns to when movement is applied. (in Turning)
    private bool dash; //True if the player is in a dash.
    public float dashTime; //The value of time in which the dash will end.
    [HideInInspector] public float dashDuration; //The end time of the player's dash.
    private float dashCooldown; //The cooldown for the time the player's next dash can begin.
    private bool canDash; //A check to ensure that the player can dash. It is only true when the player is grounded, so an airborne player must touch the ground before dashing again.
    private Vector3 dashDirection; //The direction of the dash. This is the inputDirection as it is on the frame the dash button was pushed.
    private float airborneTime;
    public AudioClip dashSound;
    private Vector3 cameraVector;
    PlayerControls controls;
    Vector2 move;
    private bool buttonJump;
    private bool buttonDash;

    public float stutterTime;

    [HideInInspector] public Vector3 hitPosition;

    private PlayerDecoyController playerDecoyController;

    public void SummonHero()
    {
        anim = playerController.anim;
        playerDecoyController = playerController.decoy.GetComponent<PlayerDecoyController>();
        charSpeed = playerController.hero.movementSpeed;
        dashTime = playerController.hero.dashTime;
        jumpForce = playerController.hero.jumpForce;
    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
        rb = GetComponent<Rigidbody>();
        charCon = GetComponent<CharacterController>();
        //camera = Camera.main.transform;
        camera = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        playerTargetting = GetComponent<PlayerTargetting>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        audio = GetComponent<AudioSource>();

        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.PlayerMove.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.PlayerMove.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.performed += ctx => StartCoroutine(inputJump());
        controls.Gameplay.Dash.performed += ctx => StartCoroutine(inputDash());


    }


    private void Update()
    {
        if (anim != null)
            
        {
            //Get Inputs
            //--Get Controller Input Values: Grabs the horizontal and vertical inputs from the player's input.
            float moveHorizontal = move.x;//Input.GetAxis("Horizontal");
            float moveVertical = move.y;//Input.GetAxis("Vertical");
            Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
            transform.Translate(m, Space.World);
            //--Get Camera Vectors: Takes the Vertical (Tilt) and Horizontal (Pan) vectors of the camera and multiplies them by the input values.
            Vector3 cameraVertical = (camera.up + camera.forward) * moveVertical;
            Vector3 cameraHorizontal = (camera.right) * moveHorizontal;

            //if (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(camera.transform.position.x, 0f, camera.transform.position.z)) > 2f)
            {
                cameraVector = cameraVertical + cameraHorizontal;
            }


            //--Calculate Input: Gets the final Vector for input. This will be the cameraVector's X and Z. It is normalized to prevent diagonals being faster than straights.
            Vector3 inputDirection = new Vector3(cameraVector.x, 0f, cameraVector.z).normalized * (new Vector2(moveHorizontal,moveVertical).magnitude);

            //Apply Movement
            Movement(inputDirection);
            if ((anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("Hit")) || (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
            {
                Vector3 hitDirection = hitPosition - transform.position;
                Turning(hitDirection);
            }
            else if ((!(anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Attack"))) && (!(anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack"))))
            {
                //If the player is not in an attack, face the direction the joystick is pulled.
                Turning(inputDirection);
            }
            else
            {
                //If the player is attacking, automatically face the lock-on target.
                if (playerTargetting.enemyTarget != null)
                {
                    Turning(playerTargetting.enemyTarget.transform.position - transform.position);
                }
                else
                {
                    Turning(inputDirection);
                }
            }

            //Apply Animation Parameters
            anim.SetFloat("Speed", inputDirection.magnitude, .05f, Time.deltaTime); //"Speed" in anim is 0=idle to 1=running. A dampening time was added to make the locomotion feel smoother.
            anim.SetBool("isGrounded", charCon.isGrounded); //"isGrounded" in anim is true=grounded or false=in air.
            anim.SetBool("Dashing", (dashDuration > Time.time));

            anim.enabled = (Time.time > stutterTime);

            bool playFootsteps = (charCon.isGrounded && (Time.time > dashDuration) && (inputDirection.magnitude > .1f) && (playerHealth.currentHealth>0f));
            anim.SetBool("PlayFootsteps", playFootsteps);
        }

        
    }

    IEnumerator inputJump()
    {
        buttonJump = true;
        yield return new WaitForEndOfFrame();
        buttonJump = false;
    }

    IEnumerator inputDash()
    {
        buttonDash = true;
        yield return new WaitForEndOfFrame();
        buttonDash = false;
    }

    void Movement(Vector3 inputDirection)
    {
        //This script controls both vertical, horizontal, and jump movement.
        //Vertical & Horizontal movement is determined by inputDirection.
        //Jump movement is determined by jumpDirection.

        //Get Jump Input. A jump can only be performed when the character controller is grounded AND the input of jump is given. Otherwise, begin descending.

        if ((!playerAttack.isCharging) && charCon.isGrounded && (buttonJump) && (!playerHealth.playerDead) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
        {
            //Character is jumping.
            verticalVelocity = jumpForce;

            playerAttack.AttackCancel();

            airborneTime += Time.deltaTime;
        }
        else
        {
            
            if (transform.position.y > 0) //This checks if the player is above the ground level (0). This additional check prevents the player from clipping into the ground. Therefore, the player cannot fall below any coordinates less than 0.
            {
                if (charCon.isGrounded)
                {
                    verticalVelocity -= gravity * Time.deltaTime;


                    //Cancel Attack if just landed
                    if (airborneTime > 0f)
                    {
                        playerAttack.AttackCancel();
                    }

                    airborneTime = 0f;
                }
                else
                {
                    //Character is in freefall.

                        verticalVelocity -= gravity * Time.deltaTime;

                }
            }
        }


        if ((!(charCon.isGrounded)) && ((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Attack")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack"))))
        {
            verticalVelocity = 2f;
            //jumpDirection.y =0f;
        }


        anim.SetBool("Falling", ((verticalVelocity < 0f)&& (!charCon.isGrounded)));

        //Once the verticalVelocity is determined, we can assign it to the jumpDirection. Note that X and Z are always 0.

        Vector3 jumpDirection = new Vector3(0, verticalVelocity, 0);


        //Get Dash Input. A dash can be performed when grounded or airborne. Whenever the player dashes, they lose the ability to dash. 
        //The ability is only replenished when the player's feet touch the floor.
        //--First check if we can replenish the player's dash ability.
        if (charCon.isGrounded)
        {
            canDash = true;
        }

        //--Then check all conditions if the player can perform a dash.
        //---The player must not already be in a dash
        //---The dash key is tapped
        //---The dash key is passed it's cooldown
        //---And the player has a dash ready
        if (((!playerAttack.isCharging)&&(dash==false)&&(buttonDash)) && (dashCooldown<Time.time) && (canDash==true) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("Hit")) && (anim.GetCurrentAnimatorStateInfo(3).tagHash != Animator.StringToHash("KnockUp")))
        {
            //dashDirection by default where the player is facing. However, this can be overwritten if the player is holding down a direction.

            dashDirection = transform.forward;

            if (inputDirection.magnitude > 0f)
            {
                dashDirection = inputDirection;
            }
            dashCooldown = Time.time + dashTime;
            dashDuration = Time.time + (dashTime/2f);
            dash = true;
            canDash = false;
            playerDecoyController.SetPosition();
            Instantiate(shockWave, transform.position + Vector3.up, transform.rotation);
            GameObject trailrender = Instantiate(dashTrail, transform.position, transform.rotation);
            trailrender.transform.SetParent(this.transform);
            trailrender.transform.localPosition = new Vector3(0f, 1f, 0f);
            Destroy(trailrender, 0.75f);

            playerAttack.AttackCancel();

            audio.PlayOneShot(dashSound, 1f);
        }

        if (Time.time < dashDuration)
        {
            if (Time.time > glitterCooldown)
            {
                glitterCooldown = Time.time + .1f;
                Instantiate(glitterParticle, transform.position + Vector3.up, transform.rotation);
            }
        }
        //--Once the player exhausts their dash duration, they stop dashing.
        if(dashDuration < Time.time)
        {
            dash = false;
        }
        
        //--Movement changes when the player is dashing.
        if(dash)
        {
            inputDirection = dashDirection * (2.5f * (Time.time / dashDuration)); //The last bit is just a ratio of the current time and the end time of the dash. This will make the dash feel like it's slowing down over time.
            jumpDirection.y -= 1f * Time.deltaTime; //Reduce the gravity to a glide when dashing.
        }


        if((playerHealth.playerDead)|| (anim.GetCurrentAnimatorStateInfo(3).tagHash == Animator.StringToHash("KnockUp")))
        {
            inputDirection = new Vector3(0f, 0f, 0f);
           
        }

        anim.applyRootMotion = (((anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("Attack")) || (anim.GetCurrentAnimatorStateInfo(1).tagHash == Animator.StringToHash("FinalAttack"))) || (anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Reactions")).tagHash == Animator.StringToHash("Hit")));

        if (anim.applyRootMotion == false)
        {
            playerController.avatar.transform.position = transform.position;
            playerController.avatar.transform.rotation = transform.rotation;
            charCon.enabled = true;
        }
        else
        {
            transform.position = anim.rootPosition;
            transform.rotation = anim.rootRotation;
            charCon.enabled = false;
        }
        

        if (((!anim.applyRootMotion) && (anim.GetBool("Charging")==false)))
        {
            charCon.Move((inputDirection * charSpeed * Time.deltaTime) + (jumpDirection * Time.deltaTime));
        }
        else if (((!anim.applyRootMotion) && (anim.GetBool("Charging") == true)))
        {
            charCon.Move(jumpDirection * Time.deltaTime);
        }
        else 
        {
            //charCon.Move(jumpDirection * Time.deltaTime);
        }

        //Flinch Negate
        anim.SetBool("FlinchNegate", false);


        if (Input.GetButtonDown("Jump"))
        {
            FlinchNegate();
        }
    }

    void Turning(Vector3 inputDirection)
    {
        //This script rotates the player using a lerp to face towards the direction they are moving in.

        //--First, determine if the player is actually moving. 
        //--This will prevent the player from automatically rotating when there are no inputs.
        if ((!playerHealth.playerDead))
        {
            if (inputDirection.magnitude > 0f)
            {
                //--Assign a "targetRotation," which is pointing towards the inputDirection along the Vector3.up axis.
                targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                //--Since we only need to pivot the rotation along the Y axis, we zero out X and Z.
                targetRotation.x = 0.0f;
                targetRotation.z = 0.0f;
                //--Then, update the rotation of the current object from it's current rotation to the targetRtation.
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
            }
        }
    }

    public void KnockUp(float force)
    {
        //This launches the player in the air. Command is called during PlayerHealth.
        verticalVelocity = force;
    }

    void FlinchNegate()
    {
        
            anim.SetTrigger("FlinchNegate");
           
    }


}
