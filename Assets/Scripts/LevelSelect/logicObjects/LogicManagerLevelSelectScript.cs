using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManagerLevelSelectScript : MonoBehaviour
{

    public AudioSource backroundSound;
    public LevelChangerScriptLevel2 levelChangerScriptLevel2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //cu ESC ma duc inapoi la title screen
            MoveToScene(0);
        }
    }

    public void MoveToScene(int sceneId)
    {
        levelChangerScriptLevel2.FadeToScene(sceneId);
    }
}
