using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioClip[] hit;
    private AudioSource audio;

    void Awake()
    {
        audio = GetComponenet<AudioSource>();
    }

    public void PlayHit(Vector3 position)
        AudioClip clip = ControllerColliderHit[Random.Range(0,hit.length)];
        AudioSource.PlayClipAtPoint(clip,Position);
}
