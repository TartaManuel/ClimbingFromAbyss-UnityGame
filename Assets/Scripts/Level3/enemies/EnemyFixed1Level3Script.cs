using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixed1Level3Script : MonoBehaviour
{

    public Transform playerTransform;
    public GameObject fireball;
    public float speed;
    public Transform shotPoint;
    public EnemyBoomergangScript enemyBoomergangScript;

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
        cooldownAttack = 3f;
        cooldownAttackActual = 2f;
        restartedCooldownAttack = true;
        speed = 25;
        activated = false;

        if (PlayerPrefs.GetInt("checkpointReachedLevel3") == 1)
        {
            StartCoroutine(DestroyEnemyForCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activated) 
        {
            if (enemyBoomergangScript.isDead == true)
            {
                Instantiate(explosionParticles, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                Destroy(gameObject, 2f);
            }

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

    //un inamic fixat, care doar arunca fireballs spre jucator
    public void ShootFireball()
    {
        StartCoroutine(Shoot());
        cooldownAttackActual = cooldownAttack;
        restartedCooldownAttack = true;
    }

    //corutina pentru fireball, il creeaza, ii da velocity si dupa 5 secunde il distruge
    private IEnumerator Shoot()
    {
        GameObject fireball1 = Instantiate(fireball, shotPoint.position + new Vector3(0, -4, 0), shotPoint.rotation);

        Vector2 projectilePosition2 = shotPoint.position;
        direction = new Vector2(playerTransform.position.x, playerTransform.position.y) - projectilePosition2;

        fireball1.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        yield return new WaitForSeconds(5f);
        Destroy(fireball1);
    }

    private IEnumerator DestroyEnemyForCheckpoint()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
