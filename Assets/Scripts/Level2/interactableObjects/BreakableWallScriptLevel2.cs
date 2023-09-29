using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScriptLevel2 : MonoBehaviour
{

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource wallDestroySound;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("CollectedChestsLevel2") != 0)
        {
            StartCoroutine(DestroyWallIfCollected());

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //ca si la alte obiecte
    public void destroyWall()
    {
        StartCoroutine(DestroyWall());
    }

    private IEnumerator DestroyWall()
    {
        wallDestroySound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0,0,0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }


    //ideea de checkpoint, daca puterea a fost colectata deja, distrug zidul ca sa nu mai trebuiasca sa o faca playerul din nou
    private IEnumerator DestroyWallIfCollected()
    {

        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
    }
}
