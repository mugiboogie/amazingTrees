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

    //Movement Parameters
    public float charSpeed = 5f; //Default speed the player traverses.
    public float turnSmoothing = 15f; //Default speed the player turns towards the direction they are moving to.
    public float gravity = 14f; //Default Gravity that is constantly applied to player.
    public float jumpForce = 10f; //Default Jump force that is applied when player jumps.
    private float verticalVelocity = 0f; //The value in which the jumpVector changes. (in Movement)
    private Quaternion targetRotation; //Rotation that the player aligns to when movement is applied. (in Turning)

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        charCon = GetComponent<CharacterController>();
        camera = Camera.main.transform;
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
        Turning(inputDirection);

        //Apply Animation Parameters
        anim.SetFloat("Speed", inputDirection.magnitude, .05f, Time.deltaTime); //"Speed" in anim is 0=idle to 1=running. A dampening time was added to make the locomotion feel smoother.
        anim.SetBool("isGrounded", charCon.isGrounded); //"isGrounded" in anim is true=grounded or false=in air.
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

        charCon.Move((inputDirection * charSpeed * Time.deltaTime) + (jumpDirection * Time.deltaTime));
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
