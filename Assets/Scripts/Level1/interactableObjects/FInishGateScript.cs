using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FInishGateScript : MonoBehaviour
{
    private LogicManagerScript logicManagerScript;
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScript = GameObject.FindGameObjectWithTag("LogicLevel1").GetComponent<LogicManagerScript>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //aici cand inamicii au fost infranti, verific in logicmanager asta si daca este adevarat activez aceasta poarta
    // Update is called once per frame
    void Update()
    {
        if(logicManagerScript.nbOfEnemies == 0) 
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if(playerInRange)
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                logicManagerScript.MoveToScene(3);
            }
        }
        else
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInRange = false;
        }
    }
}
