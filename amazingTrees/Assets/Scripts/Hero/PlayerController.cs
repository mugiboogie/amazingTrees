using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int selection;

    public Hero[] heroes;
    public float[] heroHealth;
    public Hero hero;
    private Image charPort;
    private Image charName;

    public GameObject avatar;
    public GameObject spell;
    public GameObject decoy;
    public GameObject bishoujoEyes;
    public PlayerAvatarDefinition avatarDefinition;
    public Animator anim;
    public bool manaAllMax;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    public float[] heroMana;

    private AudioSource audioVoice;
    private AudioSource audioSound;

    float autoSpawnTime;
    bool autoSpawned;

    private Transform UI;

    private AudioListener audioListener;
    private Transform camera;

    public bool allHeroesDead;
    

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        audioVoice = transform.Find("HeroVoice").GetComponent<AudioSource>();
        audioSound = transform.Find("HeroSound").GetComponent<AudioSource>();
        UI = GameObject.FindGameObjectWithTag("UI").transform;
        autoSpawnTime = Time.time + .1f;
        charPort = GameObject.FindGameObjectWithTag("HealthBar").transform.Find("CharacterPortrait").transform.Find("charPort").GetComponent<Image>();
        charName = GameObject.FindGameObjectWithTag("HealthBar").transform.Find("CharacterName").transform.Find("charName").GetComponent<Image>();
        audioListener = GameObject.FindGameObjectWithTag("AudioListener").GetComponent<AudioListener>();
        camera = Camera.main.transform;
        heroHealth = new float[heroes.Length];
        heroMana = new float[heroes.Length];
        
        for(int i=0; i<heroHealth.Length; i++)
        {
            heroHealth[i] = playerHealth.maxHealth;
        }

        for (int i=0; i<heroMana.Length; i++)
        {
            heroMana[i] = 0f;
        }

    }

    void FixedUpdate()
    {
        audioListener.transform.position = transform.position;
        audioListener.transform.rotation = camera.transform.rotation;
    }

    void Update()
    {
        allHeroesDead = true;
        int numHeroesAlive = 0;
        for(int i=0; i<heroHealth.Length; i++)
        {
            if(heroHealth[i]>0f) { allHeroesDead = false; numHeroesAlive++; }
        }

        if((Time.time>autoSpawnTime) && (autoSpawned==false))
        {
            autoSpawned = true;

            hero = heroes[0];
            selection++;
            if (selection >= heroes.Length) { selection = 0; }


            SummonHero();
        }

        if((allHeroesDead == false) &&(Input.GetKeyDown(KeyCode.F)) && (numHeroesAlive>1))
        {
            TrySummonHero();            
        }

        int currentCharacter = selection - 1;
        if (currentCharacter < 0) { currentCharacter = heroes.Length - 1; }
        heroHealth[currentCharacter] = playerHealth.currentHealth;
        heroMana[currentCharacter] = playerAttack.mana;

        manaAllMax = true;

        for (int i = 0; i < heroMana.Length; i++)
        {
            if(heroMana[i] < playerAttack.manaMax)
            {
                manaAllMax = false;
            }

        }

    }

    public void TrySummonHero()
    {
        bool selected = false;
        while ((selected == false) && (allHeroesDead == false))
        {
            if (heroHealth[selection] > 0f)
            {
                hero = heroes[selection];
                playerHealth.currentHealth = heroHealth[selection];
                playerAttack.mana = heroMana[selection];

                selection++;
                if (selection >= heroes.Length) { selection = 0; }


                SummonHero();

                selected = true;
            }
            else
            {
                selection++;
            }
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

        if (hero.summonSfx.Length > 0) { audioVoice.PlayOneShot(hero.summonSfx[Random.Range(0, hero.summonSfx.Length)]); }

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
        charPort.sprite = hero.heroPortraitImg;
        charName.sprite = hero.heroNameImg;
    }

}

internal class PlayerMana
{
}