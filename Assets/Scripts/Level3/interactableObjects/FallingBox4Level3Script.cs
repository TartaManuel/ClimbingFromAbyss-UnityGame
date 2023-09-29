using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox4Level3Script : MonoBehaviour
{

    public PlayerScriptLevel3 playerScriptLevel3;
    public AudioSource boxFallingSound;
    private bool firstTime;
    public bool connectedWithPushableBox;

    // Start is called before the first frame update
    //o cutie care ar trebui sa cada pe alta cutie albastra cu care sa se conecteze, dupa ce se conecteaza se va misca deodata cu acea cutie
    //daca il loveste in cadere pe jucator, il omoara
    void Start()
    {
        firstTime = true;
        connectedWithPushableBox = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLevel3") && !connectedWithPushableBox)
        {
            if (Math.Abs(collision.gameObject.transform.position.x - transform.position.x) < 4 &&
                -collision.gameObject.transform.position.y + transform.position.y > 2)
            {
                playerScriptLevel3.TakeDamage(100, transform.position);
            }
        }
        else if (collision.gameObject.CompareTag("Floor") && firstTime)
        {
            firstTime = false;
        }
        else if (collision.gameObject.CompareTag("Floor") && !firstTime)
        {
            boxFallingSound.Play();
        }
        else if (collision.gameObject.CompareTag("PushableBox1Level3"))
        {
            connectedWithPushableBox = true;
            boxFallingSound.Play();
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<Transform>().SetParent(collision.gameObject.transform);
        }
    }
}
