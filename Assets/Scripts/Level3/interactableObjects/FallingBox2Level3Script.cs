using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox2Level3Script : MonoBehaviour
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
    //se activeaza gravitatia cand jucatorul bate un anumit inamic, si aici dupa un anumit timp
    void Update()
    {
        if(logicManagerScriptLevel3.enemyBoomerangKilled)
        {
            activateGravity = true;
        }
        if (activateGravity)
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
            //ca sa nu sune cand mor si ii setez pozitia cutii din prefs
            if (Math.Abs(PlayerPrefs.GetFloat("box1Y") - 22.3f) < 1f)
            {
                boxFallingSound.Play();
            }
        }
    }
}
