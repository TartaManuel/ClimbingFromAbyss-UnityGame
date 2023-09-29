using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChestScript : MonoBehaviour
{
    public GameObject tutorialChest;

    //script logic manager pentru pause and gameover check
    private LogicManagerScript logicManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScript = GameObject.FindGameObjectWithTag("LogicLevel1").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            tutorialChest.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            tutorialChest.SetActive(false);
        }
    }
}
