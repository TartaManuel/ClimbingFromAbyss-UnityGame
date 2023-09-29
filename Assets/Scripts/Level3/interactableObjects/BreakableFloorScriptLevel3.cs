using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloorScriptLevel3 : MonoBehaviour
{

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource wallDestroySound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //destroy floor la o anumita conditie
    public void destroyFloor()
    {
        StartCoroutine(DestroyFloorC());
    }

    private IEnumerator DestroyFloorC()
    {
        yield return new WaitForSeconds(0.3f);

        wallDestroySound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "PlayerLevel3")
        {
            destroyFloor();
        }
    }
}
