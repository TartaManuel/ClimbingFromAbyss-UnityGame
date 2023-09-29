using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateEnemiesGroundedScript : MonoBehaviour
{
    public GameObject enemyGrounded1;
    public GameObject enemyGrounded2;
    private bool onlyOnce;

    // Start is called before the first frame update
    void Start()
    {
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
            if (onlyOnce)
            {
                enemyGrounded1.GetComponentInChildren<EnemyGrounded1Script>().startPath = true;
                enemyGrounded2.GetComponentInChildren<EnemyGrounded2Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel2")
        {

            if (onlyOnce)
            {
                enemyGrounded1.GetComponentInChildren<EnemyGrounded1Script>().startPath = true;
                enemyGrounded2.GetComponentInChildren<EnemyGrounded2Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    public IEnumerator PushOnTrigger(GameObject player)
    {
        Vector2 direction = Vector2.right * 10;

        if ((Math.Abs(player.GetComponent<Rigidbody2D>().velocity.x) < Math.Abs(direction.x)) || player.GetComponent<Rigidbody2D>().velocity.x < 0)
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
