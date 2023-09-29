using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox3Level3Script : MonoBehaviour
{

    private bool pushed;
    public AudioSource pushedSoundEffect;
    public AudioSource boxDestroyEffect;
    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        pushed = false;
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


    public void PushBox(Transform playerTransform)
    {
        if (!pushed)
        {
            pushed = true;
            pushedSoundEffect.Play();
            StartCoroutine(PushBoxC(playerTransform));
        }
    }

    private IEnumerator PushBoxC(Transform playerTransform)
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        Vector2 difference = new Vector2(transform.position.x - playerTransform.position.x, 0);
        difference = difference.normalized * 1800;
        GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Force); ;
        yield return new WaitForSeconds(1.8f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        pushed = false;
    }
    
    //este cutie care trebuie impinsa, am si functii de destroy pentru cazul in care cutia cade in tepi si trebuie distrusa
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
