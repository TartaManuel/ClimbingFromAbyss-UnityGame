using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatformsLevel3Script : MonoBehaviour, IPlatformStrategy
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
        yield return new WaitForSeconds(0.3f);

        platformScript.wallDestroySound.Play();
        Instantiate(platformScript.explosionParticles, platformScript.explosionPosition.position, Quaternion.identity);
        platformScript.gameObject.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(1f);

        platformScript.gameObject.SetActive(false);
    }
}
