using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxLevel2Enemies2Script : MonoBehaviour
{
    public LogicManagerScriptLevel2 logicManagerScriptLevel2;
    public PlayerScriptLevel2 playerScriptLevel2;
    public AudioSource boxFallingSound;

    public float timeBeforeFall;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeFall = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(logicManagerScriptLevel2.nbOfEnemiesGrounded == 0)
        {
            timeBeforeFall -= Time.deltaTime;
        }
        if(timeBeforeFall <= 0)
        {
            GetComponent<Rigidbody2D>().gravityScale = 5000;
        }
    }

    //un box care cade cand inamicii au fost infranti, daca loveste playerul, acesta moare, cand loveste pamanetul dau play la un sound
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel2"))
        {
            if (Math.Abs(collision.gameObject.transform.position.x - transform.position.x) < 2 &&
                -collision.gameObject.transform.position.y + transform.position.y > 2) 
            {
                playerScriptLevel2.TakeDamage(100, transform.position);
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            //ca sa nu sune cand mor si ii setez pozitia cutii din prefs
            if(Math.Abs(PlayerPrefs.GetFloat("boxY") - 111.8f) < 1f)
            {
                boxFallingSound.Play();
            }
        }
    }
}
