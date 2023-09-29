using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeWeaponScript : MonoBehaviour
{
    private PlayerScript playerScript;
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
    private LogicManagerScript logicManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        logicManagerScript = GameObject.FindGameObjectWithTag("LogicLevel1").GetComponent<LogicManagerScript>();

        playerScript = GetComponentInParent<PlayerScript>();
        intervalBetweenShots = 2f;
        timeRemainingToShot = 0f;

        aimNeeded = false;
        achievedRangedWeapon = false;

        points = new GameObject[numberOfPoints];
        for(int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemainingToShot > 0)
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

        if(playerScript.rightOriented)
        {
            transform.right = direction;
            speed = 30f;
        }
        else
        {
            transform.right = -direction;
            speed = -30f;
        }
        if(achievedRangedWeapon && !logicManagerScript.pausedGame && !logicManagerScript.gameIsOver)
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

    //functia care incepe atacul range, creez un fireball, ii dau velocity
    void ShootFireball()
    {
        rangeWeaponSoundEffect.Play();
        GameObject newFireball = Instantiate(fireball, shotPoint.position, shotPoint.rotation);
        newFireball.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(newFireball, 5f);
    }

    //functie pentru acele puncte care arata unde tintesc inainte sa activez atacul range
    Vector2 PointPosition(float t)
    {
        if (playerScript.rightOriented)
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
