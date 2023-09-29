using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossWeaponLevel2Script : MonoBehaviour
{

    public Animator animatorWeapon;
    public float delay = 0.01f;
    private bool cannotAttack;

    public Transform circleCenter;
    public float radius;

    public UnityEvent onAttackPerformed;

    public AudioSource swordSound;

    public PlayerScriptLevel2 playerScriptLevel2;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //ca si la caracter, deseneaza cercul pentru zona in care sabia da damage
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

    //triggerul pentru atac activat in timpul animatiei
    public void TriggerAttack()
    {
        onAttackPerformed?.Invoke();
    }

    public void Attack()
    {
        if (cannotAttack)
            return;

        float distanceToPlayer = Math.Abs(transform.position.x - playerPosition.position.x);
        if (distanceToPlayer < 6)
        {
            Debug.Log(distanceToPlayer);
            animatorWeapon.SetTrigger("Attack");
            cannotAttack = true;
            StartCoroutine(DelayAttack());
            cannotAttack = false;
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
    }

    public void DetectColliders()
    {
        bool wallBetween = false;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
        {
            if (collider.tag == "Floor")
            {
                wallBetween = true;

            }
        }
        if (!wallBetween)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleCenter.position, radius))
            {
                if (collider.name == "PlayerLevel2")
                {
                    swordSound.Play();
                    playerScriptLevel2.TakeDamage(1, transform.position);

                }
            }
        }
    }
}
