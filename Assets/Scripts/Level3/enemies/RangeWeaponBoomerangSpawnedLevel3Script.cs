using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBoomerangSpawnedLevel3Script : MonoBehaviour
{

    public Transform playerTransform;
    public GameObject boomerang;
    public float speed;
    public Transform shotPoint;
    public EnemyBoomerangSpawned1Script enemyBoomergangScript;

    Vector2 direction;

    public bool rightOriented;


    //sound effect range attack
    public AudioSource rangeWeaponSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootBoomerang()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        enemyBoomergangScript.returnedBoomerang = false;
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
        enemyBoomergangScript.returnedBoomerang = true;
        Destroy(boomerang1);
    }
}
