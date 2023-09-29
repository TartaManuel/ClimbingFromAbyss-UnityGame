using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox2Level3Script : MonoBehaviour
{

    private bool pushed;
    public AudioSource pushedSoundEffect;
    public GameObject fallingBox;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        Vector2 difference = new Vector2(transform.position.x - playerTransform.position.x, 0);
        difference = difference.normalized * 30;
        GetComponent<Rigidbody2D>().velocity = difference;

        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        pushed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            //GetComponent<Rigidbody2D>().isKinematic= true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && !pushed)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
