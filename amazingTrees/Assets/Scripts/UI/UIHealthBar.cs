using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Image healthMeter;
    private Image healthMeterBurn;
    private PlayerHealth playerHealth;
    private float burnRatio;
    [HideInInspector] public float burnTime;

    void Awake()
    {
        healthMeter = transform.Find("HealthMeterFill").GetComponent<Image>();
        healthMeterBurn = transform.Find("HealthMeterBurn").GetComponent<Image>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        float healthRatio = playerHealth.currentHealth / playerHealth.maxHealth;
        healthMeter.fillAmount = healthRatio;

        if (Time.time > burnTime)
        {
            burnRatio = Mathf.Lerp(burnRatio, healthRatio, 5f * Time.deltaTime);
            healthMeterBurn.fillAmount = burnRatio;
        }
    }


}
