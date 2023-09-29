using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeWeaponScriptLevel2 : MonoBehaviour
{
    private PlayerScriptLevel2 playerScriptLevel2;
    public GameObject fireball;
    public float speed;
    public Transform shotPoint;

    private float intervalBetweenShots;
    private float timeRemainingToShot;

    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;

    public bool aimNeeded;

    //bool cand lovesc chestul sa am acces la weapon ranged
    public bool achievedRangedWeapon;

    //imagine ca sa arat ca este on cooldown
    public Image onCooldownImage;

    //sound effect range attack
    public AudioSource rangeWeaponSoundEffect;

    //script logic manager pentru pause and gameover check
    private LogicManagerScriptLevel2 logicManagerScriptLevel2;

    //icon pentru range attack, acuma nu o mai iau din chest, ci o am daca o am din nivelul precedent
    public GameObject rangeAttackIcon;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScriptLevel2 = GameObject.FindGameObjectWithTag("LogicLevel2").GetComponent<LogicManagerScriptLevel2>();

        playerScriptLevel2 = GetComponentInParent<PlayerScriptLevel2>();
        intervalBetweenShots = 1.5f;
        timeRemainingToShot = 0f;

        aimNeeded = false;
        achievedRangedWeapon = (PlayerPrefs.GetInt("RangeWeapon") != 0);

        if(achievedRangedWeapon)
        {
            rangeAttackIcon.SetActive(true);
        }

        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemainingToShot > 0)
        {
            timeRemainingToShot = timeRemainingToShot - Time.deltaTime;
            onCooldownImage.enabled = true;
        }
        else
        {
            onCooldownImage.enabled = false;
        }

        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - weaponPosition;

        //in functie de orientarea jucatorului, directia si viteza pot fi diferie
        if (playerScriptLevel2.rightOriented)
        {
            transform.right = direction;
            speed = 30f;
        }
        else
        {
            transform.right = -direction;
            speed = -30f;
        }

        if (achievedRangedWeapon && !logicManagerScriptLevel2.pausedGame && !logicManagerScriptLevel2.gameIsOver)
        {
            if (aimNeeded == false && Input.GetKeyDown(KeyCode.LeftShift))
            {
                aimNeeded = true;
            }
            else if (aimNeeded == true && Input.GetKeyDown(KeyCode.LeftShift))
            {
                aimNeeded = false;
            }

            if (aimNeeded)
            {
                if (Input.GetMouseButtonDown(1) && timeRemainingToShot <= 0)
                {
                    ShootFireball();
                    timeRemainingToShot = intervalBetweenShots;
                }

                for (int i = 0; i < numberOfPoints; i++)
                {
                    points[i].transform.position = PointPosition(i * spaceBetweenPoints);
                    points[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < numberOfPoints; i++)
                {
                    points[i].SetActive(false);
                }
            }
        }
    }

    //functia in care instantiez fireballul si ii dau velocity in directia setata mai sus
    void ShootFireball()
    {
        rangeWeaponSoundEffect.Play();
        GameObject newFireball = Instantiate(fireball, shotPoint.position, shotPoint.rotation);
        newFireball.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(newFireball, 5f);
    }

    //functia pentru calcularea pozitiei acelor puncte
    Vector2 PointPosition(float t)
    {
        if (playerScriptLevel2.rightOriented)
        {
            Vector2 position = (Vector2)shotPoint.position + (direction.normalized * speed * t) + 0.5f * Physics2D.gravity * (t * t);
            return position;
        }
        else
        {
            Vector2 position = (Vector2)shotPoint.position + (-direction.normalized * speed * t) + 0.5f * Physics2D.gravity * (t * t);
            return position;
        }
    }
}
