using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptLevel2 : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;
    private LogicManagerScriptLevel2 logicManagerScriptLevel2;
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
    private WeaponScriptLevel2 weaponScriptLevel2;

    //pentru blood effects
    public GameObject bloodParticlesLevel2;

    //animatie idle si movement
    public Animator playerAnimator;

    //dust on movement
    public ParticleSystem dustOnMovement;
    private bool dustActive;

    //dmg sa poti lua doar din catva in catva timp
    public float dmgCooldown = 0.2f;
    public float dmgCooldownActual = 0f;

    //soundEffect push player
    public AudioSource knockbackSoundEffect;
    public bool knocked;
    public CameraScriptLevel2 cameraScriptLevel2;

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

        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();
        weaponScriptLevel2 = GameObject.FindGameObjectWithTag("WeaponLevel2").GetComponent<WeaponScriptLevel2>();

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

        if (Move != 0 && !dustActive && canJump)
        {
            dustOnMovement.Play();
            dustActive = true;
        }
        else if (Move == 0 || !canJump)
        {
            dustOnMovement.Stop();
            dustActive = false;
        }

        if (cooldownJump > 0f)
        {

            cooldownJump -= Time.deltaTime;
        }

        if (canJump)
        {
            counterForJump = timeForJump;
        }
        else
        {
            counterForJump -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && counterForJump > 0 && cooldownJump <= 0)
        {
            playerRigidBody.AddForce(new Vector2(playerRigidBody.velocity.x, jump));
            jumpSound.Play();
            cooldownJump = timeBetweenJumps;
        }


        if (Input.GetMouseButtonDown(0))
        {
            weaponScriptLevel2.Attack();
        }

        playerAnimator.SetBool("isWalking", Move != 0);

        if(dmgCooldownActual > 0)
        {
            dmgCooldownActual-= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBoxLevel2"))
        {
            canJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBoxLevel2"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBoxLevel2"))
        {
            canJump = false;
        }
    }

    //schimbarea orientarii sprite-ului in functie de directia de deplasare
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

    //funcita de primit damage
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
                logicManagerScriptLevel2.GameOver();
            }
            if(!knocked)
            {
                StartCoroutine(Knockback(playerRigidBody, enemyPosition));
            }

            dmgCooldownActual = dmgCooldown;
        }

    }

    private IEnumerator Knockback(Rigidbody2D playerRB, Vector3 enemyPosition)
    {
        Vector2 difference = new Vector2(transform.position.x - enemyPosition.x, 0);
        difference = difference.normalized * 12500;
        playerRigidBody.AddForce(difference, ForceMode2D.Force);
        Instantiate(bloodParticlesLevel2, transform.position, Quaternion.identity);
        playerRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        playerRB.velocity = Vector2.zero;
    }

    //functie de knockback de la boss, in care se face shake la camera, are un sunet si o animatie
    public IEnumerator KnockbackFromBoss(Vector3 enemyPosition)
    {
        cameraScriptLevel2.Shake();
        knockbackSoundEffect.Play();
        playerAnimator.SetBool("isKnocked", true);
        knocked = true;
        Vector2 difference = new Vector2(transform.position.x - enemyPosition.x, 0);
        difference = difference.normalized;
        GetComponent<Rigidbody2D>().velocity = difference * 30;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        knocked = false;
        playerAnimator.SetBool("isKnocked", false);
    }


}
