using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellMeter : MonoBehaviour
{
    private Image spellMeter;
    private Image spellIcon;
    private PlayerAttack playerAttack;

    private float spellRatio;

    void Awake()
    {
        spellMeter = transform.Find("SpellMeterFill").GetComponent<Image>();
        spellIcon = transform.Find("SpellIcon").GetComponent<Image>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    void Update()
    {
        float setRatio = playerAttack.mana / playerAttack.manaMax;

        spellRatio = Mathf.Lerp(spellRatio, setRatio, 10f * Time.deltaTime);

        spellMeter.fillAmount = spellRatio;

        spellIcon.enabled = (spellRatio >= 1f);

    }
}
