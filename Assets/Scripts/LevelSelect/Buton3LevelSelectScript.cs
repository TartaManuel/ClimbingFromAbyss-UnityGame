using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buton3LevelSelectScript : MonoBehaviour
{
    public Text textCollected;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LastFinishedLevel") < 2)
        {
            gameObject.SetActive(false);
        }
        textCollected.text = PlayerPrefs.GetInt("CollectedChestsLevel3") + " of 3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
