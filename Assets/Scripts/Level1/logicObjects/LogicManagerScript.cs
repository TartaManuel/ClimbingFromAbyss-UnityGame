using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class LogicManagerScript : MonoBehaviour
{
    public GameObject gameOverObject;
    private ColorBlock color;

    public AudioSource backroundSound;
    public AudioSource gameOverMusic;
    public AudioSource playerJumpSound;
    public AudioSource rangeWeaponSound;
    public AudioSource weaponSound;
    public AudioSource enemyAI1DeathSound;
    public AudioSource enemyAI2DeathSound;
    public AudioSource chestHitSound;
    public AudioSource fightMusic;

    public bool pausedGame = false;
    public bool gameIsOver = false;

    public GameObject pauseObject;

    public LevelChangerScript levelChangerScript;

    //mesajele de tutorial, le pun aici ca sa le ascund in pauza si game over
    public GameObject tutorialMovement;
    public GameObject tutorialChest;
    public GameObject tutorialEnemies;
    private bool tutorialEnemiesWasActive;
    private bool tutorialChestWasActive;
    private bool tutorialMovementWasActive;

    //pentru final de joc, cand numarul de inamici ajunge la 0
    public int nbOfEnemies;
    //animatia de deschidere a usii
    public Animator gateAnimator;

    //chestii de pus in player prefs
    public RangeWeaponScript rangeWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausedGame = false;
        pauseObject.SetActive(false);
        gameOverMusic.Stop();

        tutorialChestWasActive = false;
        tutorialEnemiesWasActive = false;
        tutorialMovementWasActive = false;

        nbOfEnemies = 2;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == false && gameIsOver == false)
        {

            Time.timeScale = 0;
            pausedGame = true;
            backroundSound.Pause();
            gameOverMusic.Play();
            pauseObject.SetActive(true);
            SelectRestartButton();
            SetExitButtonSelectedColor();

            //ascund mesajele de tutorial cand am ecranul de pauza activ
            if (tutorialEnemies.activeInHierarchy)
            {
                tutorialEnemiesWasActive = true;
            }
            else
            {
                tutorialEnemiesWasActive = false;
            }


            if (tutorialChest.activeInHierarchy)
            {
                tutorialChestWasActive = true;
            }
            else
            {
                tutorialChestWasActive = false;
            }


            if (tutorialMovement.activeInHierarchy)
            {
                tutorialMovementWasActive = true;
            }
            else
            {
                tutorialMovementWasActive = false;
            }
            tutorialChest.SetActive(false);
            tutorialEnemies.SetActive(false);
            tutorialMovement.SetActive(false);

            //dau pauza la sound effects, si dau unpause cand ies din pauza
            playerJumpSound.Pause();
            rangeWeaponSound.Pause();
            weaponSound.Pause();
            chestHitSound.Pause();
            enemyAI1DeathSound.Pause();
            enemyAI2DeathSound.Pause();
            fightMusic.Pause();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == true && gameIsOver == false)
        {

            Time.timeScale = 1;
            pausedGame = false;
            backroundSound.UnPause();
            gameOverMusic.Stop();
            pauseObject.SetActive(false);

            if (tutorialEnemiesWasActive)
            {
                tutorialEnemies.SetActive(true);
            }
            if (tutorialChestWasActive)
            {
                tutorialChest.SetActive(true);
            }
            if (tutorialMovementWasActive)
            {
                tutorialMovement.SetActive(true);
            }

            playerJumpSound.UnPause();
            rangeWeaponSound.UnPause();
            weaponSound.UnPause();
            chestHitSound.UnPause();
            enemyAI1DeathSound.UnPause();
            enemyAI2DeathSound.UnPause();
            fightMusic.UnPause();

        }
    }

    //verificare ca toti inamicii au fost batuti
    public void CheckLevelOver()
    {
        nbOfEnemies--;

        if(nbOfEnemies == 0 )
        {
            fightMusic.Stop();
            backroundSound.Play();
            gateAnimator.SetTrigger("TriggerDoor");
        }

    }

    //functie restart game
    public void RestartGame()
    {
        gameOverMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //functie de game over, afisez ecranul si pornesc muzica
    public void GameOver()
    {
        fightMusic.Stop();
        gameIsOver = true;
        gameOverObject.SetActive(true);
        backroundSound.Stop();
        Time.timeScale = 0;
        gameOverMusic.Play();
        SelectPlayAgainButton();
        SetExitGameOverButton();
    }


    //functii de selectare a butoanelor, ca sa pot navinga printre ele cu tastatura
    public void SelectPlayAgainButton()
    {
        color = GameObject.FindGameObjectWithTag("PlayAgainButton").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PlayAgainButton").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PlayAgainButton").GetComponent<Button>().colors = color;
    }

    public void SetExitGameOverButton()
    {
        color = GameObject.FindGameObjectWithTag("ExitGameOverButton").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("ExitGameOverButton").GetComponent<Button>().colors = color;
    }

    public void SelectRestartButton()
    {
        color = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>().colors = color;
    }

    public void SetExitButtonSelectedColor()
    {
        color = GameObject.FindGameObjectWithTag("ExitTitleScreenButton").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("ExitTitleScreenButton").GetComponent<Button>().colors = color;
    }

    public void MoveToScene(int sceneId)
    {
        //inainte sa trec la scena urmatoare, salvez in player prefs puterile pe care le-am obtinut
        //in acest caz, am range weapon-ul
        //nu vreau sa salvez in PlayerPrefs daca playerul a iesit la title screen, doar daca a terminat nivelul
        if (sceneId != 0)
        {
            PlayerPrefs.SetInt("RangeWeapon", rangeWeaponScript.achievedRangedWeapon ? 1 : 0);
            if (PlayerPrefs.GetInt("LastFinishedLevel") <= 1)
            {
                PlayerPrefs.SetInt("LastFinishedLevel", 1);
            }
            PlayerPrefs.SetInt("CollectedChestsLevel1", rangeWeaponScript.achievedRangedWeapon ? 1 : 0);
        }
        Time.timeScale = 1;
        levelChangerScript.FadeToScene(sceneId);
    }

    public void PlayFightMusic()
    {
        fightMusic.Play();
        backroundSound.Stop();
    }
}
