using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox1Level3Script : MonoBehaviour
{
    public LogicManagerScriptLevel3 logicManagerScriptLevel3;
    public PlayerScriptLevel3 playerScriptLevel3;
    public AudioSource boxFallingSound;

    public float timeBeforeFall;
    public bool activateGravity;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeFall = 0.5f;
        activateGravity = false;
    }

    // Update is called once per frame
    //cutie la care ii activez gravitatia cand o lovesc cu sabia, dar se activeaza doar dupa un timp ca sa imi dea sansa sa trec de ea
    void Update()
    {
        if(activateGravity)
        {
            timeBeforeFall -= Time.deltaTime;
        }

        if (timeBeforeFall <= 0)
        {
            GetComponent<Rigidbody2D>().gravityScale = 2000;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel3"))
        {
            if (Math.Abs(collision.gameObject.transform.position.x - transform.position.x) < 2 &&
                -collision.gameObject.transform.position.y + transform.position.y > 2)
            {
                playerScriptLevel3.TakeDamage(100, transform.position);
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            boxFallingSound.Play();
        }
    }
}
