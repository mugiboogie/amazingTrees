using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //References
    private Animator anim;
    private Rigidbody rb;
    private CharacterController charCon;
    private Transform camera;
    private PlayerTargetting playerTargetting;

    //Movement Parameters
    public float charSpeed = 5f; //Default speed the player traverses.
    public float turnSmoothing = 15f; //Default speed the player turns towards the direction they are moving to.
    public float gravity = 14f; //Default Gravity that is constantly applied to player.
    public float jumpForce = 10f; //Default Jump force that is applied when player jumps.
    private float verticalVelocity = 0f; //The value in which the jumpVector changes. (in Movement)
    private Quaternion targetRotation; //Rotation that the player aligns to when movement is applied. (in Turning)
    private bool dash; //True if the player is in a dash.
    private float dashDuration; //The end time of the player's dash.
    private float dashCooldown; //The cooldown for the time the player's next dash can begin.
    private bool canDash; //A check to ensure that the player can dash. It is only true when the player is grounded, so an airborne player must touch the ground before dashing again.
    private Vector3 dashDirection; //The direction of the dash. This is the inputDirection as it is on the frame the dash button was pushed.
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        charCon = GetComponent<CharacterController>();
        camera = Camera.main.transform;
        playerTargetting = GetComponent<PlayerTargetting>();
    }


    private void FixedUpdate()
    {
        //Get Inputs
        //--Get Controller Input Values: Grabs the horizontal and vertical inputs from the player's input.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //--Get Camera Vectors: Takes the Vertical (Tilt) and Horizontal (Pan) vectors of the camera and multiplies them by the input values.
        Vector3 cameraVertical = (camera.up + camera.forward)*moveVertical;
        Vector3 cameraHorizontal = (camera.right)*moveHorizontal;
        Vector3 cameraVector = cameraVertical + cameraHorizontal;
        //--Calculate Input: Gets the final Vector for input. This will be the cameraVector's X and Z. It is normalized to prevent diagonals being faster than straights.
        Vector3 inputDirection = new Vector3(cameraVector.x, 0f, cameraVector.z).normalized;

        //Apply Movement
        Movement(inputDirection);
        if (!(anim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Attack")))
        {
            //If the player is not in an attack, face the direction the joystick is pulled.
            Turning(inputDirection);
        }
        else
        {
            //If the player is attacking, automatically face the lock-on target.
            Turning(playerTargetting.enemyTarget.transform.position- transform.position);
        }

        //Apply Animation Parameters
        anim.SetFloat("Speed", inputDirection.magnitude, .05f, Time.deltaTime); //"Speed" in anim is 0=idle to 1=running. A dampening time was added to make the locomotion feel smoother.
        anim.SetBool("isGrounded", charCon.isGrounded); //"isGrounded" in anim is true=grounded or false=in air.
        anim.SetBool("Dashing", dash);

       
        
    }

    void Movement(Vector3 inputDirection)
    {
        //This script controls both vertical, horizontal, and jump movement.
        //Vertical & Horizontal movement is determined by inputDirection.
        //Jump movement is determined by jumpDirection.

        //Get Jump Input. A jump can only be performed when the character controller is grounded AND the input of jump is given. Otherwise, begin descending.

        if (charCon.isGrounded && (Input.GetButtonDown("Jump")))
        {
            //Character is jumping.
            verticalVelocity = jumpForce;
        }
        else
        {
            
            if (transform.position.y > 0) //This checks if the player is above the ground level (0). This additional check prevents the player from clipping into the ground. Therefore, the player cannot fall below any coordinates less than 0.
            {
                if (charCon.isGrounded)
                {
                    verticalVelocity = -gravity * Time.deltaTime;
                }
                else
                {
                    //Character is in freefall.
                    verticalVelocity -= gravity * Time.deltaTime;
                }
            }
        }
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
        if ((dash==false)&&(Input.GetButtonDown("Dash"))&&(dashCooldown<Time.time) && (canDash==true))
        {
            //dashDirection by default where the player is facing. However, this can be overwritten if the player is holding down a direction.
            dashDirection = transform.forward;

            if (inputDirection.magnitude > 0f)
            {
                dashDirection = inputDirection;
            }
            dashCooldown = Time.time + .425f;
            dashDuration = Time.time + .325f;
            dash = true;
            canDash = false;
        }

        //--Once the player exhausts their dash duration, they stop dashing.
        if(dashDuration<Time.time)
        {
            dash = false;
        }
        
        //--Movement changes when the player is dashing.
        if(dash)
        {
            inputDirection = dashDirection * (2.5f * (Time.time / dashDuration)); //The last bit is just a ratio of the current time and the end time of the dash. This will make the dash feel like it's slowing down over time.
            jumpDirection.y -= 1f * Time.deltaTime; //Reduce the gravity to a glide when dashing.
        }


        if (!anim.applyRootMotion)
        {
            charCon.Move((inputDirection * charSpeed * Time.deltaTime) + (jumpDirection * Time.deltaTime));
        }
        else
        {
            charCon.Move(jumpDirection * Time.deltaTime);
        }
    }

    void Turning(Vector3 inputDirection)
    {
        //This script rotates the player using a lerp to face towards the direction they are moving in.

        //--First, determine if the player is actually moving. 
        //--This will prevent the player from automatically rotating when there are no inputs.
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
