using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHeckpoint2Level3Script : MonoBehaviour
{
    private bool playerInRange = false;
    public Sprite checkpointSaved;
    private bool onlyOnce;
    public CheckpointCanvasLevel2Script checkpointCanvasLevel2Script;
    private LogicManagerScriptLevel3 logicManagerScriptLevel3;

    // Start is called before the first frame update
    void Start()
    {
        onlyOnce = true;
        logicManagerScriptLevel3 = GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onlyOnce)
        {
            if (playerInRange)
            {
                gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<SpriteRenderer>().sprite = checkpointSaved;
                    onlyOnce = false;
                    gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = false;
                    checkpointCanvasLevel2Script.ActivateCheckpointAnimation();
                    logicManagerScriptLevel3.SetPlayerPrefsForCheckpoint2();
                }
            }
            else
            {
                gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().enabled = false;
            }
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
