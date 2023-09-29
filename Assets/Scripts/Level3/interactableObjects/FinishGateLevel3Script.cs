using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGateLevel3Script : MonoBehaviour
{
    private LogicManagerScriptLevel3 logicManagerScriptLevel3;
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScriptLevel3 = GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (logicManagerScriptLevel3.bossKilled)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (playerInRange)
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                logicManagerScriptLevel3.MoveToScene(3);
            }
        }
        else
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel3")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel3")
        {
            playerInRange = false;
        }
    }
}
