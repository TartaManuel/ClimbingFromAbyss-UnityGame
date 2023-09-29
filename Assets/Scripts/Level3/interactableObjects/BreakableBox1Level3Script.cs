using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox1Level3Script : MonoBehaviour
{

    //effect de explosion on hit
    public GameObject explosionParticles;

    public AudioSource boxDestroySound;

    public FallingBox1Level3Script fallingBox1Level3Script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //destroy box la o anumita conditie
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
        fallingBox1Level3Script.activateGravity = true;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
