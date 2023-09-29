using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLevel3Script : MonoBehaviour
{
    Rigidbody2D fireballRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    //sunet on collision
    public AudioSource fireballCollisionSoundEffect;
    public bool pausedSound;

    // Start is called before the first frame update
    void Start()
    {
        fireballRigidBody = GetComponent<Rigidbody2D>();
        pausedSound = false;
    }

    // Update is called once per frame
    void Update()
    {

        float angle = Mathf.Atan2(fireballRigidBody.velocity.y, fireballRigidBody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

        if (GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().enemyBoomerangKilled == true)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
        if (GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().pausedGame && !pausedSound)
        {
            fireballCollisionSoundEffect.Pause();
            pausedSound = true;
        }
        if (!GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().pausedGame && pausedSound)
        {
            fireballCollisionSoundEffect.UnPause();
            pausedSound = false;
        }
    }

    //fireball de la inamici, daca lovesc jucatorul ii dau damage
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerLevel3"))
        {
            //EnemyAIScript enemyAIScript = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAIScript>();
            GameObject.FindGameObjectWithTag("PlayerLevel3").GetComponent<PlayerScriptLevel3>().TakeDamage(1, transform.position);
        }

        StartCoroutine(DestroyFireball());
    }

    //destroy la coliziune
    private IEnumerator DestroyFireball()
    {
        if (GetComponent<SpriteRenderer>().isVisible)
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
