using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBossLevel2Script : MonoBehaviour
{
    Rigidbody2D fireballRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    //sunet on collision
    public AudioSource fireballCollisionSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        fireballRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void Update()
    {

        float angle = Mathf.Atan2(fireballRigidBody.velocity.y, fireballRigidBody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

        //verific daca bossul a fost infrant, si daca a fost, dau destroy la fireball ca jucatorul sa nu mai ia damage dupa ce lupta s-a incheiat
        if (GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>().bossKilled == true)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DestroyFireball());

        if (collision.gameObject.CompareTag("FireballBossLevel2"))
        {
            GameObject.FindGameObjectWithTag("PlayerLevel2").GetComponent<PlayerScriptLevel2>().TakeDamage(1, transform.position);
        }
    }

    //functie de destroy on collision, cu sunet si particles
    private IEnumerator DestroyFireball()
    {
        if(GetComponent<SpriteRenderer>().isVisible) 
        {
            fireballCollisionSoundEffect.Play();
        }
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
