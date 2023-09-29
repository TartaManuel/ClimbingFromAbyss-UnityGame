using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicManagerScriptLevel2 : MonoBehaviour
{
    public GameObject gameOverObjectLevel2;
    private ColorBlock color;

    public AudioSource backroundSound;
    public AudioSource gameOverMusic;
    public AudioSource playerJumpSound;
    public AudioSource rangeWeaponSound;
    public AudioSource weaponSound;
    public AudioSource enemyAI1DeathSound;
    public AudioSource enemyAI2DeathSound;
    public AudioSource enemyGrounded1DeathSound;
    public AudioSource enemyGrounded2DeathSound;
    public AudioSource chestHitSound;
    public AudioSource fightMusic;
    public AudioSource bossDeathSound;
    public AudioSource bossWeaponSound;
    public AudioSource bossJumpSound;
    public AudioSource rangeWeaponBossSound;
    public AudioSource fireballBossSound;
    public AudioSource box1FallingSound;
    public AudioSource box2FallingSound;
    public AudioSource box3FallingSound;
    public AudioSource knockbackSound;
    public AudioSource breakableWall1Sound;
    public AudioSource boxExplodingSound;
    public AudioSource breakableWall2Sound;

    public bool pausedGame = false;
    public bool gameIsOver = false;

    public GameObject pauseObjectLevel2;

    public LevelChangerScriptLevel2 levelChangerScriptLevel2;

    //pentru final de joc, cand numarul de inamici ajunge la 0
    public int nbOfEnemies;
    //animatia de deschidere a usii
    public Animator gateAnimator;

    public int nbOfEnemiesGrounded;
    public bool bossKilled;
    private bool musicRestarted;

    //date pentru checkpoint
    public Transform playerTransform;
    public Transform boxTransform;

    //chestii de pus in player prefs
    //public RangeWeaponScriptLevel2 rangeWeaponScriptLevel2;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausedGame = false;
        pauseObjectLevel2.SetActive(false);
        gameOverMusic.Stop();

        nbOfEnemies = 2;
        nbOfEnemiesGrounded = 2;
        bossKilled = false;
        musicRestarted = false;

        //date luate din playerprefs pentru checkpoint
        float playerX = PlayerPrefs.GetFloat("playerX");
        float playerY = PlayerPrefs.GetFloat("playerY");
        float playerZ = PlayerPrefs.GetFloat("playerZ");
        if(playerX != 0 && playerY != 0 && playerZ !=0)
        {
            playerTransform.position = new Vector3(playerX, playerY, playerZ);
        }
        int chestOpened = PlayerPrefs.GetInt("chestOpened");
        if(chestOpened != 0 || PlayerPrefs.GetInt("CollectedChestsLevel2") != 0)
        {
            GameObject.FindGameObjectWithTag("ChestLevel2").GetComponent<ChestScriptLevel2>().GetHitNoSound();
        }
        float boxX = PlayerPrefs.GetFloat("boxX");
        float boxY = PlayerPrefs.GetFloat("boxY");
        float boxZ = PlayerPrefs.GetFloat("boxZ");
        if (boxX != 0 && boxY != 0 && boxZ != 0)
        {
            boxTransform.position = new Vector3(boxX, boxY, boxZ);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == false && gameIsOver == false)
        {

            Time.timeScale = 0;
            pausedGame = true;
            gameOverMusic.Play();
            pauseObjectLevel2.SetActive(true);
            SelectRestartButton();
            SetExitButtonSelectedColor();

            //dau pauza la sound effects, si dau unpause cand ies din pauza
            backroundSound.Pause();
            playerJumpSound.Pause();
            rangeWeaponSound.Pause();
            weaponSound.Pause();
            chestHitSound.Pause();
            enemyAI1DeathSound.Pause();
            enemyAI2DeathSound.Pause();
            fightMusic.Pause();
            enemyGrounded1DeathSound.Pause();
            enemyGrounded2DeathSound.Pause();
            bossDeathSound.Pause();
            bossWeaponSound.Pause();
            bossJumpSound.Pause();
            rangeWeaponBossSound.Pause();
            fireballBossSound.Pause();
            box1FallingSound.Pause();
            box2FallingSound.Pause();
            box3FallingSound.Pause();
            knockbackSound.Pause();
            breakableWall1Sound.Pause();
            boxExplodingSound.Pause();
            breakableWall2Sound.Pause();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == true && gameIsOver == false)
        {

            Time.timeScale = 1;
            pausedGame = false;
            backroundSound.UnPause();
            gameOverMusic.Stop();
            pauseObjectLevel2.SetActive(false);

            playerJumpSound.UnPause();
            rangeWeaponSound.UnPause();
            weaponSound.UnPause();
            chestHitSound.UnPause();
            enemyAI1DeathSound.UnPause();
            enemyAI2DeathSound.UnPause();
            fightMusic.UnPause();
            enemyGrounded1DeathSound.UnPause();
            enemyGrounded2DeathSound.UnPause();
            bossDeathSound.UnPause();
            bossWeaponSound.UnPause();
            bossJumpSound.UnPause();
            rangeWeaponBossSound.UnPause();
            fireballBossSound.UnPause();
            box1FallingSound.UnPause();
            box2FallingSound.UnPause();
            box3FallingSound.UnPause();
            knockbackSound.UnPause();
            breakableWall1Sound.UnPause();
            boxExplodingSound.UnPause();
            breakableWall2Sound.UnPause();

        }

        if(bossKilled && !musicRestarted)
        {
            fightMusic.Stop();
            backroundSound.Play();
            gateAnimator.SetTrigger("TriggerDoor");
            musicRestarted = true;
        }
    }

    //functia de restart
    public void RestartGame()
    {
        gameOverMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //functie game over
    public void GameOver()
    {
        fightMusic.Stop();
        gameIsOver = true;
        gameOverObjectLevel2.SetActive(true);
        backroundSound.Stop();
        Time.timeScale = 0;
        gameOverMusic.Play();
        SelectPlayAgainButton();
        SetExitGameOverButton();
    }

    //selectare butoane
    public void SelectPlayAgainButton()
    {
        color = GameObject.FindGameObjectWithTag("PlayAgainButtonLevel2").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PlayAgainButtonLevel2").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PlayAgainButtonLevel2").GetComponent<Button>().colors = color;
    }

    public void SetExitGameOverButton()
    {
        color = GameObject.FindGameObjectWithTag("ExitGameOverButtonLevel2").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("ExitGameOverButtonLevel2").GetComponent<Button>().colors = color;
    }

    public void SelectRestartButton()
    {
        color = GameObject.FindGameObjectWithTag("PauseButtonLevel2").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PauseButtonLevel2").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PauseButtonLevel2").GetComponent<Button>().colors = color;
    }

    public void SetExitButtonSelectedColor()
    {
        color = GameObject.FindGameObjectWithTag("ExitTitleScreenButtonLevel2").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("ExitTitleScreenButtonLevel2").GetComponent<Button>().colors = color;
    }


    //cand trec la o alta scena, adica ies din nivel setez pozitia unor obiecte inapoi ca fiind cea initiala, practic dau clear la setarile facute pentru checkpoint
    public void MoveToScene(int sceneId)
    {
        //cand dau load la un scene, pun pozitia inapoi ca fiind cea normala, de la inceputul nivelului
        PlayerPrefs.SetFloat("playerX", -52.4f);
        PlayerPrefs.SetFloat("playerY", -16.6f);
        PlayerPrefs.SetFloat("playerZ", playerTransform.position.z);
        PlayerPrefs.SetInt("chestOpened", 0);
        PlayerPrefs.SetFloat("boxX", -123.3f);
        PlayerPrefs.SetFloat("boxY", 111.8f);
        PlayerPrefs.SetFloat("boxZ", boxTransform.position.z);
        PlayerPrefs.SetInt("checkpointReached", 0);

        if (sceneId != 0)
        {
            PlayerPrefs.SetInt("LastFinishedLevel", 2);
            PlayerPrefs.SetInt("CollectedChestsLevel2", GameObject.FindGameObjectWithTag("ChestLevel2").GetComponent<ChestScriptLevel2>().isOpened ? 1:0);
        }

        Time.timeScale = 1;
        levelChangerScriptLevel2.FadeToScene(sceneId);
    }

    public void PlayFightMusic()
    {
        fightMusic.Play();
        backroundSound.Stop();
    }

    //setez anumite date in prefs pentru a le prelua la reinceperea de la un checkpoint
    public void SetPlayerPrefsForCheckpoint()
    {
        PlayerPrefs.SetFloat("playerX", playerTransform.position.x);
        PlayerPrefs.SetFloat("playerY", playerTransform.position.y);
        PlayerPrefs.SetFloat("playerZ", playerTransform.position.z);
        if(GameObject.FindGameObjectWithTag("ChestLevel2").GetComponent<ChestScriptLevel2>().isOpened)
        {
            PlayerPrefs.SetInt("chestOpened", 1);
        }
        else
        {
            PlayerPrefs.SetInt("chestOpened", 0);
        }
        PlayerPrefs.SetFloat("boxX", boxTransform.position.x);
        PlayerPrefs.SetFloat("boxY", boxTransform.position.y);
        PlayerPrefs.SetFloat("boxZ", boxTransform.position.z);
        PlayerPrefs.SetInt("checkpointReached", 1);
    }
}
