using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBoxLevel2Script : MonoBehaviour
{

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource boxDestroySound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //functia care doar apeleaza corutina. Corutina da play la un sunet, face explozia pentru destroy si dupa un timp distruge obiectul
    public void DestroyBox()
    {
        StartCoroutine(DestroyBoxC());
    }

    private IEnumerator DestroyBoxC()
    {
        boxDestroySound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
