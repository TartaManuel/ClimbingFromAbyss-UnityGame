using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBossLevel3Script : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject boomerang;
    public GameObject boomerangBounce;
    public float speed;
    public float speedSecondAttack;
    public Transform shotPoint;
    public Transform shotBehind;
    public Transform shotBelow;
    public Transform shotAbove;
    public Transform shotNW;
    public Transform shotNE;
    public Transform shotSW;
    public Transform shotSE;
    public BossLevel3Script bossLevel3Script;

    Vector2 direction;

    public bool rightOriented;


    //sound effect range attack
    public AudioSource rangeWeaponSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        speedSecondAttack = 25f;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //range weapon pentru inamicul cu boomerang, are functii pentru aruncarea boomerangului
    public void ShootBoomerang()
    {
        StartCoroutine(Shoot());
    }

    public void ShootBoomerangFourDirections()
    {
        StartCoroutine(ShootFourDirection());
    }

    public void ShootBoomerangFourDirectionsDiagonal()
    {
        StartCoroutine(ShootFourDirectionDiagonal());
    }

    public void ShootBoomerangEightDirections()
    {
        StartCoroutine(ShootEightDirections());
    }

    public IEnumerator Shoot()
    {
        bossLevel3Script.returnedBoomerang = false;
        GameObject boomerang1 = Instantiate(boomerang, shotPoint.position, shotPoint.rotation);

        Vector2 projectilePosition2 = shotPoint.position + new Vector3(0, 0.3f, 0);
        Vector2 initialPosition = projectilePosition2;
        direction = new Vector2(playerTransform.position.x, playerTransform.position.y) - projectilePosition2;

        if (rightOriented)
        {
            transform.right = direction;
            speed = 30f;
        }
        else
        {
            transform.right = -direction;
            speed = -30f;
        }

        rangeWeaponSoundEffect.Play();
        if (boomerang1 != null)
        {
            boomerang1.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
        else
        {
            StopCoroutine(Shoot());
        }
        projectilePosition2 = boomerang1.transform.position;

        float distanceToPlayer = Math.Abs(playerTransform.position.x - projectilePosition2.x);

        if (rightOriented)
        {
            while (playerTransform.position.x > projectilePosition2.x)
            {
                yield return new WaitForSeconds(0.1f);
                if (boomerang1 != null)
                {
                    projectilePosition2 = boomerang1.transform.position;
                }
                else
                {
                    StopCoroutine(Shoot());
                }
            }
        }
        else
        {
            while (playerTransform.position.x < projectilePosition2.x)
            {
                yield return new WaitForSeconds(0.1f);
                if (boomerang1 != null)
                {
                    projectilePosition2 = boomerang1.transform.position;
                }
                else
                {
                    StopCoroutine(Shoot());
                }
            }
        }

        distanceToPlayer = Math.Abs(playerTransform.position.x - projectilePosition2.x);

        while (distanceToPlayer < 6)
        {
            if (boomerang1 != null)
            {
                projectilePosition2 = boomerang1.transform.position;
            }
            else
            {
                StopCoroutine(Shoot());
            }
            distanceToPlayer = Math.Abs(playerTransform.position.x - projectilePosition2.x);
            yield return new WaitForSeconds(0.1f);
        }
        if (boomerang1 != null)
        {
            boomerang1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            StopCoroutine(Shoot());
        }
        yield return new WaitForSeconds(0.2f);

        float distanceToEnemy = 0;
        if (boomerang1 != null)
        {
            distanceToEnemy = Math.Abs(boomerang1.transform.position.x - initialPosition.x);
        }
        else
        {
            StopCoroutine(Shoot());
        }
        while (distanceToEnemy > 1.5)
        {
            direction = new Vector2(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y) - projectilePosition2;

            if (boomerang1 != null)
            {
                boomerang1.GetComponent<Rigidbody2D>().velocity = direction;
                distanceToEnemy = Math.Abs(boomerang1.transform.position.x - initialPosition.x);
            }
            else
            {
                StopCoroutine(Shoot());
            }
            yield return new WaitForSeconds(0.1f);
        }
        bossLevel3Script.returnedBoomerang = true;
        Destroy(boomerang1);
    }

    private IEnumerator ShootFourDirection()
    {
        GameObject boomerang1 = Instantiate(boomerangBounce, shotPoint.position, shotPoint.rotation);
        GameObject boomerang2 = Instantiate(boomerangBounce, shotBehind.position, shotBehind.rotation);
        GameObject boomerang3 = Instantiate(boomerangBounce, shotAbove.position, shotAbove.rotation);
        GameObject boomerang4 = Instantiate(boomerangBounce, shotBelow.position, shotBelow.rotation);
        yield return new WaitForSeconds(0.3f);

        rangeWeaponSoundEffect.Play();
        boomerang1.GetComponent<Rigidbody2D>().velocity = Vector2.right * (rightOriented ? 1:-1) * speedSecondAttack;
        boomerang2.GetComponent<Rigidbody2D>().velocity = Vector2.right * (rightOriented ? -1 : 1) * speedSecondAttack;
        boomerang3.GetComponent<Rigidbody2D>().velocity = Vector2.up * speedSecondAttack;
        boomerang4.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;

        yield return new WaitForSeconds(4f);
        boomerang1.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang2.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang3.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang4.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
    }
    private IEnumerator ShootFourDirectionDiagonal()
    {
        GameObject boomerang1 = Instantiate(boomerangBounce, shotNW.position, shotNW.rotation);
        GameObject boomerang2 = Instantiate(boomerangBounce, shotNE.position, shotNE.rotation);
        GameObject boomerang3 = Instantiate(boomerangBounce, shotSW.position, shotSW.rotation);
        GameObject boomerang4 = Instantiate(boomerangBounce, shotSE.position, shotSE.rotation);
        yield return new WaitForSeconds(0.3f);

        rangeWeaponSoundEffect.Play();
        boomerang1.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? -1 : 1) + Vector2.up) * speedSecondAttack;
        boomerang2.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? 1 : -1) + Vector2.up) * speedSecondAttack;
        boomerang3.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? -1 : 1) + Vector2.down) * speedSecondAttack;
        boomerang4.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? 1 : -1) + Vector2.down) * speedSecondAttack;

        yield return new WaitForSeconds(4f);
        boomerang1.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang2.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang3.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang4.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
    }

    private IEnumerator ShootEightDirections()
    {

        GameObject boomerang1 = Instantiate(boomerangBounce, shotPoint.position, shotPoint.rotation);
        GameObject boomerang2 = Instantiate(boomerangBounce, shotBehind.position, shotBehind.rotation);
        GameObject boomerang3 = Instantiate(boomerangBounce, shotAbove.position, shotAbove.rotation);
        GameObject boomerang4 = Instantiate(boomerangBounce, shotBelow.position, shotBelow.rotation);
        GameObject boomerang5 = Instantiate(boomerangBounce, shotNW.position, shotNW.rotation);
        GameObject boomerang6 = Instantiate(boomerangBounce, shotNE.position, shotNE.rotation);
        GameObject boomerang7 = Instantiate(boomerangBounce, shotSW.position, shotSW.rotation);
        GameObject boomerang8 = Instantiate(boomerangBounce, shotSE.position, shotSE.rotation);

        yield return new WaitForSeconds(0.3f);

        rangeWeaponSoundEffect.Play();
        boomerang1.GetComponent<Rigidbody2D>().velocity = Vector2.right * (rightOriented ? 1 : -1) * speedSecondAttack;
        boomerang2.GetComponent<Rigidbody2D>().velocity = Vector2.right * (rightOriented ? -1 : 1) * speedSecondAttack;
        boomerang3.GetComponent<Rigidbody2D>().velocity = Vector2.up * speedSecondAttack;
        boomerang4.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;

        yield return new WaitForSeconds(1f);


        rangeWeaponSoundEffect.Play();
        boomerang5.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? -1 : 1) + Vector2.up) * speedSecondAttack;
        boomerang6.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? 1 : -1) + Vector2.up) * speedSecondAttack;
        boomerang7.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? -1 : 1) + Vector2.down) * speedSecondAttack;
        boomerang8.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (rightOriented ? 1 : -1) + Vector2.down) * speedSecondAttack;

        yield return new WaitForSeconds(4f);
        boomerang1.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang2.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang3.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang4.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang5.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang6.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang7.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
        boomerang8.GetComponent<BoomerangBounceBossScript>().DestroyBoomerang();
    }
}
