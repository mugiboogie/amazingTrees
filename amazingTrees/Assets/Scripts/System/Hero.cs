using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Hero : ScriptableObject
{
    public string heroName;

    public Sprite heroPortraitImg;
    public Sprite heroNameImg;

    public float attackRange;
    public float attackAngle;
    public float lightAttackRate;
    public float heavyAttackRate;
    public float movementSpeed;
    public float dashTime;
    public float damageReduction;

    public AudioClip[] summonSfx;
    public AudioClip[] injuredSfx;
    public AudioClip deathSfx;
    public AudioClip chargeAttackVoice;

    public GameObject spell;
    public GameObject avatar;
    public GameObject decoy;
    public RuntimeAnimatorController animatorController;

    public GameObject bishoujoEyes;
}
