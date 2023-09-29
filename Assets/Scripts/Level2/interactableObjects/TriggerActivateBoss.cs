using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateBoss : MonoBehaviour
{
    public GameObject bossLevel2;
    private bool onlyOnce;

    //sunet on collision
    public AudioSource bossFightLevel2SoundEffect;

    private LogicManagerScriptLevel2 logicManagerScriptLevel2;

    // Start is called before the first frame update
    void Start()
    {
        onlyOnce = true;
        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();

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
                logicManagerScriptLevel2.PlayFightMusic();
                bossLevel2.GetComponentInChildren<BossLevel2Script>().startPath = true;
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
                logicManagerScriptLevel2.PlayFightMusic();
                bossLevel2.GetComponentInChildren<BossLevel2Script>().startPath = true;
                StartCoroutine(PushOnTrigger(collision.gameObject));
            }
        }
    }

    public IEnumerator PushOnTrigger(GameObject player)
    {
        Vector2 direction = Vector2.right * 10;
        onlyOnce = false;
        Debug.Log(onlyOnce);

        if ((Math.Abs(player.GetComponent<Rigidbody2D>().velocity.x) < Math.Abs(direction.x)) || player.GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            player.GetComponent<PlayerScriptLevel2>().knocked = true;
            player.GetComponent<Rigidbody2D>().velocity = direction * 4;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<PlayerScriptLevel2>().knocked = false;
        }
    }
}
