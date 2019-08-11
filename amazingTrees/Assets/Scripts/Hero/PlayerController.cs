using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int selection;
    public Hero[] heroes;
    public Hero hero;

    public GameObject avatar;
    public GameObject spell;
    public GameObject decoy;
    public GameObject bishoujoEyes;
    public PlayerAvatarDefinition avatarDefinition;
    public Animator anim;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private AudioSource audio;

    float autoSpawnTime;
    bool autoSpawned;

    private Transform UI;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        audio = GetComponent<AudioSource>();
        UI = GameObject.FindGameObjectWithTag("UI").transform;
        autoSpawnTime = Time.time + .1f;


    }

    private void Update()
    {
        if((Time.time>autoSpawnTime) && (autoSpawned==false))
        {
            autoSpawned = true;
            hero = heroes[0];
            selection++;
            if (selection >= heroes.Length) { selection = 0; }
            SummonHero();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            hero = heroes[selection];
            selection++;
            if(selection>=heroes.Length) { selection = 0; }
            SummonHero();
        }
    }

    private void SummonHero()
    {
        if (avatar != null) { Destroy(avatar); }
        if (decoy != null) { Destroy(decoy); }
        if (spell != null) { Destroy(spell); }
        if (bishoujoEyes != null) { Destroy(bishoujoEyes); }

        GameObject avatarSpawn = Instantiate(hero.avatar,Vector3.zero,Quaternion.identity);
        avatar = avatarSpawn;
        avatarDefinition = avatar.GetComponent<PlayerAvatarDefinition>();
        avatarDefinition.playerAttack = playerAttack;
        anim = avatar.GetComponent<Animator>();

        audio.PlayOneShot(hero.summonSfx[Random.Range(0, hero.summonSfx.Length)]);

        GameObject decoySpawn = Instantiate(hero.decoy, Vector3.zero, Quaternion.identity);
        decoy = decoySpawn;

        GameObject spellSpawn = Instantiate(hero.spell, Vector3.zero, Quaternion.identity);
        spell = spellSpawn;

        GameObject bishoujoEyesSpawn = Instantiate(hero.bishoujoEyes, Vector3.zero, Quaternion.identity);
        bishoujoEyes = bishoujoEyesSpawn;
        bishoujoEyes.transform.SetParent(UI, false);

        playerAttack.SummonHero();
        playerMovement.SummonHero();
        playerHealth.SummonHero();
        decoy.GetComponent<PlayerDecoyController>().SummonHero();
    }

}
