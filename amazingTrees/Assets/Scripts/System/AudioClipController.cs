using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioClip[] hit;
    public AudioClip[] swing;
    public AudioClip[] hitSpecific;
    private AudioSource audio;
    private ParticleController particleController;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        AudioListener.volume = .25f;
        particleController = GameObject.FindGameObjectWithTag("ParticleController").GetComponent<ParticleController>();
    }

    public void PlayHit(Vector3 position, float damage)
    {
        AudioClip clip = hit[Random.Range(0, hit.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayHitSpecific(Vector3 position, AudioClip[] hitSFX)
    {
        AudioClip clip = hitSpecific[Random.Range(0, hit.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlaySwing(Vector3 position)
    {
        AudioClip clip = swing[Random.Range(0, swing.Length)];
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
