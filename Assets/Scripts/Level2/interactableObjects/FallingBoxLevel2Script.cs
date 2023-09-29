using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxLevel2Script : MonoBehaviour
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

    //o idee asemanatoare, un box care cade, doar ca este activat diferit, cand playerul loveste cutia cu sabia
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
