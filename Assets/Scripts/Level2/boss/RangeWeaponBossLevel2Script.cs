using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBossLevel2Script : MonoBehaviour
{
    public Transform playerPosition;
    public GameObject fireball;
    public float speed;
    public Transform shotPoint;

    Vector2 direction1;
    Vector2 direction2;
    Vector2 direction3;

    //sound effect range attack
    public AudioSource rangeWeaponSoundEffect;

    public bool rightOriented;

    int i = 0;

    public float speedSecondAttack;

    // Start is called before the first frame update
    void Start()
    {
        speed = 50f;
        speedSecondAttack = 65f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //functiile care apeleaza corutinele de mai jos
    public void ShootFireball()
    {
        StartCoroutine(Shoot());

    }

    public void ShootFireballAbove(Vector3 position)
    {
        StartCoroutine(ShootAboveMe(position));

    }

    public void ShootFireballAfterJump()
    {
        StartCoroutine(ShootAfterJump());
    }

    //prima functie in care bossul are 3 fireballuri in fata si le arunca pe rand spre jucator
    private IEnumerator Shoot()
    {

        GameObject fireball1 = Instantiate(fireball, shotPoint.position + new Vector3(0, 3f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.3f);

        GameObject fireball3 = Instantiate(fireball, shotPoint.position, shotPoint.rotation);

        yield return new WaitForSeconds(0.3f);

        GameObject fireball2 = Instantiate(fireball, shotPoint.position - new Vector3(0, 3f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.3f);

        Vector2 projectilePosition2 = shotPoint.position + new Vector3(0, 0.3f, 0);
        direction2 = new Vector2(playerPosition.position.x, playerPosition.position.y) - projectilePosition2;

        if (rightOriented)
        {
            transform.right = direction2;
            speed = 50f;
        }
        else
        {
            transform.right = -direction2;
            speed = -50f;
        }
        rangeWeaponSoundEffect.Play();
        fireball1.GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        yield return new WaitForSeconds(1f);

        Vector2 projectilePosition1 = shotPoint.position;
        direction1 = new Vector2(playerPosition.position.x, playerPosition.position.y) - projectilePosition1;

        if (rightOriented)
        {
            transform.right = direction1;
            speed = 50f;
        }
        else
        {
            transform.right = -direction1;
            speed = -50f;
        }
        rangeWeaponSoundEffect.Play();
        fireball3.GetComponent<Rigidbody2D>().velocity = transform.right * speed;


        yield return new WaitForSeconds(1f);

        Vector2 projectilePosition3 = shotPoint.position - new Vector3(0, 0.3f, 0); ;
        direction3 = new Vector2(playerPosition.position.x, playerPosition.position.y) - projectilePosition3;

        if (rightOriented)
        {
            transform.right = direction3;
            speed = 50f;
        }
        else
        {
            transform.right = -direction3;
            speed = -50f;
        }
        rangeWeaponSoundEffect.Play();
        fireball2.GetComponent<Rigidbody2D>().velocity = transform.right * speed;


        yield return new WaitForSeconds(7f);
        Destroy(fireball1);
        Destroy(fireball2);
        Destroy(fireball3);
    }

    //aici sunt 7 fireballuri deasupra jucatorului care cad intr-o secventa
    private IEnumerator ShootAboveMe(Vector3 position)
    {
        GameObject fireball4 = Instantiate(fireball, position + new Vector3(0, 14f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.1f);

        GameObject fireball3 = Instantiate(fireball, position + new Vector3(-4, 14f, 0), shotPoint.rotation);

        GameObject fireball5 = Instantiate(fireball, position + new Vector3(4, 14f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.1f);

        GameObject fireball2 = Instantiate(fireball, position + new Vector3(-8, 14f, 0), shotPoint.rotation);

        GameObject fireball6 = Instantiate(fireball, position + new Vector3(8, 14f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.1f);

        GameObject fireball1 = Instantiate(fireball, position + new Vector3(-12, 14f, 0), shotPoint.rotation);

        GameObject fireball7 = Instantiate(fireball, position + new Vector3(12, 14f, 0), shotPoint.rotation);

        yield return new WaitForSeconds(0.25f);

        rangeWeaponSoundEffect.Play();
        fireball4.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;

        yield return new WaitForSeconds(0.15f);

        rangeWeaponSoundEffect.Play();
        fireball3.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;
        fireball5.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;

        yield return new WaitForSeconds(0.15f);

        rangeWeaponSoundEffect.Play();
        fireball2.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;
        fireball6.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;

        yield return new WaitForSeconds(0.15f);

        rangeWeaponSoundEffect.Play();
        fireball1.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;
        fireball7.GetComponent<Rigidbody2D>().velocity = Vector2.down * speedSecondAttack;


        yield return new WaitForSeconds(5f);
        Destroy(fireball1);
        Destroy(fireball2);
        Destroy(fireball3);
        Destroy(fireball4);
        Destroy(fireball5);
        Destroy(fireball6);
        Destroy(fireball7);
    }

    //dupa jump la boss, cate un fireball aproape de pamant in ambele directii
    private IEnumerator ShootAfterJump()
    {
        GameObject fireball1 = Instantiate(fireball, shotPoint.position + new Vector3(3, -1 , 0), shotPoint.rotation);

        GameObject fireball2 = Instantiate(fireball, shotPoint.position + new Vector3(3, -1, 0), shotPoint.rotation);
        fireball2.transform.localScale = new Vector3(-1f * fireball2.transform.localScale.x, fireball2.transform.localScale.y, fireball2.transform.localScale.z);

        yield return new WaitForSeconds(0.05f);

        rangeWeaponSoundEffect.Play();
        fireball1.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed / 1.5f;
        fireball2.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed / 1.5f;


        yield return new WaitForSeconds(5f);
        Destroy(fireball1);
        Destroy(fireball2);
    }
}
