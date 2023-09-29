using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel2Script : MonoBehaviour
{
    public Transform playerLevel2;
    public float speed;
    public float nextWaypointDistance;

    Pathfinding.Path path;
    int currentWaypoint;
    bool reachedEnd;

    Seeker seeker;
    Rigidbody2D rb;

    public Transform enemyVisuals;

    public PlayerScriptLevel2 playerScriptLevel2;

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

    private float cooldownDash = 2;
    private float cooldownDashActual = 0;
    private bool didDash;

    public BossWeaponLevel2Script bossWeaponLevel2Script;
    public RangeWeaponBossLevel2Script rangeWeaponBossLevel2Script;
    private double whichAttack;
    private float cooldownAttack;
    private float cooldownAttackActual;
    private bool restartedCooldownAttack;
    private bool jumpedForAttack;

    private int damageTookIntermediate = 0;
    public bool shouldMove;
    private float timeAfterJump;
    private float timeFromLastAttack;

    //soundEffectsAttacks
    public AudioSource jumpAttackSoundEffect;

    public CameraScriptLevel2 cameraScriptLevel2;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2000f;
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

        cooldownDash = 2;
        cooldownDashActual = 0;
        didDash = false;

        enemyVisuals.GetComponent<TrailRenderer>().enabled = false;

        //pentru attacks
        cooldownAttack = 1.5f;
        cooldownAttackActual = 1f;
        restartedCooldownAttack = true;
        jumpedForAttack = false;
        shouldMove = true;
        timeAfterJump = 0;
        timeFromLastAttack = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldownDashActual > 0)
        {
            cooldownDashActual -= Time.deltaTime;
            if (cooldownDashActual <= 1.8 && didDash)
            {
                enemyVisuals.GetComponent<TrailRenderer>().enabled = false;
                rb.velocity = Vector2.zero;
                didDash = false;
            }
        }

        if (health == 0 && !isDead)
        {
            deathSoundEffect.Play();
            StartCoroutine(DestroyEnemy());
            isDead = true;
            GameObject.FindGameObjectWithTag("LogicLevel2").GetComponentInChildren<LogicManagerScriptLevel2>().nbOfEnemiesGrounded--;
        }

        if (startPath && startOnlyOnce)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            startOnlyOnce = false;
            animatorEnemy.SetTrigger("Movement");
        }

        if (startPath)
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

            if(shouldMove)
            {
                //vector care indica directia in care trebuie sa continue calea de la inamic spre noi
                //normalizam ca lungimea vectorului sa fie 1
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                //forta pe care vrem sa o aplicam. inmultim cu deltatime ca sa nu varieze in functie de framerate
                Vector2 force = direction * speed * Time.deltaTime;


                //daca inamicul este destul de aproape de caracter da un mic dash, altfel se misca normal
                if (Math.Abs(playerLevel2.transform.position.x - transform.position.x) < 20 && cooldownDashActual <= 0)
                {
                    force = new Vector2(force.x * 70, force.y);
                    cooldownDashActual = cooldownDash;
                    enemyVisuals.GetComponent<TrailRenderer>().enabled = true;
                    didDash = true;
                    rb.AddForce(force);
                }
                else if (Math.Abs(GetComponent<Rigidbody2D>().velocity.x) < 20)
                {
                    rb.AddForce(force);
                }


                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance) //daca am ajuns la o distanta de nextWaypoint mai mica decat acel threshold, trecem
                {
                    currentWaypoint++;
                }

                if (force.x >= 0.1f && oldForce.x > 0)
                {
                    if (transform.localScale.x < 0)
                    {
                        rb.velocity = new Vector2(0, 0);
                        transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    rangeWeaponBossLevel2Script.rightOriented = true;
                }
                else if (force.x <= -0.1f && oldForce.x < 0)
                {
                    if (transform.localScale.x > 0)
                    {
                        rb.velocity = new Vector2(0, 0);
                        transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                    }
                    rangeWeaponBossLevel2Script.rightOriented = false;
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

                        float distanceToPlayer = Math.Abs(transform.position.x - playerLevel2.position.x);

                        if (whichAttack < 0.25 && distanceToPlayer < 10)
                        {
                            //attack-ul cu sabia
                            bossWeaponLevel2Script.Attack();
                            cooldownAttackActual = cooldownAttack;

                        }
                        else if (whichAttack >= 0.25 && whichAttack < 0.5)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(AttackRange());
                        }
                        else if (whichAttack >= 0.5 && whichAttack < 0.75)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(AttackRangeAbove());
                        }
                        else if (whichAttack >= 0.75)
                        {
                            restartedCooldownAttack = false;
                            StartCoroutine(JumpForAttack());
                        }

                    }
                }
            }

            if(jumpedForAttack)
            {
                rb.isKinematic = false;
                timeAfterJump += Time.deltaTime;
            }
            if(timeAfterJump > 2)
            {
                jumpedForAttack = false;
                timeAfterJump = 0;
                GetComponent<Rigidbody2D>().isKinematic = false;
                enemyVisuals.GetComponent<TrailRenderer>().enabled = false;
                cooldownAttackActual = cooldownAttack;
                restartedCooldownAttack = true;
                shouldMove = true;
            }

            //ca sa primesc knockback doar daca dau 4 atacuri relativ consecutive, nu cu mult timp intre ele
            if(damageTookIntermediate > 0)
            {
                timeFromLastAttack += Time.deltaTime;
            }
            if(timeFromLastAttack > 2f)
            {
                damageTookIntermediate = 0;
            }
        }
    }

    //corutina pentru primul atac
    private IEnumerator AttackRange()
    {
        shouldMove = false;

        enemyVisuals.GetComponent<TrailRenderer>().enabled = true;
        Vector2 direction = (playerLevel2.transform.position - transform.position).normalized;
        Vector2 force = -direction * speed * Time.deltaTime;
        force = new Vector2(force.x * 50, force.y);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = false;
        rb.AddForce(force);

        yield return new WaitForSeconds(0.5f);
        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;

        rangeWeaponBossLevel2Script.ShootFireball();

        yield return new WaitForSeconds(2.5f);
        enemyVisuals.GetComponent<TrailRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        cooldownAttackActual= cooldownAttack;
        restartedCooldownAttack = true;
        shouldMove = true;
    }

    //corutina pentru al doilea atac, cel cu fireballs deasupra caracterului
    private IEnumerator AttackRangeAbove()
    {
        shouldMove = false;

        enemyVisuals.GetComponent<TrailRenderer>().enabled = true;
        Vector2 direction = (playerLevel2.transform.position - transform.position).normalized;
        Vector2 force = -direction * speed * Time.deltaTime;
        force = new Vector2(force.x * 50, force.y);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = false;
        rb.AddForce(force);

        yield return new WaitForSeconds(0.5f);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;

        rangeWeaponBossLevel2Script.ShootFireballAbove(playerLevel2.position);

        yield return new WaitForSeconds(2f);
        enemyVisuals.GetComponent<TrailRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;

        shouldMove = true;
    }

    //corutina in care bossul sare ca sa faca al treilea atac
    private IEnumerator JumpForAttack()
    {
        shouldMove = false;

        enemyVisuals.GetComponent<TrailRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        yield return new WaitForSeconds(0.1f);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 60);
        jumpedForAttack = true;
    }

    //inca o corutina la jump atac in care da fireballurile la coliziunea cu pamantul
    private IEnumerator AttackAfterJump()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;
        rangeWeaponBossLevel2Script.ShootFireballAfterJump();

        yield return new WaitForSeconds(0.5f);

        GetComponent<Rigidbody2D>().isKinematic = false;
        enemyVisuals.GetComponent<TrailRenderer>().enabled = false;
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
        shouldMove = true;
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
            seeker.StartPath(rb.position, playerLevel2.position, OnPathComplete);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel2") && !jumpedForAttack)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else if (collision.gameObject.CompareTag("Floor") && jumpedForAttack)
        {
            cameraScriptLevel2.Shake();
            jumpAttackSoundEffect.Play();
            jumpedForAttack = false;
            timeAfterJump = 0;
            StartCoroutine(AttackAfterJump());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel2"))
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    //functia cu care bossul ia damage, o data la 4 atacuri consecutive, playerul este impins
    public void TakeDamage(int damage)
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
            GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>().bossKilled = true;
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation

        damageTookIntermediate++;
        timeFromLastAttack = 0;

        if (damageTookIntermediate == 4)
        {
            damageTookIntermediate = 0;
            StartCoroutine(playerScriptLevel2.KnockbackFromBoss(transform.position));
        }
    }

    private IEnumerator Knockback(Rigidbody2D playerRB)
    {
        yield return new WaitForSeconds(0.2f);
        playerRB.velocity = Vector2.zero;
    }

    //functia apelata cand hp=0
    private IEnumerator DestroyEnemy()
    {
        Debug.Log("Intra aici");
        deathSoundEffect.Play();
        enemy.GetComponent<Rigidbody2D>().gravityScale = 0;
        animatorEnemy.SetTrigger("Dead");
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        enemy.SetActive(false);
    }
}
