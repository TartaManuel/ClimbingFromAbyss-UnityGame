using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest1ScriptLevel3 : MonoBehaviour
{

    public Sprite chestOpenend;
    public bool isOpened = false;

    public AudioSource chestHitSound;
    public PlayerScriptLevel3 playerScriptLevel3;
    public GameObject shieldIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //functii de lovit chest, cu sunet in modul normal, fara sunet in caz de checkpoint
    public void GetHit()
    {
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            chestHitSound.Play();
            isOpened = true;

            shieldIcon.SetActive(true);
            playerScriptLevel3.ActivateShieldPower();
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

            shieldIcon.SetActive(true);
            playerScriptLevel3.ActivateShieldPower();
        }
    }
}
