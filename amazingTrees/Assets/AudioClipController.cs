using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioClip[] hit;
    private AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayHit(Vector3 position)
    {
        AudioClip clip = hit[Random.Range(0, hit.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
