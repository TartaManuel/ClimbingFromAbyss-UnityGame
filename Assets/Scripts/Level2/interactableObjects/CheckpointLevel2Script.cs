using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointLevel2Script : MonoBehaviour
{
    private bool playerInRange = false;
    public Sprite checkpointSaved;
    private bool onlyOnce;
    public CheckpointCanvasLevel2Script checkpointCanvasLevel2Script;
    private LogicManagerScriptLevel2 logicManagerScriptLevel2;
    // Start is called before the first frame update
    void Start()
    {
        onlyOnce = true;
        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();
    }

    //acesta este scriptul pentru checkpointul in sine, daca playerul este aproape poate apasa o tasta care sa salveze progresul
    // Update is called once per frame
    void Update()
    {
        if(onlyOnce)
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
                    logicManagerScriptLevel2.SetPlayerPrefsForCheckpoint();
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
