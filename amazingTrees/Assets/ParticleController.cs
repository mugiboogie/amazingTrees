using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject[] weakHit;
    public GameObject[] medHit;
    public GameObject[] heavyHit;

    public void CreateParticle(Vector3 position, float damage)
    {
        GameObject setParticle;
        if (damage < 10f)
        {
            setParticle = weakHit[Random.Range(0, weakHit.Length)];
        }
        else if ((damage >= 10f) || (damage < 20f))
        {
            setParticle = medHit[Random.Range(0, medHit.Length)];
        }
        else
        {
            setParticle = heavyHit[Random.Range(0, heavyHit.Length)];
        }
        Instantiate(setParticle, position, Quaternion.identity);
    }
}
