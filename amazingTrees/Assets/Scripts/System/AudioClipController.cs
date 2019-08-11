using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioClip[] hit;
    public AudioClip[] swing;
    private AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        AudioListener.volume = .25f;
    }

    public void PlayHit(Vector3 position)
    {
        AudioClip clip = hit[Random.Range(0, hit.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayHitSpecific(Vector3 position, AudioClip[] hitSFX)
    {
        AudioClip clip = hitSFX[Random.Range(0, hitSFX.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlaySwing(Vector3 position)
    {
        AudioClip clip = swing[Random.Range(0, swing.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
