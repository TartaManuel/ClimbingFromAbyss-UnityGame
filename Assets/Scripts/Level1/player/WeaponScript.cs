using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WeaponScript : MonoBehaviour
{
    //pentru animatie la weapon
    public Animator animatorWeapon;
    public float delay = 0.3f;
    private bool cannotAttack;

    public Transform circleCenter;
    public float radius;

    public UnityEvent onAttackPerformed;

    private EnemyAIScript enemyScript;
    private EnemyAI2Script enemyAI2Script;
    private ChestScript chestScript;

    public AudioSource swordSound;

    public void TriggerAttack()
    {
        onAttackPerformed?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        delay = 0.3f;
        enemyScript = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAIScript>();
        enemyAI2Script = GameObject.FindGameObjectWithTag("EnemyAI2").GetComponent<EnemyAI2Script>();
        chestScript = GameObject.FindGameObjectWithTag("Chest").GetComponent<ChestScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //functia de atac care verifica daca pot ataca, si daca pot activeaza triggerul pentru animatia de atac si incepe corutina de delay
    public void Attack()
    {
        if (cannotAttack)
            return;
        animatorWeapon.SetTrigger("Attack");
        cannotAttack = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        cannotAttack = false;
    }

    //functia care deseneaza cercul care reprezinta zona in care trebuie sa se afle in timp ce atac ca sa ia damage
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position;
        if(circleCenter == null)
        {
            position= Vector3.zero;
        }
        else
        {
            position = circleCenter.position;
        }
        Gizmos.DrawWireSphere(position, radius);
    }

    //functia care detecteaza coliziuni in momentul in care am atacat. in functie de obiectul detectat fac diferite lucruri
    public void DetectColliders()
    {
        bool wallBetween = false;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
        {
            if (collider.tag == "Floor")
            {
                wallBetween = true;

            }
        }
        if (!wallBetween)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
            {
                if (collider.name == "EnemyAI")
                {
                    enemyScript.TakeDamage(1);
                    swordSound.Play();

                    Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                    Vector2 difference = rbEnemy.transform.position - transform.position;
                    difference = difference.normalized * 50;

                    rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                    StartCoroutine(Knockback(rbEnemy));

                }
                else if (collider.name == "Chest")
                {
                    chestScript.GetHit();
                }
                else if (collider.name == "EnemyAI2")
                {
                    enemyAI2Script.TakeDamage(1);
                    swordSound.Play();

                    Rigidbody2D rbEnemy = collider.GetComponent<Rigidbody2D>();
                    Vector2 difference = rbEnemy.transform.position - transform.position;
                    difference = difference.normalized * 50;

                    rbEnemy.AddForce(difference, ForceMode2D.Impulse);
                    StartCoroutine(Knockback(rbEnemy));

                }
            }
        }
    }

    //functie folosita ca sa dau knockback la diverse obiecte cu care am intrat in coliziune
    private IEnumerator Knockback(Rigidbody2D rbEnemy)
    {
        yield return new WaitForSeconds(0.2f);
        rbEnemy.velocity = Vector2.zero;
    }
}
