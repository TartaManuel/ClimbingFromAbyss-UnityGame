using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptLevel2 : MonoBehaviour
{
    //cat de rapida sa fie camera
    public float followSpeed;

    //pozitie player
    public GameObject playerLevel2;
    public float offsetX;
    public float offsetY;

    //scriptul playerului
    private PlayerScriptLevel2 playerScriptLevel2;

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

        playerScriptLevel2 = GameObject.FindGameObjectWithTag("PlayerLevel2").GetComponent<PlayerScriptLevel2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScriptLevel2.playerAlive && playerScriptLevel2.playerInVision)
        {
            //pozitie noua camera
            Vector3 newpos = new Vector3(playerLevel2.transform.position.x + offsetX, playerLevel2.transform.position.y + offsetY, -10f);
            //modificare pozitie camera
            transform.position = Vector3.Slerp(transform.position, newpos, followSpeed * Time.deltaTime);
        }
    }

    //la fel ca si in scena 1, shake la camera cu o functie care este invocata repetat, si dupa un timp stop la shake
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
    }
}
