using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public float slowdownLength = .0125f;


    void Update()
    {
       

        Time.timeScale += (1f / slowdownLength)*Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);


        
    }


    public void DoSlowmotion()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = slowdownFactor;
        }
        else
        {
            Time.timeScale = 1f;
        }
        Time.fixedDeltaTime = Time.timeScale * .02f;

    }
}
