using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointCanvasLevel2Script : MonoBehaviour
{
    private Animator checkpointCanvasAnimator;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().enabled = false;
        checkpointCanvasAnimator = GetComponentInChildren<Text>().GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //functii care apeleaza un mesaj care indica ca progresul este salvat la checkpoint
    public void ActivateCheckpointAnimation()
    {
        StartCoroutine(ActivateCheckpointAnimationC());
    }
    private IEnumerator ActivateCheckpointAnimationC()
    {
        GetComponentInChildren<Text>().enabled = true;
        checkpointCanvasAnimator.SetBool("checkpointHit", true);
        yield return new WaitForSeconds(2.5f);
        checkpointCanvasAnimator.SetBool("checkpointHit", false);
        GetComponentInChildren<Text>().enabled = false;
    }
}
