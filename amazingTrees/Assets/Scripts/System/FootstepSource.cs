using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSource : MonoBehaviour
{
    //This script should be attached to any humanoid that can produce footsteps and has their locomotion tied to an animator layer called "Base Locomotion".
    //It essentially grabs the progress of the animation clip and then plays a foostep sound (and can be expaneded by creating footstep particles, such as water splashes and footprints)
    //This script is dependent of the object's Animator and Audio Source.
    //To ensure this is synched properly, all right feet must land on 0%, and left feet must land on 50%.
    //Finally, the animatorBool "PlayFoosteps" must be true in order for this to run.
    //Each individual gameObject should give specific instructions on when this is enabled and when this isn't.
    //For instance, the hero will not play footsteps when sliding or rolling.
    //Currently this only plays one set of footsteps. Consider expanding to change the sound based on the material they are stepping on.

    private Animator anim;

    private bool rightFootCanPlay; //When true, the script waits for the right foot to play and plays it. This is disabled immediately when the right foot plays, but will become available when the left foot is about to land.
    private bool leftFootCanPlay; //When true, the script waits for the left foot to play and plays it. This is disabled immediately when the left foot plays, but will become available when the right foot is about to land.

    public AudioClip clip; //The sound effect that plays when the foot lands.
    private AudioSource audio; //Reference to this gameObject's audioSource component. Remember that the hero's AudioSource should be set to 2D so you're not hearing footsteps ping left and right in your headphones.

    private float footstepCooldown; //the Cooldown timer to ensure that the footsteps don't play immediately one after another.

    void Awake()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        //Get the progress percentage that the animation has gone through so far.
        //Note that we're only pulling from the animator layer "Base Locomotion".
        float animationFrame = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Locomotion")).normalizedTime % 1;
        bool playFoosteps = anim.GetBool("PlayFootsteps");

        //the Animator's "PlayFootsteps" should be enabled for this to operate properly.
        if (playFoosteps)
        {
            if ((animationFrame > 0f) && (animationFrame <= .5f))
            {
                //Play right foot at 0%.
                leftFootCanPlay = true;
                if (rightFootCanPlay && (Time.time > footstepCooldown))
                {
                    rightFootCanPlay = false;
                    audio.PlayOneShot(clip, 1f);
                    footstepCooldown = Time.time + .125f;
                }
            }

            if ((animationFrame > .5f) && (animationFrame <= 1f))
            {
                //Play left foot at 50%.
                rightFootCanPlay = true;
                if (leftFootCanPlay && (Time.time > footstepCooldown))
                {
                    leftFootCanPlay = false;
                    audio.PlayOneShot(clip, 1f);
                    footstepCooldown = Time.time + .125f;
                }
            }
        }
        else
        {
            //Reset both values to default when the hero cannot play footstep sounds.
            leftFootCanPlay = true;
            rightFootCanPlay = true;
        }

    }


}

