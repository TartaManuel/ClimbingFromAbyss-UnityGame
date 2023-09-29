using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEnemiesScript : MonoBehaviour
{

    public GameObject explosionParticles;
    public AudioSource wallOpenSound;
    private bool oneTime;

    // Start is called before the first frame update
    void Start()
    {
        oneTime = true;

        if (PlayerPrefs.GetInt("checkpointReachedLevel3") == 2)
        {
            StartCoroutine(DestroyWallForCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawnerScript>().isDead && oneTime)
        {
            oneTime = false;
            StartCoroutine(DestroyWall());
        }
    }

    private IEnumerator DestroyWall()
    {
        wallOpenSound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BoomerangSpawn(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang1").GetComponent<EnemyBoomerangSpawned1Script>().returnedBoomerang = true;
        }
    }

    private IEnumerator DestroyWallForCheckpoint()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
