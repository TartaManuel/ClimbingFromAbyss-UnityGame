using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest3ScriptLevel3 : MonoBehaviour
{

    public Sprite chestOpenend;
    public bool isOpened = false;

    public AudioSource chestHitSound;
    public WeaponScriptLevel3 weaponScriptLevel3;
    public GameObject stunIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit()
    {
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            chestHitSound.Play();
            isOpened = true;
            stunIcon.SetActive(true);
            weaponScriptLevel3.stunPowerReceived = true;
        }
    }

    public void GetHitNoSound()
    {
        if (!isOpened)
        {
            StartCoroutine(GetHitNoSoundC());
        }
    }

    public IEnumerator GetHitNoSoundC()
    {
        yield return new WaitForSeconds(0.2f);
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            isOpened = true;

            stunIcon.SetActive(true);
            weaponScriptLevel3.stunPowerReceived = true;
        }
    }
}
