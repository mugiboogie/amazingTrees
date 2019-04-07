using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecoyController : MonoBehaviour
{

    private GameObject player;
    private Transform[] decoyBones;
    private Transform[] playerBones;

    public float visibility;

    public Color visibleColor;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        decoyBones = GetComponentInChildren<SkinnedMeshRenderer>().bones;
        playerBones = player.GetComponentInChildren<SkinnedMeshRenderer>().bones;
    }

    void Update()
    {
        visibility = Mathf.Lerp(visibility, 0f, 5f * Time.deltaTime);

        visibleColor.a = visibility;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            for (int i = 0; i < r.materials.Length; i++)
            {
                r.materials[i].color = visibleColor;
            }
        }
    }

    public void SetPosition()
    {
        for (int i = 0; i < decoyBones.Length; i++)
        {
            decoyBones[i].SetPositionAndRotation(playerBones[i].position, playerBones[i].rotation);
        }
        visibility = 1f;
    }
}
