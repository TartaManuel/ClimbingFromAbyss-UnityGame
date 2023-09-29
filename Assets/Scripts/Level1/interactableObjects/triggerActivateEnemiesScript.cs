using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerActivateEnemiesScript : MonoBehaviour
{
    public GameObject enemyAI1;
    public GameObject enemyAI2;
    public AudioSource fightMusic;
    LogicManagerScript logicManagerScript;
    private bool onlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScript = GameObject.FindGameObjectWithTag("LogicLevel1").GetComponent<LogicManagerScript>();
        onlyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if(onlyOnce)
            {
                enemyAI1.GetComponentInChildren<EnemyAIScript>().startPath = true;
                enemyAI2.GetComponentInChildren<EnemyAI2Script>().startPath = true;
                logicManagerScript.PlayFightMusic();
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (onlyOnce)
            {
                enemyAI1.GetComponentInChildren<EnemyAIScript>().startPath = true;
                enemyAI2.GetComponentInChildren<EnemyAI2Script>().startPath = true;
                logicManagerScript.PlayFightMusic();
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    //functie care ma impinge cand ajung pe trigger, ca sa nu pot sta in zid
    public IEnumerator PushOnTrigger(GameObject player)
    {
        Vector2 direction = Vector2.right * 10;
        Debug.Log(direction.x);
        onlyOnce = false;

        if ((Math.Abs(player.GetComponent<Rigidbody2D>().velocity.x) < Math.Abs(direction.x)) || player.GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            player.GetComponent<PlayerScript>().knocked = true;
            player.GetComponent<Rigidbody2D>().velocity = direction * 5;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<PlayerScript>().knocked = false;
        }
    }
}
