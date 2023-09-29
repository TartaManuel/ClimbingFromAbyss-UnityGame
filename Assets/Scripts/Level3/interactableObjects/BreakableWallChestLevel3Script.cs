using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallChestLevel3Script : MonoBehaviour
{
    public GameObject explosionParticles;

    public AudioSource wallDestroySound;

    public GameObject chest2Level3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //destroy wall la o anumita conditie
    public void destroyWall()
    {
        StartCoroutine(DestroyWall());
    }

    private IEnumerator DestroyWall()
    {
        wallDestroySound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.3f);
        chest2Level3.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }

    private IEnumerator DestroyWallIfCollected()
    {

        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
    }
}
