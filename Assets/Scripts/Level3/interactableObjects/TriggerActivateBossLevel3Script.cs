using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateBossLevel3Script : MonoBehaviour
{
    public GameObject bossLevel3;
    private bool onlyOnce;
    public LogicManagerScriptLevel3 logicManagerScriptLevel3;

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
        if (collision.name == "PlayerLevel3")
        {
            if (onlyOnce)
            {
                logicManagerScriptLevel3.PlayFightMusic();
                bossLevel3.GetComponentInChildren<BossLevel3Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel3")
        {
            if (onlyOnce)
            {
                bossLevel3.GetComponentInChildren<BossLevel3Script>().startPath = true;
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
            player.GetComponent<PlayerScriptLevel3>().knocked = true;
            player.GetComponent<Rigidbody2D>().velocity = direction * 4;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<PlayerScriptLevel3>().knocked = false;
        }
    }
}
