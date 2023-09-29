using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallEnemiesScriptLevel2 : MonoBehaviour
{

    //effect de explosion on hit
    public GameObject explosionParticles;
    public LogicManagerScriptLevel2 logicManagerScriptLevel2;

    public AudioSource wallDestroySound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroyWall()
    {
        if(logicManagerScriptLevel2.nbOfEnemies == 0)
        {
            StartCoroutine(DestroyWall());
        }
    }

    private IEnumerator DestroyWall()
    {
        wallDestroySound.Play();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
