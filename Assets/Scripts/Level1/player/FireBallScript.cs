using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{

    Rigidbody2D fireballRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        fireballRigidBody= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotatia sa se faca in functie de velocity
        float angle = Mathf.Atan2(fireballRigidBody.velocity.y, fireballRigidBody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    //daca intra in coliziune cu un inamic din acest level, ii dau damage la acel inamic
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);

        if (collision.gameObject.CompareTag("EnemyAI"))
        {
            collision.gameObject.GetComponent<EnemyAIScript>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("EnemyAI2"))
        {
            collision.gameObject.GetComponent<EnemyAI2Script>().TakeDamage(1);
        }
    }
}
