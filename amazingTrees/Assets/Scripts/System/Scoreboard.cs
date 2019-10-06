using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    
    public List<CombatZoneController> combatZones;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;
    private Text text;
    public CombatZoneController combatZone;
    private string defaultText;

    public GameObject prize;

    void Awake()
    {
        text = GetComponent<Text>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        combatZone = GetComponent<CombatZoneController>();
        defaultText = "Press F to swap heroes.";
    }
    void Update()
    {
        string result = "Congratulations \n";
        
        for(int i=0; i<combatZones.Count; i++)
        {
            result = result + "Zone " + (i + 1).ToString() + ": " + (combatZones[i].combatTimer).ToString("F2") +" seconds"+"\n";
        }

        result = result + "Damage Taken: " + playerHealth.damageTaken.ToString("F2") + "\n";
        result = result + "Damage Dealt: " + playerAttack.damageDealt.ToString("F2") + "\n";

        if(Input.GetKeyDown(KeyCode.F))
        {
            defaultText = "";
        }

        text.text = defaultText;

        bool success = true;

        for(int j=0; j<combatZones.Count; j++)
        {
            if(!combatZones[j].completed)
            {
                success = false;
            }
        }

        if(success == true)
        {
            text.text = result;
            prize.SetActive(true);
        }

    }

}
