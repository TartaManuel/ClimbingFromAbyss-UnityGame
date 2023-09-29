using UnityEngine;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour
{

    public AudioSource backroundSound;
    public LevelChangerScript levelChangerScript;
    public GameObject resetProgressObject;
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject levelSelectButton;
    public GameObject clearProgressButton;


    public void MoveToScene(int sceneId)
    {
        //SceneManager.LoadScene(sceneId);
        levelChangerScript.FadeToScene(sceneId);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectPlay()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("PlayButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PlayButtonTitleScreen").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PlayButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void SetSelectedExit()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("ExitButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("ExitButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void SetSelectedLevelSelect()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("LevelSelectButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("LevelSelectButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void SetSelectedClearProggress()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("ResetProgressButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("ResetProgressButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void SelectYes()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("YesButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("YesButtonTitleScreen").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("YesButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void SetSelectedNo()
    {
        ColorBlock color;
        color = GameObject.FindGameObjectWithTag("NoButtonTitleScreen").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("NoButtonTitleScreen").GetComponent<Button>().colors = color;
    }

    public void PlayLastPlayedLevel()
    {
        int sceneToPlay = PlayerPrefs.GetInt("LastFinishedLevel") + 1;
        if (sceneToPlay == 3)
        {
            MoveToScene(sceneToPlay + 1);
        }
        else if(sceneToPlay == 5)
        {
            MoveToScene(sceneToPlay - 1);
        }
        else
        {
            MoveToScene(sceneToPlay);
        }
    }

    public void ClearProgress()
    {
        PlayerPrefs.SetInt("LastFinishedLevel", 0);
        PlayerPrefs.SetInt("CollectedChestsLevel1", 0);
        PlayerPrefs.SetInt("CollectedChestsLevel2", 0);
        PlayerPrefs.SetInt("CollectedChestsLevel3", 0);
        PlayerPrefs.SetInt("CollectedChest1Level3", 0);
        PlayerPrefs.SetInt("CollectedChest2Level3", 0);
        PlayerPrefs.SetInt("CollectedChest3Level3", 0);
        HideResetProgressScreen();
    }

    public void ShowResetProgressScreen()
    {
        resetProgressObject.SetActive(true);
        SelectYes();
        SetSelectedNo();
        playButton.SetActive(false);
        exitButton.SetActive(false);
        levelSelectButton.SetActive(false);
        clearProgressButton.SetActive(false);
    }

    public void HideResetProgressScreen()
    {
        resetProgressObject.SetActive(false);
        playButton.SetActive(true);
        exitButton.SetActive(true);
        levelSelectButton.SetActive(true);
        clearProgressButton.SetActive(true);
        SelectPlay();
    }


    // start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        SelectPlay();
        SetSelectedExit();
        SetSelectedLevelSelect();
        SetSelectedClearProggress();
        backroundSound.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
