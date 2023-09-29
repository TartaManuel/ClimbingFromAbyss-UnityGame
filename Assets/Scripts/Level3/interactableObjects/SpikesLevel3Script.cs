using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLevel3Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spikes, daca jucatorul cade in ei, moare
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "PlayerLevel3")
        {
            collision.gameObject.GetComponent<PlayerScriptLevel3>().TakeDamage(1000, transform.position);
        }
    }
}
