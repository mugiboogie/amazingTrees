using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject[] WeakHit;
    public GameObject[] MedHit;
    public GameObject[] HeavyHit;

    public GameObject[] enemyWeakHit;
    public GameObject[] enemyMedHit;
    public GameObject[] enemyHeavyHit;

    public GameObject[] destructableHit;

    public void CreateParticle(Vector3 position, float damage)
    {
        GameObject setParticle;
        if (damage < 10f)
        {
            setParticle = WeakHit[Random.Range(0, WeakHit.Length)];
        }
        else if ((damage >= 10f) || (damage < 20f))
        {
            setParticle = MedHit[Random.Range(0, MedHit.Length)];
        }
        else
        {
            setParticle = HeavyHit[Random.Range(0, HeavyHit.Length)];
        }
        Instantiate(setParticle, position, Quaternion.identity);
    }

    public void CreateEnemyParticle(Vector3 position, float damage)
    {
        GameObject setParticle;
        if (damage < 10f)
        {
            setParticle = enemyWeakHit[Random.Range(0, enemyWeakHit.Length)];
        }
        else if ((damage >= 10f) || (damage < 20f))
        {
            setParticle = enemyMedHit[Random.Range(0, enemyMedHit.Length)];
        }
        else
        {
            setParticle = enemyHeavyHit[Random.Range(0, enemyHeavyHit.Length)];
        }
        Instantiate(setParticle, position, Quaternion.identity);
    }

    public void CreateDestructableParticle(Vector3 position)
    {
        GameObject setParticle = destructableHit[Random.Range(0, destructableHit.Length)];
       
        GameObject iParticle = Instantiate(setParticle, position, Quaternion.identity);
        iParticle.transform.localScale = new Vector3(.25f, .25f, .25f);
    }
}
