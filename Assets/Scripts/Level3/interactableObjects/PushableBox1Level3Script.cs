using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox1Level3Script : MonoBehaviour
{
    // Start is called before the first frame update
    private bool pushed;
    public AudioSource pushedSoundEffect;
    void Start()
    {
        pushed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //functii de impingere cutiei
    public void PushBox(Transform playerTransform)
    {
        if(!pushed)
        {
            pushed = true;
            pushedSoundEffect.Play();
            StartCoroutine(PushBoxC(playerTransform));
        }
    }

    private IEnumerator PushBoxC(Transform playerTransform)
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        Vector2 difference = new Vector2(transform.position.x - playerTransform.position.x, 0);
        difference = difference.normalized * 1000;
        GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Force);;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        pushed = false;
    }
}
