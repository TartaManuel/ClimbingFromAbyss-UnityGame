using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponScriptLevel2 : MonoBehaviour
{
    //pentru animatie la weapon
    public Animator animatorWeapon;
    public float delay = 2.3f;
    private bool cannotAttack;

    public Transform circleCenter;
    public float radius;

    public UnityEvent onAttackPerformed;

    private EnemyAILevel2Script enemyAILevel2Script;
    private EnemyAI2Level2Script enemyAI2Level2Script;
    private ChestScriptLevel2 chestScriptLevel2;
    private BreakableWallScriptLevel2 breakableWallScriptLevel2;
    public FallingBoxLevel2Script fallingBoxLevel2Script;
    public BreakableBoxLevel2Script breakableBoxLevel2Script;
    public FallingBoxLevel2EnemiesScript fallingBoxLevel2EnemiesScript;
    public BreakableWallEnemiesScriptLevel2 breakableWallEnemiesScriptLevel2;
    public EnemyGrounded1Script enemyGrounded1Script;
    public EnemyGrounded2Script enemyGrounded2Script;
    public BossLevel2Script bossLevel2Script;

    public AudioSource swordSound;

    public void TriggerAttack()
    {
        onAttackPerformed?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        delay = 0.3f;
        enemyAILevel2Script = GameObject.FindGameObjectWithTag("EnemyAILevel2").GetComponent<EnemyAILevel2Script>();
        enemyAI2Level2Script = GameObject.FindGameObjectWithTag("EnemyAI2Level2").GetComponent<EnemyAI2Level2Script>();
        chestScriptLevel2 = GameObject.FindGameObjectWithTag("ChestLevel2").GetComponent<ChestScriptLevel2>();
        breakableWallScriptLevel2 = GameObject.FindGameObjectWithTag("BreakableWallLevel2").GetComponent<BreakableWallScriptLevel2>();
        enemyGrounded1Script = GameObject.FindGameObjectWithTag("EnemyGrounded1").GetComponent<EnemyGrounded1Script>();
        enemyGrounded2Script = GameObject.FindGameObjectWithTag("EnemyGrounded2").GetComponent<EnemyGrounded2Script>();
        bossLevel2Script = GameObject.FindGameObjectWithTag("BossLevel2").GetComponent<BossLevel2Script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //functia care activeaza animatia de atac, si dupa activeaza timpul de asteptare pana la urmatorul atac
    public void Attack()
    {
        if (cannotAttack)
        {
            return;
        }
        animatorWeapon.SetTrigger("Attack");
        cannotAttack = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        cannotAttack = false;
    }

    //desenarea zonei in care trebuie sa fie inamicii pentru a fi loviti de atac
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position;
        if (circleCenter == null)
        {
            position = Vector3.zero;
        }
        else
        {
            position = circleCenter.position;
        }
        Gizmos.DrawWireSphere(position, radius);
    }

    //detectare coliziuni in timpul atacului
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
        {
            if (collider.name == "BreakableWall")
            {
                swordSound.Play();
                breakableWallScriptLevel2.destroyWall();

            }
            else if (collider.name == "ChestLevel2")
            {
                chestScriptLevel2.GetHit();
            }
            else if (collider.name == "FallingBoxLevel2")
            {
                swordSound.Play();
                fallingBoxLevel2Script.SetGravity();
            }
            else if (collider.name == "BreakableBoxLevel2")
            {
                swordSound.Play();
                breakableBoxLevel2Script.DestroyBox();
            }
            else if (collider.name == "EnemyAILevel2")
            {
                enemyAILevel2Script.TakeDamage(1);
                swordSound.Play();

                Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                Vector2 difference = rbEnemy.transform.position - transform.position;
                difference = difference.normalized * 50;

                rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rbEnemy));
            }
            else if (collider.name == "EnemyAI2Level2")
            {
                enemyAI2Level2Script.TakeDamage(1);
                swordSound.Play();

                Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                Vector2 difference = rbEnemy.transform.position - transform.position;
                difference = difference.normalized * 50;

                rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rbEnemy));
            }
            else if (collider.name == "FallingBoxLevel2Enemies")
            {
                swordSound.Play();
                fallingBoxLevel2EnemiesScript.SetGravity();
            }
            else if (collider.name == "BreakableWallEnemies")
            {
                swordSound.Play();
                breakableWallEnemiesScriptLevel2.destroyWall();

            }
            else if (collider.name == "EnemyGrounded1")
            {
                enemyGrounded1Script.TakeDamage(1);
                swordSound.Play();

                Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                Vector2 difference = rbEnemy.transform.position - transform.position;
                difference = difference.normalized * 50;

                rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rbEnemy));
            }
            else if (collider.name == "EnemyGrounded2")
            {
                enemyGrounded2Script.TakeDamage(1);
                swordSound.Play();

                Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                Vector2 difference = rbEnemy.transform.position - transform.position;
                difference = difference.normalized * 50;

                rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(Knockback(rbEnemy));
            }
            else if (collider.name == "BossLevel2")
            {
                bossLevel2Script.TakeDamage(1);
                swordSound.Play();

                Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();

                //rbEnemy.isKinematic = false;
                rbEnemy.velocity = Vector2.zero;
                //rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockbackBoss(rbEnemy));
            }
        }
    }

    private IEnumerator Knockback(Rigidbody2D rbEnemy)
    {
        yield return new WaitForSeconds(0.3f);
        rbEnemy.velocity = Vector2.zero;

    }

    private IEnumerator KnockbackBoss(Rigidbody2D rbEnemy)
    {
        Vector2 difference = rbEnemy.transform.position - transform.position;
        difference = new Vector2(difference.normalized.x * 1, 0);
        rbEnemy.velocity = difference;

        GameObject.FindGameObjectWithTag("BossLevel2").GetComponent<BossLevel2Script>().shouldMove = false;

        yield return new WaitForSeconds(0.1f);
        rbEnemy.velocity = Vector2.zero;
        rbEnemy.isKinematic = true;

        GameObject.FindGameObjectWithTag("BossLevel2").GetComponent<BossLevel2Script>().shouldMove = true;

    }
}
