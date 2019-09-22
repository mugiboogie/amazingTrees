using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockOnCursor : MonoBehaviour
{
    private Animator anim;
    private PlayerTargetting playerTargetting;

    void Awake()
    {
        anim = transform.Find("Graphic").GetComponent<Animator>();
        playerTargetting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTargetting>();
    }

    void Update()
    {
        anim.SetBool("LockedOn", playerTargetting.lockedOn);
        if(playerTargetting.lockedOn)
        {
            transform.position = playerTargetting.enemyTarget.transform.position;
        }

        /*Vector3 playerPosition = playerTargetting.transform.position;
        playerPosition.y = 0f;
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0f;
        Vector3 targetDir = playerPosition - currentPosition;

        transform.rotation = Quaternion.LookRotation(targetDir);*/
    }

}
