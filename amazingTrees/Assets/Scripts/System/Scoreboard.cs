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

    public GameObject prize;

    void Awake()
    {
        text = GetComponent<Text>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        combatZone = GetComponent<CombatZoneController>();
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

        text.text = "";

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

    void AreaClear()
    {
        string aC = "Area Cleared! \n";

        if (combatZone.completed == true)
        {
            text.text = aC;
        }
    }


}
