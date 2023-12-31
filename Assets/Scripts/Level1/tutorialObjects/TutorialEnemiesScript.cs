using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemiesScript : MonoBehaviour
{
    public GameObject tutorialEnemies;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            tutorialEnemies.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            tutorialEnemies.SetActive(false);
        }
    }
}
