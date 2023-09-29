using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //cat de rapida sa fie camera
    public float followSpeed;

    //pozitie player
    public GameObject player;
    public float offsetX;
    public float offsetY;

    //scriptul playerului
    private PlayerScript playerScript;

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

    playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.playerAlive && playerScript.playerInVision)
        {
            //pozitie noua camera
            Vector3 newpos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, -10f);
            //modificare pozitie camera
            transform.position = Vector3.Slerp(transform.position, newpos, followSpeed * Time.deltaTime);
        }
    }

    //functia care o apelez cand vreau sa se faca shake la camera
    public void Shake()
    {
        cameraPosition = transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    //functia care face shake propriu-zis. Foloseste valori random pentru a misca putin camera
    void StartCameraShaking()
    {
        float cameraOffsetX = Random.value * shakeValue * 2 - shakeValue;
        float cameraOffsetY = Random.value * shakeValue * 2 - shakeValue;
        Vector3 cameraIntermediate = transform.position;
        cameraIntermediate.x = cameraIntermediate.x + cameraOffsetX;
        cameraIntermediate.y = cameraIntermediate.y + cameraOffsetY;
        transform.position = cameraIntermediate;
    }

    //opreste shake la camera dupa un anumit timp
    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
    }
}
