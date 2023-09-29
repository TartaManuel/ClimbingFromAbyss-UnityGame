using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponScriptLevel3 : MonoBehaviour
{

    //pentru animatie la weapon
    public Animator animatorWeapon;
    public float delay = 0.3f;
    private bool cannotAttack;
    public int weaponDamage;

    public Transform circleCenter;
    public float radius;

    public UnityEvent onAttackPerformed;


    public AudioSource swordSound;
    public AudioSource swordBuffedSound;

    //pentru puterea care imi da bonus damage;
    public Text textAttackUntilBuff;
    public GameObject onCooldown;
    private bool powerActive;
    public GameObject buffedAttackParticles;
    public bool powerReceived;

    public GameObject stunPowerOnCooldown;
    public bool stunPowerReceived;
    public GameObject stunParticles;
    public float stunPowerCooldownActual;
    public float stunPowerCooldown;

    public void TriggerAttack()
    {
        onAttackPerformed?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        delay = 0.3f;
        weaponDamage = 1;
        powerActive = false;
        powerReceived= false;
        buffedAttackParticles.GetComponent<ParticleSystem>().enableEmission = false;
        stunParticles.GetComponent<ParticleSystem>().enableEmission = false;

        stunPowerReceived = false;
        stunPowerCooldown = 20f;
        stunPowerCooldownActual = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        if (stunPowerReceived && stunPowerCooldownActual > 0)
        {
            stunPowerCooldownActual -= Time.deltaTime;
            stunPowerOnCooldown.SetActive(true);
            stunParticles.GetComponent<ParticleSystem>().enableEmission = false;
        }
        else if (stunPowerReceived && stunPowerCooldownActual <= 0)
        {
            stunPowerOnCooldown.SetActive(false);
            stunParticles.GetComponent<ParticleSystem>().enableEmission = true;
        }
    }

    public void Attack()
    {
        if (cannotAttack)
        {
            return;
        }
        animatorWeapon.SetTrigger("Attack");
        cannotAttack = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        cannotAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position;
        if (circleCenter == null)
        {
            position = Vector3.zero;
        }
        else
        {
            position = circleCenter.position;
        }
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
        {

            if (collider.name == "BreakableBox1Level3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox2Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
            }
            else if (collider.name == "BreakableBox2Level3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox1Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
            }
            else if (collider.name == "PushableBox1Level3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<PushableBox1Level3Script>().PushBox(transform);
            }
            else if (collider.name == "BreakableWallChestLevel3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<BreakableWallChestLevel3Script>().destroyWall();
            }
            else if (collider.name == "EnemyBoomerang")
            {
                if(powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                if (stunPowerReceived && stunPowerCooldownActual <= 0)
                {
                    collider.GetComponent<EnemyBoomergangScript>().isStunned = true;
                    stunPowerCooldownActual = stunPowerCooldown;
                }
                collider.GetComponent<EnemyBoomergangScript>().TakeDamage(weaponDamage, transform);
            }
            else if (collider.name == "Chest1Level3")
            {
                swordSound.Play();
                collider.GetComponent<Chest1ScriptLevel3>().GetHit();
            }
            else if (collider.name == "Chest2Level3")
            {
                swordSound.Play();
                collider.GetComponent<Chest2Level3Script>().GetHit();
            }
            else if (collider.name == "Chest3Level3")
            {
                swordSound.Play();
                collider.GetComponent<Chest3ScriptLevel3>().GetHit();
            }
            else if (collider.name == "FallingBox3Level3")
            {
                swordSound.Play();
                collider.GetComponent<FallingBox3Level3Script>().activateGravity = true;
            }
            else if (collider.name == "PushableBox2Level3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<PushableBox2Level3Script>().PushBox(transform);
            }
            else if (collider.name == "BreakableBox3Level3")
            {
                swordSound.Play();
                collider.GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox4Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox5Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
            }
            else if (collider.name == "BreakableBox4Level3")
            {
                swordSound.Play();
                collider.GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox3Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox5Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
            }
            else if (collider.name == "BreakableBox5Level3")
            {
                swordSound.Play();
                collider.GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox3Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
                GameObject.FindGameObjectWithTag("BreakableBox4Level3").GetComponent<BreakableBox1Level3Script>().DestroyBox();
            }
            else if (collider.name == "FallingBox5Level3")
            {
                swordSound.Play();
                collider.GetComponent<FallingBox5Level3Script>().activateGravity = true;
            }
            else if (collider.name == "TilemapBreakablePlatform4")
            {
                swordSound.Play();
                collider.GetComponent<PlatformScript>().SetDesiredStrategy();
            }
            else if (collider.name == "EnemyBoomerang1")
            {
                if (powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                if (stunPowerReceived && stunPowerCooldownActual <= 0)
                {
                    collider.GetComponent<EnemyBoomerangSpawned1Script>().isStunned = true;
                    stunPowerCooldownActual = stunPowerCooldown;
                }
                collider.GetComponent<EnemyBoomerangSpawned1Script>().TakeDamage(weaponDamage, transform);
            }
            else if (collider.name == "EnemySpawner")
            {
                if (powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                collider.GetComponent<EnemySpawnerScript>().TakeDamage(weaponDamage, transform);
            }
            else if (collider.name == "EnemyGroundedSpawn")
            {
                if (powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                if (stunPowerReceived && stunPowerCooldownActual <= 0)
                {
                    collider.GetComponent<EnemyGroundedSpawn>().isStunned = true;
                    stunPowerCooldownActual = stunPowerCooldown;
                }
                collider.GetComponent<EnemyGroundedSpawn>().TakeDamage(weaponDamage, transform);
            }
            else if (collider.name == "EnemyGroundedSpawn2")
            {
                if (powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                if (stunPowerReceived && stunPowerCooldownActual <= 0)
                {
                    collider.GetComponent<EnemyGroundedSpawn>().isStunned = true;
                    stunPowerCooldownActual = stunPowerCooldown;
                }
                collider.GetComponent<EnemyGroundedSpawn>().TakeDamage(weaponDamage, transform);
            }
            else if (collider.name == "PushableBox3Level3")
            {
                swordSound.Play();
                //breakableBoxLevel2Script.DestroyBox();
                collider.GetComponent<PushableBox3Level3Script>().PushBox(transform);
            }
            else if (collider.name == "FallingBox11Level3")
            {
                swordSound.Play();
                collider.GetComponent<FallingBox11Level3Script>().activateGravity = true;
            }
            else if (collider.name == "BossLevel3")
            {
                if (powerActive)
                {
                    swordBuffedSound.Play();
                }
                else
                {
                    swordSound.Play();
                }
                if (stunPowerReceived && stunPowerCooldownActual <= 0)
                {
                    collider.GetComponent<BossLevel3Script>().isStunned = true;
                    stunPowerCooldownActual = stunPowerCooldown;
                }
                collider.GetComponent<BossLevel3Script>().TakeDamage(weaponDamage, transform);
            }

            CheckWeaponPowerup(collider);
        }
    }

    private void CheckWeaponPowerup(Collider2D collider)
    {
        // o sa pun aici toate posibilitatile de inamici
        // daca lovesc un inamic, scad numarul de lovituri necesare pentru powerup la weapon
        if (collider.name == "EnemyBoomerang" || collider.name == "EnemyBoomerang1" || collider.name == "EnemySpawner" || collider.name == "EnemyGroundedSpawn" ||
            collider.name == "EnemyGroundedSpawn2" || collider.name == "BossLevel3")
        {
            if(powerReceived)
            {
                if (powerActive)
                {
                    powerActive = false;
                    textAttackUntilBuff.text = "3";
                    onCooldown.SetActive(true);
                    weaponDamage = 1;
                    buffedAttackParticles.GetComponent<ParticleSystem>().enableEmission = false;
                }
                else
                {
                    int numberOfAttacks = Int32.Parse(textAttackUntilBuff.text);
                    numberOfAttacks--;
                    textAttackUntilBuff.text = numberOfAttacks.ToString();

                    if (numberOfAttacks == 0)
                    {
                        onCooldown.SetActive(false);
                        weaponDamage = 2;
                        powerActive = true;
                        buffedAttackParticles.GetComponent<ParticleSystem>().enableEmission = true;
                    }
                }
            }
        }
    }

    private IEnumerator Knockback(Rigidbody2D rbEnemy)
    {
        yield return new WaitForSeconds(0.3f);
        rbEnemy.velocity = Vector2.zero;

    }

    //private IEnumerator KnockbackBoss(Rigidbody2D rbEnemy)
    //{
    //    Vector2 difference = rbEnemy.transform.position - transform.position;
    //    difference = new Vector2(difference.normalized.x * 1, 0);
    //    rbEnemy.velocity = difference;

    //    GameObject.FindGameObjectWithTag("BossLevel3").GetComponent<BossLevel2Script>().shouldMove = false;

    //    yield return new WaitForSeconds(0.1f);
    //    rbEnemy.velocity = Vector2.zero;
    //    rbEnemy.isKinematic = true;

    //    GameObject.FindGameObjectWithTag("BossLevel3").GetComponent<BossLevel2Script>().shouldMove = true;

    //}
}
