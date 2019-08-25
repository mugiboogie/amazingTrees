using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablesChips : MonoBehaviour
{
    public GameObject[] chips;
    public float chipForce;
    public int chipsNum;

    public void EmitChips()
    {

        int probability = Mathf.RoundToInt(Random.Range(chipsNum * .5f, chipsNum * 1.5f));

        for (int i = 0; i < probability; i++)
        {
            GameObject chipSpawn = Instantiate(chips[Random.Range(0, chips.Length)], transform.position + Vector3.up, Quaternion.identity) as GameObject;
            Rigidbody rb = chipSpawn.GetComponent<Rigidbody>();
            if (rb != null) { rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)).normalized * (chipForce * Random.Range(.8f, 1.2f))); }
        }
    }
}
