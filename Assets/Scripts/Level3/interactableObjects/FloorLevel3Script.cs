using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLevel3Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //script pentru floor, daca e lovit de boomerang trimit un semnal catre inamic ca boomerangul nu o sa se intoarca la el
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BoomerangSpawn(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang1").GetComponent<EnemyBoomerangSpawned1Script>().returnedBoomerang = true;
        }
        if (collision.gameObject.name == "Boomerang(Clone)")
        {
            GameObject.FindGameObjectWithTag("EnemyBoomerang").GetComponent<EnemyBoomergangScript>().returnedBoomerang = true;
        }
        if (collision.gameObject.name == "BoomerangBoss(Clone)")
        {
            GameObject.FindGameObjectWithTag("BossLevel3").GetComponent<BossLevel3Script>().returnedBoomerang = true;
        }
    }
}
