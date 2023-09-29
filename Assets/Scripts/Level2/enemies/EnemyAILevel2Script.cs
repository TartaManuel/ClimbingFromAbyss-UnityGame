using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAILevel2Script : MonoBehaviour
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
    public Animator animatorDead;
    public AudioSource deathSoundEffect;

    //trigger start path
    public bool startPath;
    private bool startOnlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2000f;
        nextWaypointDistance = 3f;
        currentWaypoint = 0;
        reachedEnd = false;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        maxHealth = 5;
        health = maxHealth;
        hpBar.maxValue = health;
        hpBar.value = health;

        startPath = false;
        startOnlyOnce = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (health == 0 && !isDead)
        {
            deathSoundEffect.Play();
            StartCoroutine(DestroyEnemy());
            isDead = true;
            GameObject.FindGameObjectWithTag("LogicLevel2").GetComponentInChildren<LogicManagerScriptLevel2>().nbOfEnemies--;
        }

        if (startPath && startOnlyOnce)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            startOnlyOnce = false;
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

            if (force.x >= 0.01f)
            {
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                }
            }
            else if (force.x <= -0.01f)
            {
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(-1f * GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    GetComponentInChildren<Canvas>().transform.localScale = new Vector3(GetComponentInChildren<Canvas>().transform.localScale.x, GetComponentInChildren<Canvas>().transform.localScale.y, GetComponentInChildren<Canvas>().transform.localScale.z);
                }
            }
        }
    }

    //functii pentru path finding
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
        if (collision.gameObject.CompareTag("PlayerLevel2"))
        {
            if (health > 0)
            {
                playerScriptLevel2.TakeDamage(1, transform.position);

                Vector2 difference = -collision.transform.position + transform.position;
                difference = difference.normalized * 50;

                rb.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rb));
            }
        }
    }

    //functia de primit damage de la caracter, activez si blood particles
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
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation
    }

    private IEnumerator Knockback(Rigidbody2D playerRB)
    {
        yield return new WaitForSeconds(0.2f);
        playerRB.velocity = Vector2.zero;
    }

    private IEnumerator DestroyEnemy()
    {
        deathSoundEffect.Play();
        animatorDead.SetTrigger("Dead");
        enemy.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        enemy.SetActive(false);
    }
}
