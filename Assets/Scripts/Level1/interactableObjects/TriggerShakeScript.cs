using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShakeScript : MonoBehaviour
{
    public CameraScript cameraScript;
    public bool shakeOnlyOnce;
    // Start is called before the first frame update
    void Start()
    {
        shakeOnlyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player" && shakeOnlyOnce)
        {
            cameraScript.Shake();
            shakeOnlyOnce = false;
        }
    }
}
