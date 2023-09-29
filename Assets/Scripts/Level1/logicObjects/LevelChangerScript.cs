using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator levelChangeAnimator;
    private int sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToScene(int indexScene)
    {
        levelChangeAnimator.SetTrigger("FadeOut");
        sceneToLoad = indexScene;
    }

    public void OnFadeCompleted()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
