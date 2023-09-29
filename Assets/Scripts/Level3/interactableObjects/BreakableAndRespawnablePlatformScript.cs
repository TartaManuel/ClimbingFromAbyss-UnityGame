using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableAndRespawnablePlatformScript : MonoBehaviour
{

    public GameObject explosionParticles;
    public AudioSource wallDestroySound;
    public Transform explosionPosition;
    private bool oneTime;

    // Start is called before the first frame update
    void Start()
    {
        oneTime = true;

        if (PlayerPrefs.GetInt("checkpointReachedLevel3") == 2)
        {
            StartCoroutine(DestroyPlatformForCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BoomerangSpawn(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang1").GetComponent<EnemyBoomerangSpawned1Script>().returnedBoomerang = true;
        }
    }

    //functii de destroy on collision si restore dupa un anumit timp cand se indeplineste o anumita conditie
    public void destroyFloor()
    {
        if (oneTime)
        {
            oneTime = false;
            wallDestroySound.Play();
            Instantiate(explosionParticles, explosionPosition.position, Quaternion.identity);
            //gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
            gameObject.GetComponent<TilemapRenderer>().enabled = false;
            gameObject.GetComponent<TilemapCollider2D>().enabled = false;
        }
    }

    public void restoreFloor()
    {
        gameObject.GetComponent<TilemapRenderer>().enabled = true;
        gameObject.GetComponent<TilemapCollider2D>().enabled = true;
        oneTime = true;
    }

    private IEnumerator DestroyPlatformForCheckpoint()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
