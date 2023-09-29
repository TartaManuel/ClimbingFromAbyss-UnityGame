using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGateLevel2Script : MonoBehaviour
{
    private LogicManagerScriptLevel2 logicManagerScriptLevel2;
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //gate pentru finalul nivelului, se activeaza cand bossul a fost batut, si playerul poate apasa E ca sa termine nivelul
    // Update is called once per frame
    void Update()
    {
        if (logicManagerScriptLevel2.bossKilled)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (playerInRange)
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                logicManagerScriptLevel2.MoveToScene(3);
            }
        }
        else
        {
            gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel2")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerLevel2")
        {
            playerInRange = false;
        }
    }
}
