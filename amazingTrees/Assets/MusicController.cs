using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicS;
    public AudioSource musicM;
    public AudioSource musicL;

    private float fadeTime = .5f;

    private CombatZoneController[] combatZones;

    void Awake()
    {
        GameObject[] combatZonesObj = GameObject.FindGameObjectsWithTag("CombatZone");
        combatZones = new CombatZoneController[combatZonesObj.Length];
        for (int i = 0; i < combatZones.Length; i++)
        {
            combatZones[i] = combatZonesObj[i].GetComponent<CombatZoneController>();
        }
    }

    void Update()
    {
        int musicIntensity = 0;

        for (int i = 0; i < combatZones.Length; i++)
        {
            if (combatZones[i] == true)
            {
                musicIntensity = 1;
            }
        }

        for (int i = 0; i < combatZones.Length; i++)
        {
            if (combatZones[i].currentWave >= (combatZones[i].waves.Count - 1))
            {
                musicIntensity = 2;
            }
        }

        if (musicIntensity == 0)
        {
            musicS.volume = Mathf.Lerp(musicS.volume, 1f, fadeTime * Time.deltaTime);
            musicM.volume = Mathf.Lerp(musicM.volume, 0f, fadeTime * Time.deltaTime);
            musicL.volume = Mathf.Lerp(musicL.volume, 0f, fadeTime * Time.deltaTime);
        }

        else if (musicIntensity == 1)
        {
            musicS.volume = Mathf.Lerp(musicS.volume, 0f, fadeTime * Time.deltaTime);
            musicM.volume = Mathf.Lerp(musicM.volume, 1f, fadeTime * Time.deltaTime);
            musicL.volume = Mathf.Lerp(musicL.volume, 0f, fadeTime * Time.deltaTime);
        }

        else
        {
            musicS.volume = Mathf.Lerp(musicS.volume, 0f, fadeTime * Time.deltaTime);
            musicM.volume = Mathf.Lerp(musicM.volume, 0f, fadeTime * Time.deltaTime);
            musicL.volume = Mathf.Lerp(musicL.volume, 1f, fadeTime * Time.deltaTime);
        }
    }
}
