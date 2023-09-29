using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button2LevelSelectScript : MonoBehaviour
{
    public Text textCollected;
    // Start is called before the first frame update
    void Start()
    {
        //practic vreau sa arat nivelul actual terminat, plus urmatorul, deci cand am terminat lvl 1, imi apare si lvl 2
        if (PlayerPrefs.GetInt("LastFinishedLevel") < 1)
        {
            gameObject.SetActive(false);
        }
        textCollected.text = PlayerPrefs.GetInt("CollectedChestsLevel2") + " of 1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
