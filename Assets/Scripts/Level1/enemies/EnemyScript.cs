using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public PlayerScript playerScript;

    //variabile pentru health
    public int health;
    public int maxHealth;

    public GameObject enemy;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0)
        {
            enemy.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerScript.TakeDamage(1, transform.position);
        }
    }

    public void TakeDamage(int damage)
    {
        int remainingHealth = health - damage;
        if (remainingHealth > 0)
        {
            health = remainingHealth;
        }
        else
        {
            health = 0;
        }
    }
}
