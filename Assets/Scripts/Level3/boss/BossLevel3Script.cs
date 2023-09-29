using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel3Script : MonoBehaviour
{

    public Transform playerLevel3;
    public float speed;
    public float nextWaypointDistance;

    Pathfinding.Path path;
    int currentWaypoint;
    bool reachedEnd;

    Seeker seeker;
    Rigidbody2D rb;

    public Transform enemyVisuals;

    public PlayerScriptLevel3 playerScriptLevel3;

    //variabile pentru health
    public int health;
    public int maxHealth;
    public GameObject enemy;
    public UnityEngine.UI.Slider hpBar;
    public bool isDead = false;

    //pentru blood effect
    public GameObject bloodParticles;

    //pentru death animation
    public Animator animatorEnemy;
    public AudioSource deathSoundEffect;

    //trigger start path
    public bool startPath;
    private bool startOnlyOnce;

    Vector2 oldForce = new Vector2(-1, -1);

    public RangeWeaponBossLevel3Script rangeWeaponBossLevel3Script;
    private double whichAttack;
    private float cooldownAttack;
    private float cooldownAttackActual;
    private bool restartedCooldownAttack;
    private bool jumpedForAttack;

    private int damageTookIntermediate = 0;
    public bool shouldMove;
    private float timeAfterJump;
    private float timeFromLastAttack;
    public bool returnedBoomerang;
    public GameObject explosionParticles;
    public AudioSource teleportSound;

    public GameObject stunParticles;

    public bool isStunned;
    private float timeStunned;
    private float timeStunnedActual;
    private bool isIdle;
    public AudioSource stunSoundEffect;

    //soundEffectsAttacks
    //public AudioSource jumpAttackSoundEffect;

    public CameraScriptLevel3 cameraScriptLevel3;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1000f;
        nextWaypointDistance = 3f;
        currentWaypoint = 0;
        reachedEnd = false;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        maxHealth = 35;
        health = maxHealth;
        hpBar.maxValue = health;
        hpBar.value = health;

        startPath = false;
        startOnlyOnce = true;

        //pentru attacks
        cooldownAttack = 2.3f;
        cooldownAttackActual = 2f;
        restartedCooldownAttack = true;
        jumpedForAttack = false;
        shouldMove = true;
        timeAfterJump = 0;
        timeFromLastAttack = 0;
        returnedBoomerang = true;

        isStunned = false;
        stunParticles.GetComponent<ParticleSystem>().enableEmission = false;
        timeStunned = 2f;
        timeStunnedActual = 2f;
        isIdle = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (health == 0 && !isDead)
        {
            StopCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
            StartCoroutine(DestroyEnemy());
            isDead = true;
            GameObject.FindGameObjectWithTag("LogicLevel3").GetComponentInChildren<LogicManagerScriptLevel3>().bossKilled = true;
        }

        if (startPath && startOnlyOnce)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            startOnlyOnce = false;
            animatorEnemy.SetTrigger("Movement");
        }

        if(isStunned && timeStunnedActual > 0 && !isIdle)
        {
            timeStunnedActual-= Time.deltaTime;
            stunParticles.GetComponent<ParticleSystem>().enableEmission = true;
            animatorEnemy.SetTrigger("Idle");
            isIdle= true;
            stunSoundEffect.Play();
        }
        else if (isStunned && timeStunnedActual > 0 && isIdle)
        {
            timeStunnedActual -= Time.deltaTime;
        }
        else if(isStunned && timeStunnedActual <=0)
        {
            isStunned = false;
            timeStunnedActual = timeStunned;
            stunParticles.GetComponent<ParticleSystem>().enableEmission = false;
            animatorEnemy.SetTrigger("Movement");
            isIdle = false;
            stunSoundEffect.Stop();
        }

        if (startPath && !isDead && !isStunned)
        {

            if (path == null)
            {
                return;
            }
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEnd = true;
                return;
            }
            else
            {
                reachedEnd = false;
            }

            if (shouldMove)
            {
                //vector care indica directia in care trebuie sa continue calea de la inamic spre noi
                //normalizam ca lungimea vectorului sa fie 1
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                //forta pe care vrem sa o aplicam. inmultim cu deltatime ca sa nu varieze in functie de framerate
                Vector2 force = direction * speed * Time.deltaTime;
                rb.AddForce(force);


                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance) //daca am ajuns la o distanta de nextWaypoint mai mica decat acel threshold, trecem
                {
                    currentWaypoint++;
                }

                if (force.x >= 0.1f && oldForce.x > 0)
                {
                    if (transform.localScale.x > 0)
                    {
                        rb.velocity = new Vector2(0, 0);
                        transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                        stunParticles.transform.localScale = new Vector3(-1f * stunParticles.transform.localScale.x, stunParticles.transform.localScale.y, stunParticles.transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    rangeWeaponBossLevel3Script.rightOriented = true;
                }
                else if (force.x <= -0.1f && oldForce.x < 0)
                {
                    if (transform.localScale.x < 0)
                    {
                        rb.velocity = new Vector2(0, 0);
                        transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                        stunParticles.transform.localScale = new Vector3(-1f * stunParticles.transform.localScale.x, stunParticles.transform.localScale.y, stunParticles.transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    rangeWeaponBossLevel3Script.rightOriented = false;
                }

                oldForce = force;


                //partea in care daca cooldownul atacurilor este 0, alege unul din cele posibile
                if (cooldownAttackActual > 0)
                {
                    cooldownAttackActual -= Time.deltaTime;
                }
                else
                {
                    if (restartedCooldownAttack)
                    {
                        System.Random rnd = new System.Random();
                        whichAttack = rnd.NextDouble();

                        if (whichAttack < 0.25)
                        {
                            //attack-ul simplu cu boomerang
                            restartedCooldownAttack = false;
                            float distanceToPlayer = Math.Abs(transform.position.x - playerLevel3.position.x);
                            if (playerScriptLevel3.canJump && !playerScriptLevel3.knocked && distanceToPlayer < 15)
                            {
                                StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
                            }
                            StartCoroutine(AttackBoomerang());

                        }
                        else if (whichAttack >= 0.25 && whichAttack < 0.5)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(AttackBoomerang4Directions());
                        }
                        else if (whichAttack >= 0.5 && whichAttack < 0.75)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(AttackBoomerang4DirectionsDiagonal());
                        }
                        else if (whichAttack >= 0.75)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(AttackBoomerang8Directions());
                        }

                    }
                }
            }

            //ca sa primesc knockback doar daca dau 4 atacuri relativ consecutive, nu cu mult timp intre ele
            if (damageTookIntermediate > 0)
            {
                timeFromLastAttack += Time.deltaTime;
            }
            if (timeFromLastAttack > 1f)
            {
                damageTookIntermediate = 0;
            }
        }
    }

    //functia de atac cu boomerang, poate sa fie activata doar daca inamicul se uita catre jucator, si nu face nimic pana ce boomerangul nu se intoarce la el
    private IEnumerator AttackBoomerang()
    {
        shouldMove = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animatorEnemy.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);

        if ((rangeWeaponBossLevel3Script.rightOriented && playerLevel3.position.x > transform.position.x) || (!rangeWeaponBossLevel3Script.rightOriented && playerLevel3.position.x < transform.position.x))
        {
            rangeWeaponBossLevel3Script.ShootBoomerang();
        }

        while (!returnedBoomerang)
        {
            yield return new WaitForSeconds(0.1f);
        }
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
        animatorEnemy.SetTrigger("Movement");
        shouldMove = true;
    }

    private IEnumerator AttackBoomerang4Directions()
    {
        float distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if (distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        shouldMove = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animatorEnemy.SetTrigger("Idle");
        GetComponent<Transform>().position = new Vector3(698.3f, 72.2f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = true;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        teleportSound.Play();
        yield return new WaitForSeconds(0.5f);

        rangeWeaponBossLevel3Script.ShootBoomerangFourDirections();

        yield return new WaitForSeconds(1f);

        distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if(distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        yield return new WaitForSeconds(0.2f);
        GetComponent<Transform>().position = new Vector3(698.3f, 65f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = false;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
        animatorEnemy.SetTrigger("Movement");
        shouldMove = true;
        teleportSound.Play();
    }

    private IEnumerator AttackBoomerang4DirectionsDiagonal()
    {
        float distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if (distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        shouldMove = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animatorEnemy.SetTrigger("Idle");
        GetComponent<Transform>().position = new Vector3(698.3f, 72.2f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = true;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        teleportSound.Play();
        yield return new WaitForSeconds(0.5f);

        rangeWeaponBossLevel3Script.ShootBoomerangFourDirectionsDiagonal();

        yield return new WaitForSeconds(1f);

        distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if (distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        yield return new WaitForSeconds(0.2f);
        GetComponent<Transform>().position = new Vector3(698.3f, 65f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = false;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
        animatorEnemy.SetTrigger("Movement");
        shouldMove = true;
        teleportSound.Play();
    }

    private IEnumerator AttackBoomerang8Directions()
    {
        float distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if (distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        shouldMove = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animatorEnemy.SetTrigger("Idle");
        GetComponent<Transform>().position = new Vector3(698.3f, 72.2f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = true;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        teleportSound.Play();
        yield return new WaitForSeconds(0.5f);

        rangeWeaponBossLevel3Script.ShootBoomerangEightDirections();

        yield return new WaitForSeconds(2f);

        distanceToPlayer = Math.Abs(698.3f - playerLevel3.position.x);
        if (distanceToPlayer < 10)
        {
            StartCoroutine(playerScriptLevel3.KnockbackFromBoomerang(transform.position));
        }
        yield return new WaitForSeconds(0.2f);
        GetComponent<Transform>().position = new Vector3(698.3f, 65f, GetComponent<Transform>().position.z);
        GetComponent<Rigidbody2D>().isKinematic = false;
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
        animatorEnemy.SetTrigger("Movement");
        shouldMove = true;
        teleportSound.Play();
    }


    //functiile pentru pathfinding
    void OnPathComplete(Pathfinding.Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, playerLevel3.position, OnPathComplete);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel3"))
        {
            if (health > 0)
            {
                playerScriptLevel3.TakeDamage(1, transform.position);

                Vector2 difference = -collision.transform.position + transform.position;
                difference = new Vector2(difference.normalized.x, 0) * 50;

                rb.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rb));
            }
        }
    }

    //functia cu care bossul ia damage, o data la 4 atacuri consecutive, playerul este impins
    public void TakeDamage(int damage, Transform playerPosition)
    {
        int remainingHealth = health - damage;
        if (remainingHealth > 0)
        {
            health = remainingHealth;
            hpBar.value = health;
        }
        else
        {
            health = 0;
            hpBar.value = health;
            GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel2>().bossKilled = true;
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation
        StartCoroutine(KnockbackFromPlayer(playerPosition));

        damageTookIntermediate++;
        timeFromLastAttack = 0;

        if (damageTookIntermediate == 4)
        {
            damageTookIntermediate = 0;
            StartCoroutine(playerScriptLevel3.KnockbackFromBoss(transform.position));
        }
    }

    private IEnumerator KnockbackFromPlayer(Transform playerPosition)
    {
        Vector2 difference = transform.position - playerPosition.position;
        difference = new Vector2(difference.normalized.x * 20, 0);
        GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private IEnumerator Knockback(Rigidbody2D playerRB)
    {
        yield return new WaitForSeconds(0.2f);
        playerRB.velocity = Vector2.zero;
    }

    //functia apelata cand hp=0
    private IEnumerator DestroyEnemy()
    {
        deathSoundEffect.Play();
        enemy.GetComponent<Rigidbody2D>().gravityScale = 0;
        animatorEnemy.SetTrigger("Dead");
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        enemy.SetActive(false);
    }
}
