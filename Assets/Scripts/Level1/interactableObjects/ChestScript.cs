using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite chestOpenend;
    private bool isOpened = false;

    public AudioSource chestHitSound;
    public GameObject rangeAttackIcon;

    void Start()
    {
        if (PlayerPrefs.GetInt("CollectedChestsLevel1") != 0)
        {
            StartCoroutine(AchievedWeaponPreviousRun());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //cand lovesc chestul cu sabia apelez aceasta functie care da play la un sunet, si seteaza puterea activa
    public void GetHit()
    {
        if(!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            chestHitSound.Play();
            isOpened = true;
            rangeAttackIcon.SetActive(true);
            RangeWeaponScript rangeWeaponScript = GameObject.FindGameObjectWithTag("RangedWeapon").GetComponent<RangeWeaponScript>();
            rangeWeaponScript.achievedRangedWeapon = true;
        }
    }

    //aceeasi functie ca mai sus doar ca o apelez la checkpoint, deci nu vreau sunet
    public void GetHitNoSound()
    {
        if (!isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = chestOpenend;
            isOpened = true;
            rangeAttackIcon.SetActive(true);
            RangeWeaponScript rangeWeaponScript = GameObject.FindGameObjectWithTag("RangedWeapon").GetComponent<RangeWeaponScript>();
            rangeWeaponScript.achievedRangedWeapon = true;
            Debug.Log(rangeWeaponScript.achievedRangedWeapon);
        }
    }

    private IEnumerator AchievedWeaponPreviousRun()
    {

        yield return new WaitForSeconds(0.2f);

        GetHitNoSound();
    }
}
