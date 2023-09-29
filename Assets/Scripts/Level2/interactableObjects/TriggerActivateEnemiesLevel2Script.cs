using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateEnemiesLevel2Script : MonoBehaviour
{
    public GameObject enemyAI1Level2;
    public GameObject enemyAI2Level2;
    LogicManagerScriptLevel2 logicManagerScriptLevel2;
    private bool onlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();
        onlyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel2")
        {
            if(onlyOnce)
            {
                enemyAI1Level2.GetComponentInChildren<EnemyAILevel2Script>().startPath = true;
                enemyAI2Level2.GetComponentInChildren<EnemyAI2Level2Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel2")
        {
            if(onlyOnce)
            {
                enemyAI1Level2.GetComponentInChildren<EnemyAILevel2Script>().startPath = true;
                enemyAI2Level2.GetComponentInChildren<EnemyAI2Level2Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    public IEnumerator PushOnTrigger(GameObject player)
    {
        Vector2 direction = Vector2.left * 10;
        Debug.Log(direction.x);

        if ((Math.Abs(player.GetComponent<Rigidbody2D>().velocity.x) < Math.Abs(direction.x))|| player.GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            onlyOnce = false;
            player.GetComponent<PlayerScriptLevel2>().knocked = true;
            player.GetComponent<Rigidbody2D>().velocity = direction * 4;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<PlayerScriptLevel2>().knocked = false;
        }
    }
}
