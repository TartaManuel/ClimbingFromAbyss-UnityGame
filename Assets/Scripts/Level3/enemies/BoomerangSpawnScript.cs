using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawnScript : MonoBehaviour
{

    Rigidbody2D boomerangRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource boomerangCollisionSoundEffect;
    public bool pausedSound;

    // Start is called before the first frame update
    void Start()
    {
        boomerangRigidBody = GetComponent<Rigidbody2D>();
        pausedSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        //la fel ca si boomeeang, dar e folosit de alt inamic
        if (GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawnerScript>().enemyToSpawnBoomerang.active == false)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
        if (GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().pausedGame && !pausedSound)
        {
            boomerangCollisionSoundEffect.Pause();
            pausedSound = true;
        }
        if (!GameObject.FindGameObjectWithTag("LogicLevel3").GetComponent<LogicManagerScriptLevel3>().pausedGame && pausedSound)
        {
            boomerangCollisionSoundEffect.UnPause();
            pausedSound = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag("PlayerLevel3"))
        {
            collision.gameObject.GetComponent<PlayerScriptLevel3>().TakeDamage(1, transform.position);
        }

        StartCoroutine(DestroyBoomerang());
    }

    private IEnumerator DestroyBoomerang()
    {
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            boomerangCollisionSoundEffect.Play();
        }
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
