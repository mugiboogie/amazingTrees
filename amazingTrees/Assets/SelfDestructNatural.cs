using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructNatural : MonoBehaviour
{
    public float selfdestruct_in = 5; // Setting this to 0 means no selfdestruct.

    void Start()
    {
        selfdestruct_in = selfdestruct_in * (Random.Range(.8f, 1.2f));
        if (selfdestruct_in != 0)
        {
            Destroy(gameObject, selfdestruct_in);
        }
    }
}
