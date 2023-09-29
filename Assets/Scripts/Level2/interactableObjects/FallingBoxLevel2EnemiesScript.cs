using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxLevel2EnemiesScript : MonoBehaviour
{

    public AudioSource boxFallingSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //functie la care ii activez gravitatia din alt script, cand se intampla ceva, de ex cand o lovesc
    public void SetGravity()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 2000;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            boxFallingSound.Play();
        }
    }
}
