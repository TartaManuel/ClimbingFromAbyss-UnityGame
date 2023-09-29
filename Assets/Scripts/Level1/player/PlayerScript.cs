using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;
    private LogicManagerScript logicManagerScript;


    public AudioSource jumpSound;

    public bool playerAlive = true;
    public bool playerInVision = true;

    public bool setSelectedOnce = true;

    //variabile pentru movement
    public float speed = 25;
    public float jump = 2000;
    public float Move;

    private bool canJump;
    public bool rightOriented = true;

    public float timeForJump = 0.1f;
    public float counterForJump;
    public float timeBetweenJumps = 1f;
    public float cooldownJump = 0f;

    //variabile pentru health
    public int health;
    public int maxHealth;
    public UnityEngine.UI.Slider hpBar;


    //scriptul de la weapon
    private WeaponScript weaponScript;

    //pentru blood effects
    public GameObject bloodParticles;

    //animatie idle si movement
    public Animator playerAnimator;

    //dust on movement
    public ParticleSystem dustOnMovement;
    private bool dustActive;

    //dmg sa poti lua doar din catva in catva timp
    public float dmgCooldown = 0.2f;
    public float dmgCooldownActual = 0f;

    public bool knocked;


    // Start is called before the first frame update
    void Start()
    {
        timeBetweenJumps = 0.5f;
        cooldownJump = 0f;
        timeForJump = 0.1f;
        speed = 25;
        jump = 2400;
        playerAlive = true;
        playerInVision = true;

        logicManagerScript = GameObject.FindGameObjectWithTag("LogicLevel1").GetComponent<LogicManagerScript>();
        weaponScript = GameObject.FindGameObjectWithTag("Weapon").GetComponent<WeaponScript>();

        maxHealth = 5;
        health = maxHealth;

        dustActive = false;

        dmgCooldown = 0.2f;
        dmgCooldownActual = 0f;

        knocked = false;
    }

    // Update is called once per frame
    void Update()
    {

        Move = Input.GetAxis("Horizontal");

        if(!knocked)
        {
            playerRigidBody.velocity = new Vector2(speed * Move, playerRigidBody.velocity.y);
            FlipPlayerOnChangeDirection();
        }

        if(Move != 0 && !dustActive && canJump)
        {
            dustOnMovement.Play();
            dustActive = true;
        }
        else if(Move == 0 || !canJump)
        {
            dustOnMovement.Stop();
            dustActive = false;
        }

        if(cooldownJump > 0f) 
        {

            cooldownJump -= Time.deltaTime;
        }   

        if(canJump)
        {
            counterForJump = timeForJump;
        }
        else
        {
            counterForJump -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && counterForJump > 0 && cooldownJump <=0)
        {
            playerRigidBody.AddForce(new Vector2(playerRigidBody.velocity.x, jump));
            jumpSound.Play();
            cooldownJump = timeBetweenJumps;
        }


        if (Input.GetMouseButtonDown(0))
        {
            weaponScript.Attack();
        }

        playerAnimator.SetBool("isWalking", Move != 0);

        if (dmgCooldownActual > 0)
        {
            dmgCooldownActual -= Time.deltaTime;
        }
    }

    //pot da jump doar daca am intrat in coliziune cu pamantul si nu am iesit inca
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = false;
        }
    }

    //flip la sprite in functie de directia de miscare
    private void FlipPlayerOnChangeDirection()
    {
        if ((rightOriented && Move < 0) || (!rightOriented && Move > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            rightOriented = !rightOriented;
            transform.localScale = localScale;
        }
    }

    //functie de primit damage de la inamici
    public void TakeDamage(int damage, Vector3 enemyPosition)
    {
        if(dmgCooldownActual <= 0) 
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
                logicManagerScript.GameOver();
            }

            dmgCooldownActual = dmgCooldown;

            StartCoroutine(Knockback(playerRigidBody, enemyPosition));
        }

    }

    //knockback atunci cand iau damage
    private IEnumerator Knockback(Rigidbody2D playerRB, Vector3 enemyPosition)
    {
        Vector2 difference = new Vector2(transform.position.x - enemyPosition.x, 0);
        difference = difference.normalized * 12500;
        playerRigidBody.AddForce(difference, ForceMode2D.Force);
        Instantiate(bloodParticles, transform.position, Quaternion.identity); //ultimul parametru inseamna no rotation
        playerRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        playerRB.velocity = Vector2.zero;
    }
}
