using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarDefinition : MonoBehaviour
{
    public MeleeWeaponTrail EmitterStart;
    public MeleeWeaponTrail EmitterEnd;

    //Vivi specific:
    public Transform rightHandWeapon;
    public Transform leftHandWeapon;
    public GameObject muzzleFlash;

    public PlayerAttack playerAttack;

    private bool isShooting;

    private AudioSource audio;

    public AudioClip[] shotSounds;

    private void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    public void Swing()
    {
        playerAttack.Swing();
    }

    public void Melee(string property)
    {
        playerAttack.Melee(property, isShooting);

        if (isShooting)
        {
            Transform selectedHand = (playerAttack.activeHand ? rightHandWeapon : leftHandWeapon);
            Instantiate(muzzleFlash, selectedHand.position, selectedHand.rotation);

            audio.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);

        }


    }

    public void SwitchHands(string side)
    {
        playerAttack.activeHand = (side == "Right");
        isShooting = (side != "");
    }
}
