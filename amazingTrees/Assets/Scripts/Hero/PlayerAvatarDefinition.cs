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

    public void Swing()
    {
        playerAttack.Swing();
    }

    public void Melee(string property)
    {
        playerAttack.Melee(property);

        Transform selectedHand = (playerAttack.activeHand?rightHandWeapon:leftHandWeapon);
        Instantiate(muzzleFlash, selectedHand.position, selectedHand.rotation);
    }

    public void SwitchHands(string side)
    {
        playerAttack.activeHand = (side == "Right");
    }
}
