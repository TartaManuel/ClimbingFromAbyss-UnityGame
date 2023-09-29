using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox8Level3Script : MonoBehaviour
{

    public AudioSource boxDestroyEffect;
    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TilemapSpikes")
        {
            DestroyBox();
        }
    }

    public void DestroyBox()
    {
        StartCoroutine(DestroyBoxC());
    }

    private IEnumerator DestroyBoxC()
    {
        boxDestroyEffect.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
