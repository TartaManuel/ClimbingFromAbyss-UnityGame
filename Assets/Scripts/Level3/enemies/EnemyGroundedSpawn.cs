using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedSpawn : MonoBehaviour
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

    private float cooldownDash = 2;
    private float cooldownDashActual = 0;
    private bool didDash;

    public GameObject stunParticles;
    public bool isStunned;
    private float timeStunned;
    private float timeStunnedActual;
    private bool isIdle;
    public AudioSource stunSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2000f;
        nextWaypointDistance = 3f;
        currentWaypoint = 0;
        reachedEnd = false;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //InvokeRepeating("UpdatePath", 0f, 0.5f);

        maxHealth = 4;
        health = maxHealth;
        hpBar.maxValue = health;
        hpBar.value = health;

        startPath = false;
        startOnlyOnce = true;

        cooldownDash = 2f;
        cooldownDashActual = 1;
        didDash = false;

        enemyVisuals.GetComponent<TrailRenderer>().enabled = false;

        //if (PlayerPrefs.GetInt("checkpointReached") == 1)
        //{
        //    StartCoroutine(DestroyEnemyForCheckpoint());
        //}

        StartCoroutine(SpawnEnemy());

        isStunned = false;
        stunParticles.GetComponent<ParticleSystem>().enableEmission = false;
        timeStunned = 2f;
        timeStunnedActual = 2f;
        isIdle = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldownDashActual > 0 && !isStunned)
        {
            cooldownDashActual -= Time.deltaTime;
            if (cooldownDashActual <= 1.8f && didDash)
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
        }

        if (startPath && startOnlyOnce)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            startOnlyOnce = false;
            animatorEnemy.SetTrigger("Movement");
        }

        if (isStunned && timeStunnedActual > 0 && !isIdle)
        {
            timeStunnedActual -= Time.deltaTime;
            stunParticles.GetComponent<ParticleSystem>().enableEmission = true;
            isIdle = true;
            stunSoundEffect.Play();
        }
        else if (isStunned && timeStunnedActual > 0 && isIdle)
        {
            timeStunnedActual -= Time.deltaTime;
        }
        else if (isStunned && timeStunnedActual <= 0)
        {
            isStunned = false;
            timeStunnedActual = timeStunned;
            stunParticles.GetComponent<ParticleSystem>().enableEmission = false;
            isIdle = false;
            stunSoundEffect.Stop();
        }

        if (startPath && !isStunned)
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

            //vector care indica directia in care trebuie sa continue calea de la inamic spre noi
            //normalizam ca lungimea vectorului sa fie 1
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            //forta pe care vrem sa o aplicam. inmultim cu deltatime ca sa nu varieze in functie de framerate
            Vector2 force = direction * speed * Time.deltaTime;

            if (Math.Abs(playerLevel3.transform.position.x - transform.position.x) < 20 && cooldownDashActual <= 0)
            {
                force = new Vector2(force.x * 70, force.y);
                cooldownDashActual = cooldownDash;
                enemyVisuals.GetComponent<TrailRenderer>().enabled = true;
                didDash = true;
            }

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
            }

            oldForce = force;
        }
    }

    //functii pentru path
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
            //AstarPath.active.Scan();
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
                difference = difference.normalized * 50;

                rb.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rb));
            }
        }
    }

    //functia de primit damage
    public void TakeDamage(int damage, Transform playerTransform)
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
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation
        StartCoroutine(KnockbackFromPlayer(playerTransform));
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

    //functia de destroy la hp egal cu 0
    private IEnumerator DestroyEnemy()
    {
        deathSoundEffect.Play();
        enemy.GetComponent<Rigidbody2D>().gravityScale = 0;
        animatorEnemy.SetTrigger("Dead");
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        enemy.SetActive(false);
    }

    private IEnumerator SpawnEnemy()
    {
        animatorEnemy.SetBool("Spawn", true);
        yield return new WaitForSeconds(0.5f);
        animatorEnemy.SetBool("Spawn", false);
        startPath = true;
    }
}
