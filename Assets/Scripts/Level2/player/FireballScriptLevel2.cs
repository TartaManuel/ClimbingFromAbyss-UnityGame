using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScriptLevel2 : MonoBehaviour
{

    Rigidbody2D fireballRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        fireballRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float angle = Mathf.Atan2(fireballRigidBody.velocity.y, fireballRigidBody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);

        if (collision.gameObject.CompareTag("EnemyAILevel2"))
        {
            collision.gameObject.GetComponent<EnemyAILevel2Script>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("EnemyAI2Level2"))
        {
            collision.gameObject.GetComponent<EnemyAI2Level2Script>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("EnemyGrounded1"))
        {
            collision.gameObject.GetComponent<EnemyGrounded1Script>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("EnemyGrounded2"))
        {
            collision.gameObject.GetComponent<EnemyGrounded2Script>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("FireballLevel2"))
        {
            GameObject.FindGameObjectWithTag("BossLevel2").GetComponent<BossLevel2Script>().TakeDamage(1);
        }
    }
}
