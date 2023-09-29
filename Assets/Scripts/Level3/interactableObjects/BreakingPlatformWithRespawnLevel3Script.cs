using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakingPlatformWithRespawnLevel3Script : MonoBehaviour, IPlatformStrategy
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //tot destroy la o conditie
    public void BreakPlatform(PlatformScript platformScript)
    {
        StartCoroutine(DestroyFloorC(platformScript));
    }

    private IEnumerator DestroyFloorC(PlatformScript platformScript)
    {
        yield return new WaitForSeconds(0.5f);

        platformScript.wallDestroySound.Play();
        Instantiate(platformScript.explosionParticles, platformScript.explosionPosition.position, Quaternion.identity);
        //gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        platformScript.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        platformScript.gameObject.GetComponent<TilemapCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);
        //gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        platformScript.gameObject.GetComponent<TilemapRenderer>().enabled = true;
        platformScript.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
        platformScript.oneTime = true;

    }
}
