using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerAttack playerAttack;
    float dissolveValue;
    Renderer renderer;
    Light light;
    float lightValue;
    bool playedOn;
    bool playedOff = true;
    public AudioClip weaponOn;
    public AudioClip weaponOff;
    private AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        renderer = GetComponent<Renderer>();
        light = GetComponent<Light>();
    }

    void Update()
    {
        float setDissolve = (Time.time > playerAttack.weaponVisibleTime) ? 0f : 1f;
        dissolveValue = Mathf.Lerp(dissolveValue, setDissolve, 10f * Time.deltaTime);

        renderer.material.SetFloat("_Progress", dissolveValue);

        if ((Time.time > playerAttack.weaponVisibleTime) && (!playedOff))
        {
            if (audio != null) { audio.PlayOneShot(weaponOff); }

            playedOff = true;
            playedOn = false;
            lightValue = 1f;
        }
        else if ((Time.time < playerAttack.weaponVisibleTime) && (!playedOn))
        {
            if (audio != null) { audio.PlayOneShot(weaponOn); }
            
            playedOff = false;
            playedOn = true;
            lightValue = 1f;
        }

        lightValue -= 2f*Time.deltaTime;
        lightValue = Mathf.Max(0f, lightValue);

        if (light != null) { light.intensity = lightValue; }

        renderer.enabled = dissolveValue > .0125f;
    }

}
