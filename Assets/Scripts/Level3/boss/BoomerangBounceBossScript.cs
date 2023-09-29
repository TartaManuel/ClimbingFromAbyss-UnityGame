using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBounceBossScript : MonoBehaviour
{
    Rigidbody2D boomerangRigidBody;

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource boomerangCollisionSoundEffect;
    public bool pausedSound;
    private float timeBetweenCollisions;
    private float timeActual;

    // Start is called before the first frame update
    void Start()
    {
        boomerangRigidBody = GetComponent<Rigidbody2D>();
        pausedSound = false;
        timeBetweenCollisions = 0.2f;
        timeActual = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("BossLevel3").GetComponent<BossLevel3Script>().isDead == true)
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
        if(timeActual> 0)
        {
            timeActual -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Instantiate(explosionParticles, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag("PlayerLevel3"))
        {
            collision.gameObject.GetComponent<PlayerScriptLevel3>().TakeDamage(1, transform.position);
        }

        if(timeActual <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * (-1);
            timeActual = timeBetweenCollisions;
        }
    }

    private IEnumerator DestroyBoomerangC()
    {
        //if (GetComponent<SpriteRenderer>().isVisible)
        //{
        //    boomerangCollisionSoundEffect.Play();
        //}
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    public void DestroyBoomerang()
    {
        StartCoroutine(DestroyBoomerangC());
    }
}
