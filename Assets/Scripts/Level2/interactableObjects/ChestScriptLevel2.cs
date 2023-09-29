using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScriptLevel2 : MonoBehaviour
{
    public Sprite chestOpenend;
    public bool isOpened = false;

    public AudioSource chestHitSound;
    public PlayerScriptLevel2 playerScriptLevel2;
    public GameObject healthBoostIcon;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //chestul care ofera bonus la viata pentru caracter
    public void GetHit()
    {
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            chestHitSound.Play();
            isOpened = true;

            healthBoostIcon.SetActive(true);
            playerScriptLevel2.maxHealth = playerScriptLevel2.maxHealth + 2;
            playerScriptLevel2.health = playerScriptLevel2.health + 2;
            playerScriptLevel2.hpBar.maxValue = playerScriptLevel2.hpBar.maxValue + 2;
            playerScriptLevel2.hpBar.value = playerScriptLevel2.hpBar.value + 2;
        }
    }

    public void GetHitNoSound()
    {
        if (!isOpened)
        {
            StartCoroutine(GetHitNoSoundC());
        }
    }

    //face acelasi lucru, doar ca se activeaza la inceputul nivelului pentru checkpoint, deci vreau fara sunet
    public IEnumerator GetHitNoSoundC()
    {
        yield return new WaitForSeconds(0.2f);
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            isOpened = true;

            healthBoostIcon.SetActive(true);
            playerScriptLevel2.maxHealth = playerScriptLevel2.maxHealth + 2;
            playerScriptLevel2.health = playerScriptLevel2.maxHealth;
            playerScriptLevel2.hpBar.maxValue = playerScriptLevel2.hpBar.maxValue + 2;
            playerScriptLevel2.hpBar.value = playerScriptLevel2.hpBar.maxValue;
        }
    }
}
