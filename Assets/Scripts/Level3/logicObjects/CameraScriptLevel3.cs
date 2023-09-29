using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptLevel3 : MonoBehaviour
{

    //cat de rapida sa fie camera
    public float followSpeed;

    //pozitie player
    public GameObject playerLevel3;
    public float offsetX;
    public float offsetY;

    //scriptul playerului
    private PlayerScriptLevel3 playerScriptLevel3;

    Vector3 cameraPosition;
    public float shakeValue;
    public float shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        followSpeed = 2.5f;
        offsetX = 1f;
        offsetY = 1f;
        shakeValue = 0.25f;
        shakeTime = 1f;

        playerScriptLevel3 = GameObject.FindGameObjectWithTag("PlayerLevel3").GetComponent<PlayerScriptLevel3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScriptLevel3.playerAlive && playerScriptLevel3.playerInVision)
        {
            //pozitie noua camera
            Vector3 newpos = new Vector3(playerLevel3.transform.position.x + offsetX, playerLevel3.transform.position.y + offsetY, -10f);
            //modificare pozitie camera
            transform.position = Vector3.Slerp(transform.position, newpos, followSpeed * Time.deltaTime);
        }
    }

    //shake ca si in celelalte scene
    public void Shake()
    {
        cameraPosition = transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    void StartCameraShaking()
    {
        float cameraOffsetX = Random.value * shakeValue * 2 - shakeValue;
        float cameraOffsetY = Random.value * shakeValue * 2 - shakeValue;
        Vector3 cameraIntermediate = transform.position;
        cameraIntermediate.x = cameraIntermediate.x + cameraOffsetX;
        cameraIntermediate.y = cameraIntermediate.y + cameraOffsetY;
        transform.position = cameraIntermediate;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        //transform.position = cameraPosition;
    }
}
