using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptLevel3 : MonoBehaviour
{

    public Rigidbody2D playerRigidBody;
    private LogicManagerScriptLevel3 logicManagerScriptLevel3;
    public AudioSource jumpSound;

    public bool playerAlive = true;
    public bool playerInVision = true;

    public bool setSelectedOnce = true;

    //variabile pentru movement
    public float speed = 25;
    public float jump = 2000;
    public float Move;

    public bool canJump;
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
    private WeaponScriptLevel3 weaponScriptLevel3;

    //pentru blood effects
    public GameObject bloodParticlesLevel2;

    //animatie idle si movement
    public Animator playerAnimator;

    //dust on movement
    public ParticleSystem dustOnMovement;
    private bool dustActive;

    //dmg sa poti lua doar din catva in catva timp
    public float dmgCooldown;
    public float dmgCooldownActual;

    //soundEffect push player
    public AudioSource knockbackSoundEffect;
    public bool knocked;
    public CameraScriptLevel3 cameraScriptLevel3;

    //pentru shield power
    private bool shieldPowerActive;
    private float shieldPowerCooldown;
    private float shieldPowerCooldownActual;
    public GameObject shieldOnCooldown;
    public GameObject shieldObject;
    public GameObject shieldDestroyParticles;
    public AudioSource shieldBreakingSoundEffect;

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

        logicManagerScriptLevel3 = GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>();
        weaponScriptLevel3 = GameObject.FindGameObjectWithTag("WeaponLevel3").GetComponent<WeaponScriptLevel3>();

        maxHealth = 5;
        health = maxHealth;

        dustActive = false;

        dmgCooldown = 0.5f;
        dmgCooldownActual = 0f;

        knocked = false;

        shieldPowerActive = false;
        shieldPowerCooldown = 10f;
        shieldPowerCooldownActual = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        if (!knocked)
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
            weaponScriptLevel3.Attack();
        }

        playerAnimator.SetBool("isWalking", Move != 0);

        if (dmgCooldownActual > 0)
        {
            dmgCooldownActual -= Time.deltaTime;
        }

        if (shieldPowerActive && shieldPowerCooldownActual > 0)
        {
            shieldPowerCooldownActual -= Time.deltaTime;
            shieldOnCooldown.SetActive(true);
        }
        else if(shieldPowerActive && shieldPowerCooldownActual <= 0) 
        {
            shieldOnCooldown.SetActive(false);
            shieldObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBox1Level3") || collision.gameObject.CompareTag("PushableBox1Level3")
            || collision.gameObject.CompareTag("BreakableBox5Level3") || collision.gameObject.CompareTag("FallingBox11Level3"))
        {
            canJump = true;
        }
        if(collision.gameObject.name == "BoomerangSpawn(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang1").GetComponent<EnemyBoomerangSpawned1Script>().returnedBoomerang = true;
        }
        if (collision.gameObject.name == "Boomerang(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang").GetComponent<EnemyBoomergangScript>().returnedBoomerang = true;
        }
        if (collision.gameObject.name == "BoomerangBoss(Clone)")
        {
            GameObject.FindGameObjectWithTag("BossLevel3").GetComponent<BossLevel3Script>().returnedBoomerang = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBox1Level3") || collision.gameObject.CompareTag("PushableBox1Level3")
            || collision.gameObject.CompareTag("BreakableBox5Level3") || collision.gameObject.CompareTag("FallingBox11Level3"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("FallingBox1Level3") || collision.gameObject.CompareTag("PushableBox1Level3")
            || collision.gameObject.CompareTag("BreakableBox5Level3") || collision.gameObject.CompareTag("FallingBox11Level3"))
        {
            canJump = false;
        }
    }

    //flip la sprite in functie de orientarea actuala
    private void FlipPlayerOnChangeDirection()
    {
        if ((rightOriented && Move < 0) || (!rightOriented && Move > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            rightOriented = !rightOriented;
            transform.localScale = localScale;

            // la fel si pentru particle effects
            Vector3 localScaleParticleEffects = weaponScriptLevel3.buffedAttackParticles.GetComponent<Transform>().localScale;
            localScaleParticleEffects.x *= -1;
            weaponScriptLevel3.buffedAttackParticles.GetComponent<Transform>().localScale = localScaleParticleEffects;
        }
    }

    //functie de luat damage
    public void TakeDamage(int damage, Vector3 enemyPosition)
    {
        bool hadShield = false;
        if (dmgCooldownActual <= 0)
        {
            if (shieldPowerActive && shieldPowerCooldownActual <= 0)
            {
                damage--;
                shieldPowerCooldownActual = shieldPowerCooldown;
                shieldObject.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(shieldDestroyParticles, transform.position, Quaternion.identity);
                shieldBreakingSoundEffect.Play();
                hadShield = true;
            }
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
                logicManagerScriptLevel3.GameOver();
            }
            if (!knocked && !hadShield)
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

    //knockback cu sunet, animatie
    public IEnumerator KnockbackFromBoss(Vector3 enemyPosition)
    {
        cameraScriptLevel3.Shake();
        knockbackSoundEffect.Play();
        playerAnimator.SetBool("isKnocked", true);
        knocked = true;
        Vector2 difference = new Vector2(transform.position.x - enemyPosition.x, 0);
        difference = difference.normalized;
        //playerRigidBody.AddForce(difference, ForceMode2D.Force);
        //Instantiate(bloodParticlesLevel2, transform.position, Quaternion.identity);
        GetComponent<Rigidbody2D>().velocity = difference * 30;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        knocked = false;
        playerAnimator.SetBool("isKnocked", false);
        //knockbackSoundEffect.Stop();
    }

    //knockback pentru boomerang, asemanator cu cel de mai sus
    public IEnumerator KnockbackFromBoomerang(Vector3 enemyPosition)
    {
        knockbackSoundEffect.Play();
        playerAnimator.SetBool("isKnocked", true);
        knocked = true;
        Vector2 difference = new Vector2(transform.position.x - enemyPosition.x, 0);
        difference = difference.normalized;
        GetComponent<Rigidbody2D>().velocity = difference * 30;
        yield return new WaitForSeconds(0.4f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        knocked = false;
        playerAnimator.SetBool("isKnocked", false);
        //knockbackSoundEffect.Stop();
    }

    //activare putere care iti da un shield care absoarbe un damage primit o data la cateva secunde
    public void ActivateShieldPower()
    {
        shieldPowerActive = true;
        shieldObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
