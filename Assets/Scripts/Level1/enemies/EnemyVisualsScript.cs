using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyVisualsScript : MonoBehaviour
{
    public AIPath aiPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //in functie de directia in care vrea sa se miste inamicul, rotesc daca este nevoie partea vizuala astfel incat sprite-ul sa fie orientat in directia buna
        if(aiPath.desiredVelocity.x >=0.01f)
        {
            if(transform.localScale.x > 0 )
            {
                transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if(aiPath.desiredVelocity.x <= -0.01f)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
