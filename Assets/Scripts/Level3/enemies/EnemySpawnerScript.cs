using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{

    public Transform playerTransform;

    public PlayerScriptLevel3 playerScriptLevel3;

    //variabile pentru health
    public int health;
    public int maxHealth;
    public GameObject enemy;
    public UnityEngine.UI.Slider hpBar;
    public bool isDead;

    //pentru blood effect
    public GameObject bloodParticles;

    //pentru death animation
    public Animator animatorEnemy;
    public AudioSource deathSoundEffect;

    private float cooldownAttack;
    private float cooldownAttackActual;
    private bool restartedCooldownAttack;
    private int numberOfEnemiesSpawned;

    public bool enemyActivated;
    private int numberOfFalls;
    private int damageTookIntermediate;
    private int whichTime;

    public GameObject enemyToSpawnBoomerang;
    public GameObject enemyToSpawnSkeleton;
    public GameObject enemyToSpawnSkeleton2;
    public BreakableAndRespawnablePlatformScript breakablePlatformsLevel3;

    public GameObject explosionParticles;
    public AudioSource teleportSoundEffect;
    public AudioSource spawnSoundEffect;


    // Start is called before the first frame update
    void Start()
    {

        maxHealth = 6;
        health = maxHealth;
        hpBar.maxValue = health;
        hpBar.value = health;

        cooldownAttack = 0.3f;
        cooldownAttackActual = 2f;
        restartedCooldownAttack = true;
        isDead = false;
        enemyActivated = false;
        numberOfEnemiesSpawned = 0;
        numberOfFalls = 0;
        damageTookIntermediate = 0;
        whichTime = 1;

        if (PlayerPrefs.GetInt("checkpointReachedLevel3") == 2)
        {
            StartCoroutine(DestroyEnemyForCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && enemyActivated)
        {
            if(numberOfEnemiesSpawned == 1 && enemyToSpawnBoomerang.active == false && numberOfFalls == 0)
            {
                //aici inseamna ca am spawnat primul inamic si l-am si omorat
                numberOfFalls++;
                breakablePlatformsLevel3.destroyFloor();
                whichTime++;
                GameObject.FindGameObjectWithTag("EnemyFixed" + whichTime).GetComponent<EnemyFixedSpawnedLevel3Script>().DestoroySelf();
                animatorEnemy.SetBool("Fell", true);
            }
            if (numberOfEnemiesSpawned == 2 && enemyToSpawnSkeleton.active == false && enemyToSpawnSkeleton2.active == false  && numberOfFalls == 1)
            {
                //aici inseamna ca am spawnat setul 2 de inamici si l-am si omorat
                numberOfFalls++;
                breakablePlatformsLevel3.destroyFloor();
                whichTime++;
                GameObject.FindGameObjectWithTag("EnemyFixed" + whichTime).GetComponent<EnemyFixedSpawnedLevel3Script>().DestoroySelf();
                animatorEnemy.SetBool("Fell", true);
            }
            if (health == 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(DestroyEnemy());
            }

            if (cooldownAttackActual > 0)
            {
                cooldownAttackActual -= Time.deltaTime;
            }
            else
            {
                if (restartedCooldownAttack && !isDead)
                {
                    float distanceToPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
                    //vreau sa ma impinga numai la primul set de inamici, pentru ca la al doilea voi fi in mijloc si inamicii se vor spawna in laterale, deci nu o sa
                    //am problema ca se spawneaza langa mine
                    if (playerScriptLevel3.canJump && distanceToPlayer < 15 && numberOfEnemiesSpawned == 0)
                    {
                        StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
                    }
                    StartCoroutine(SpawnEnemies());
                }
            }
        }
    }

    //functia pentru spawn de inamici, are un pattern exact, prima data spawneaza un inamic, dupa asteapta pana acel inamic moare dupa spawneaza alti 2 inamici
    private IEnumerator SpawnEnemies()
    {
        restartedCooldownAttack = false;

        yield return new WaitForSeconds(0.2f);

        if(numberOfEnemiesSpawned == 0 )
        {
            spawnSoundEffect.Play();
            enemyToSpawnBoomerang.SetActive(true);
            numberOfEnemiesSpawned++;
        }
        else if(numberOfEnemiesSpawned == 1)
        {
            spawnSoundEffect.Play();
            enemyToSpawnSkeleton.SetActive(true);
            enemyToSpawnSkeleton2.SetActive(true);
            numberOfEnemiesSpawned++;
        }
    }

    //functie de primit damage
    public void TakeDamage(int damage, Transform playerPosition)
    {
        int remainingHealth = health - damage;
        if (remainingHealth > 0)
        {
            health = remainingHealth;
            hpBar.value = health;

            damageTookIntermediate = damageTookIntermediate + damage;
            if(damageTookIntermediate >= 3)
            {
                damageTookIntermediate = 0;
                breakablePlatformsLevel3.restoreFloor();
                transform.position = new Vector3(501.43f, 33.11f, -2.182675f);
                animatorEnemy.SetBool("Fell", false);
                Instantiate(explosionParticles, transform.position - new Vector3(0,3,0), Quaternion.identity);
                cooldownAttackActual = cooldownAttack;
                restartedCooldownAttack = true;
                teleportSoundEffect.Play();

            }
        }
        else
        {
            health = 0;
            hpBar.value = health;

            whichTime++;
            GameObject.FindGameObjectWithTag("EnemyFixed" + whichTime).GetComponent<EnemyFixedSpawnedLevel3Script>().DestoroySelf();
            //GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().enemyBoomerangKilled = true;
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation
    }

    private IEnumerator DestroyEnemy()
    {
        deathSoundEffect.Play();
        enemy.GetComponent<Rigidbody2D>().gravityScale = 0;
        animatorEnemy.SetTrigger("Dead");
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        enemy.GetComponentInChildren<Canvas>().enabled = false;
        yield return new WaitForSeconds(1f);
        animatorEnemy.enabled = false;
        yield return new WaitForSeconds(3f);
        enemy.SetActive(false);
    }

    private IEnumerator DestroyEnemyForCheckpoint()
    {
        yield return new WaitForSeconds(0.1f);
        enemy.SetActive(false);
    }
}
