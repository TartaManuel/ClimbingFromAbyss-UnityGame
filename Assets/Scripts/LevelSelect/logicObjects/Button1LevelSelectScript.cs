using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button1LevelSelectScript : MonoBehaviour
{
    Button button;
    public Text textCollected;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.Select();

        if(PlayerPrefs.GetInt("LastFinishedLevel") < 0)
        {
            gameObject.SetActive(false);
        }
        textCollected.text = PlayerPrefs.GetInt("CollectedChestsLevel1") + " of 1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
