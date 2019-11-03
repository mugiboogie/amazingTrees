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

    private AudioSource audioVoice;
    private AudioSource audioSound;

    public AudioClip[] shotSounds;

    private void Awake()
    {
        audioVoice = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = GameObject.FindGameObjectWithTag("Player").transform.Find("HeroSound").GetComponent<AudioSource>();
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

            audioSound.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);

        }


    }

    public void SwitchHands(string side)
    {
        playerAttack.activeHand = (side == "Right");
        isShooting = (side != "");
    }
}
