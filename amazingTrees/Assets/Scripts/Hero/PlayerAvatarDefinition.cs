using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarDefinition : MonoBehaviour
{
    public MeleeWeaponTrail EmitterStart;
    public MeleeWeaponTrail EmitterEnd;
    

    public PlayerAttack playerAttack;

    public void Swing()
    {
        playerAttack.Swing();
    }

    public void Melee(string property)
    {
        playerAttack.Melee(property);
    }
}
