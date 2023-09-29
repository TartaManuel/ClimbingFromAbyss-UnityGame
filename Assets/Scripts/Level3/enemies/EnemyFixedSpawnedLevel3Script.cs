using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixedSpawnedLevel3Script : MonoBehaviour
{

    public Transform playerTransform;
    public GameObject fireball;
    public float speed;
    public Transform shotPoint;

    Vector2 direction;

    public bool rightOriented;

    private float cooldownAttack;
    private float cooldownAttackActual;
    private bool restartedCooldownAttack;

    public GameObject explosionParticles;

    public bool activated;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "EnemyFixed2")
        {
            cooldownAttackActual = 3.5f;
            cooldownAttack = 3.5f;
        }
        else if(gameObject.name == "EnemyFixed3")
        {
            cooldownAttackActual = 2.5f;
            cooldownAttack = 2.5f;
        }
        else if (gameObject.name == "EnemyFixed4")
        {
            cooldownAttackActual = 4.5f;
            cooldownAttack = 4.5f;
        }
        restartedCooldownAttack = true;
        speed = 20;
        activated = false;

        if (PlayerPrefs.GetInt("checkpointReachedLevel3") == 2)
        {
            StartCoroutine(DestroyEnemyForCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (cooldownAttackActual > 0)
            {
                cooldownAttackActual -= Time.deltaTime;
            }
            else
            {
                if (restartedCooldownAttack)
                {
                    ShootFireball();
                }
            }
        }
    }

    //la fel ca la celalalt inamic
    public void ShootFireball()
    {
        StartCoroutine(Shoot());
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
    }

    private IEnumerator Shoot()
    {
        GameObject fireball1 = Instantiate(fireball, shotPoint.position + new Vector3(0, -4, 0), shotPoint.rotation);

        Vector2 projectilePosition2 = shotPoint.position;
        direction = new Vector2(playerTransform.position.x, playerTransform.position.y) - projectilePosition2;

        fireball1.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        yield return new WaitForSeconds(5f);
        Destroy(fireball1);
    }

    public void DestoroySelf()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Destroy(gameObject, 2f);
    }

    private IEnumerator DestroyEnemyForCheckpoint()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
