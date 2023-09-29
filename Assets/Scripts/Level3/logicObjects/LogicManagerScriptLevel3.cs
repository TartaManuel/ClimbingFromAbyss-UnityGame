using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicManagerScriptLevel3 : Singleton<LogicManagerScriptLevel3>
{
    private ColorBlock color;

    public AudioSource backroundSound;
    public AudioSource gameOverMusic;
    public AudioSource playerJumpSound;
    public AudioSource weaponSound;
    public AudioSource fightMusic;
    public AudioSource floorBreakingSound;
    public AudioSource chest1HitSound;
    public AudioSource platform1Sound;
    public AudioSource platform2Sound;
    public AudioSource box1DestroySound;
    public AudioSource box2DestroySound;
    public AudioSource box1FallingSound;
    public AudioSource box1PushSound;
    public AudioSource platform3Sound;
    public AudioSource enemyBoomerang1LaunchAttackSound;
    public AudioSource shieldBreakSound;
    public AudioSource playerKnockBackSound;
    public AudioSource enemyBoomerangDeathSound;
    public AudioSource chest2HitSound;
    public AudioSource box2FallingSound;
    public AudioSource weaponSound2;
    public AudioSource box3FallingSound;
    public AudioSource box2PushSound;
    public AudioSource box3DestroySound;
    public AudioSource box4DestroySound;
    public AudioSource box5DestroySound;
    public AudioSource platform4Sound;
    public AudioSource box4FallingSound;
    public AudioSource box5FallingSound;
    public AudioSource enemySpawnerDeathSound;
    public AudioSource enemySpawnerSpawnSound;
    public AudioSource enemySpawnerTeleportSound;
    public AudioSource platform5Sound;
    public AudioSource wallEnemiesSound;
    public AudioSource enemyBoomerandSpawnDeathSound;
    public AudioSource enemyGrounded1SpawnDeathSound;
    public AudioSource enemyGrounded2SpawnDeathSound;
    public AudioSource box6FallingSound;
    public AudioSource box7FallingSound;
    public AudioSource box8FallingSound;
    public AudioSource box9FallingSound;
    public AudioSource box10FallingSound;
    public AudioSource box11FallingSound;
    public AudioSource box3PushSound;
    public AudioSource platform6Sound;
    public AudioSource platform7Sound;
    public AudioSource bossDeathSound;
    public AudioSource bossTeleportSound;
    public AudioSource bossRangeSound;
    public AudioSource enemyBoomerangStunSound;
    public AudioSource enemyBoomerangSpawnedStunSound;
    public AudioSource enemyGrounded1SpawnStunSound;
    public AudioSource enemyGrounded2SpawnStunSound;
    public AudioSource bossStunSound;


    public bool pausedGame = false;
    public bool gameIsOver = false;
    public GameObject pauseObjectLevel3;
    public GameObject gameOverObjectLevel3;

    public LevelChangerScriptLevel3 levelChangerScriptLevel3;

    //pentru final de joc, cand numarul de inamici ajunge la 0
    public bool enemyBoomerangKilled;
    //animatia de deschidere a usii
    public Animator gateAnimator;

    public int nbOfEnemiesGrounded;
    public bool bossKilled;
    private bool musicRestarted;

    //date pentru checkpoint
    public Transform playerTransform;
    public Transform box1Transform;
    public Transform box2Transform;
    public Transform box3Transform;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausedGame = false;
        pauseObjectLevel3.SetActive(false);
        gameOverMusic.Stop();

        enemyBoomerangKilled = false;
        nbOfEnemiesGrounded = 2;
        bossKilled = false;
        musicRestarted = false;

        ////Debug.Log(PlayerPrefs.GetInt("bossKilled"));
        ////date luate din playerprefs pentru checkpoint
        float playerX = PlayerPrefs.GetFloat("playerXLevel3");
        float playerY = PlayerPrefs.GetFloat("playerYLevel3");
        float playerZ = PlayerPrefs.GetFloat("playerZLevel3");
        if (playerX != 0 && playerY != 0 && playerZ != 0)
        {
            playerTransform.position = new Vector3(playerX, playerY, playerZ);
        }
        int chest1Opened = PlayerPrefs.GetInt("chest1Opened");
        if (chest1Opened != 0 || PlayerPrefs.GetInt("CollectedChest1Level3") != 0)
        {
            GameObject.FindGameObjectWithTag("Chest1Level3").GetComponent<Chest1ScriptLevel3>().GetHitNoSound();
        }
        int chest2Opened = PlayerPrefs.GetInt("chest2Opened");
        if (chest2Opened != 0 || PlayerPrefs.GetInt("CollectedChest2Level3") != 0)
        {
            GameObject.FindGameObjectWithTag("Chest2Level3").GetComponent<Chest2Level3Script>().GetHitNoSound();
        }
        float box1X = PlayerPrefs.GetFloat("box1X");
        float box1Y = PlayerPrefs.GetFloat("box1Y");
        float box1Z = PlayerPrefs.GetFloat("box1Z");
        if (box1X != 0 && box1Y != 0 && box1Z != 0)
        {
            box1Transform.position = new Vector3(box1X, box1Y, box1Z);
        }
        float box2X = PlayerPrefs.GetFloat("box2X");
        float box2Y = PlayerPrefs.GetFloat("box2Y");
        float box2Z = PlayerPrefs.GetFloat("box2Z");
        if (box2X != 0 && box2Y != 0 && box2Z != 0)
        {
            box2Transform.position = new Vector3(box2X, box2Y, box2Z);
            if(Mathf.Abs(box2Y - 27.47f) > 0.1)
            {
                GameObject.FindGameObjectWithTag("FallingBox11Level3").GetComponent<FallingBox11Level3Script>().activateGravity = true;
            }
        }
        float box3X = PlayerPrefs.GetFloat("box3X");
        float box3Y = PlayerPrefs.GetFloat("box3Y");
        float box3Z = PlayerPrefs.GetFloat("box3Z");
        if (box3X != 0 && box3Y != 0 && box3Z != 0)
        {
            box3Transform.position = new Vector3(box3X, box3Y, box3Z);
        }
        int chest3Opened = PlayerPrefs.GetInt("chest3Opened");
        if (chest3Opened != 0 || PlayerPrefs.GetInt("CollectedChest3Level3") != 0)
        {
            GameObject.FindGameObjectWithTag("Chest3Level3").GetComponent<Chest3ScriptLevel3>().GetHitNoSound();
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
            pauseObjectLevel3.SetActive(true);
            SelectRestartButton();
            SetExitButtonSelectedColor();

            //dau pauza la sound effects, si dau unpause cand ies din pauza
            backroundSound.Pause();
            playerJumpSound.Pause();
            weaponSound.Pause();
            fightMusic.Pause();
            floorBreakingSound.Pause();
            chest1HitSound.Pause();
            platform1Sound.Pause();
            platform2Sound.Pause();
            box1DestroySound.Pause();
            box2DestroySound.Pause();
            box1FallingSound.Pause();
            box1PushSound.Pause();
            platform3Sound.Pause();
            enemyBoomerang1LaunchAttackSound.Pause();
            shieldBreakSound.Pause();
            playerKnockBackSound.Pause();
            enemyBoomerangDeathSound.Pause();
            chest2HitSound.Pause();
            box2FallingSound.Pause();
            weaponSound2.Pause();
            box3FallingSound.Pause();
            box2PushSound.Pause();
            box3DestroySound.Pause();
            box4DestroySound.Pause();
            box5DestroySound.Pause();
            box4FallingSound.Pause();
            platform4Sound.Pause();
            box5FallingSound.Pause();
            enemySpawnerDeathSound.Pause();
            enemySpawnerSpawnSound.Pause();
            enemySpawnerTeleportSound.Pause();
            platform5Sound.Pause();
            wallEnemiesSound.Pause();
            enemyBoomerandSpawnDeathSound.Pause();
            enemyGrounded1SpawnDeathSound.Pause();
            enemyGrounded2SpawnDeathSound.Pause();
            box6FallingSound.Pause();
            box7FallingSound.Pause();
            box8FallingSound.Pause();
            box9FallingSound.Pause();
            box10FallingSound.Pause();
            box11FallingSound.Pause();
            box3PushSound.Pause();
            platform6Sound.Pause();
            platform7Sound.Pause();
            bossDeathSound.Pause();
            bossTeleportSound.Pause();
            bossRangeSound.Pause();
            enemyBoomerangStunSound.Pause();
            enemyBoomerangSpawnedStunSound.Pause();
            enemyGrounded1SpawnStunSound.Pause();
            enemyGrounded2SpawnStunSound.Pause();
            bossStunSound.Pause();

}
        else if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == true && gameIsOver == false)
        {

            Time.timeScale = 1;
            pausedGame = false;
            backroundSound.UnPause();
            gameOverMusic.Stop();
            pauseObjectLevel3.SetActive(false);

            playerJumpSound.UnPause();
            weaponSound.UnPause();
            fightMusic.UnPause();
            floorBreakingSound.UnPause();
            chest1HitSound.UnPause();
            platform1Sound.UnPause();
            platform2Sound.UnPause();
            box1DestroySound.UnPause();
            box2DestroySound.UnPause();
            box1FallingSound.UnPause();
            box1PushSound.UnPause();
            platform3Sound.UnPause();
            enemyBoomerang1LaunchAttackSound.UnPause();
            shieldBreakSound.UnPause();
            playerKnockBackSound.UnPause();
            enemyBoomerangDeathSound.UnPause();
            chest2HitSound.UnPause();
            box2FallingSound.UnPause();
            weaponSound2.UnPause();
            box3FallingSound.UnPause();
            box2PushSound.UnPause();
            box3DestroySound.UnPause();
            box4DestroySound.UnPause();
            box5DestroySound.UnPause();
            box4FallingSound.UnPause();
            platform4Sound.UnPause();
            box5FallingSound.UnPause();
            enemySpawnerDeathSound.UnPause();
            enemySpawnerSpawnSound.UnPause();
            enemySpawnerTeleportSound.UnPause();
            platform5Sound.UnPause();
            wallEnemiesSound.UnPause();
            enemyBoomerandSpawnDeathSound.UnPause();
            enemyGrounded1SpawnDeathSound.UnPause();
            enemyGrounded2SpawnDeathSound.UnPause();
            box6FallingSound.UnPause();
            box7FallingSound.UnPause();
            box8FallingSound.UnPause();
            box9FallingSound.UnPause();
            box10FallingSound.UnPause();
            box11FallingSound.UnPause();
            box3PushSound.UnPause();
            platform6Sound.UnPause();
            platform7Sound.UnPause();
            bossDeathSound.UnPause();
            bossTeleportSound.UnPause();
            bossRangeSound.UnPause();
            enemyBoomerangStunSound.UnPause();
            enemyBoomerangSpawnedStunSound.UnPause();
            enemyGrounded1SpawnStunSound.UnPause();
            enemyGrounded2SpawnStunSound.UnPause();
            bossStunSound.UnPause();

        }

        if (bossKilled && !musicRestarted)
        {
            fightMusic.Stop();
            backroundSound.Play();
            gateAnimator.SetTrigger("TriggerDoor");
            musicRestarted = true;
        }
    }

    public void RestartGame()
    {
        gameOverMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        fightMusic.Stop();
        gameIsOver = true;
        gameOverObjectLevel3.SetActive(true);
        backroundSound.Stop();
        Time.timeScale = 0;
        gameOverMusic.Play();
        SelectPlayAgainButton();
        SetExitGameOverButton();
    }

    public void SelectPlayAgainButton()
    {
        color = GameObject.FindGameObjectWithTag("PlayAgainButtonLevel3").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PlayAgainButtonLevel3").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PlayAgainButtonLevel3").GetComponent<Button>().colors = color;
    }

    public void SetExitGameOverButton()
    {
        color = GameObject.FindGameObjectWithTag("ExitGameOverButtonLevel3").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("ExitGameOverButtonLevel3").GetComponent<Button>().colors = color;
    }

    public void SelectRestartButton()
    {
        color = GameObject.FindGameObjectWithTag("PauseButtonLevel3").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;

        GameObject.FindGameObjectWithTag("PauseButtonLevel3").GetComponent<Button>().Select();
        GameObject.FindGameObjectWithTag("PauseButtonLevel3").GetComponent<Button>().colors = color;
    }

    public void SetExitButtonSelectedColor()
    {
        color = GameObject.FindGameObjectWithTag("ExitTitleScreenButtonLevel3").GetComponent<Button>().colors;
        color.selectedColor = Color.yellow;
        GameObject.FindGameObjectWithTag("ExitTitleScreenButtonLevel3").GetComponent<Button>().colors = color;
    }

    public void MoveToScene(int sceneId)
    {
        //cand dau load la un scene, pun pozitia inapoi ca fiind cea normala, de la inceputul nivelului
        PlayerPrefs.SetFloat("playerXLevel3", -46f);
        PlayerPrefs.SetFloat("playerYLevel3", -16.6f);
        PlayerPrefs.SetFloat("playerZLevel3", playerTransform.position.z);
        PlayerPrefs.SetInt("chest1Opened", 0);
        PlayerPrefs.SetInt("chest2Opened", 0);
        PlayerPrefs.SetInt("chest3Opened", 0);
        PlayerPrefs.SetFloat("box1X", 247.3f);
        PlayerPrefs.SetFloat("box1Y", 22.3f);
        PlayerPrefs.SetFloat("box1Z", box1Transform.position.z);
        PlayerPrefs.SetFloat("box2X", 602.3f);
        PlayerPrefs.SetFloat("box2Y", 27.47f);
        PlayerPrefs.SetFloat("box2Z", box1Transform.position.z);
        PlayerPrefs.SetFloat("box3X", 685.8f);
        PlayerPrefs.SetFloat("box3Y", 16.61f);
        PlayerPrefs.SetFloat("box3Z", box1Transform.position.z);
        PlayerPrefs.SetInt("checkpointReachedLevel3", 0);

        if (sceneId != 0)
        {
            PlayerPrefs.SetInt("LastFinishedLevel", 3);
            PlayerPrefs.SetInt("CollectedChestsLevel3", (GameObject.FindGameObjectWithTag("Chest1Level3").GetComponent<Chest1ScriptLevel3>().isOpened ? 1 : 0)
                + (GameObject.FindGameObjectWithTag("Chest2Level3").GetComponent<Chest2Level3Script>().isOpened ? 1 : 0)
                + (GameObject.FindGameObjectWithTag("Chest3Level3").GetComponent<Chest3ScriptLevel3>().isOpened ? 1 : 0));
            PlayerPrefs.SetInt("CollectedChest1Level3", GameObject.FindGameObjectWithTag("Chest1Level3").GetComponent<Chest1ScriptLevel3>().isOpened ? 1 : 0);
            PlayerPrefs.SetInt("CollectedChest2Level3", GameObject.FindGameObjectWithTag("Chest2Level3").GetComponent<Chest2Level3Script>().isOpened ? 1 : 0);
            PlayerPrefs.SetInt("CollectedChest3Level3", GameObject.FindGameObjectWithTag("Chest3Level3").GetComponent<Chest3ScriptLevel3>().isOpened ? 1 : 0);
        }


        //nu pot salva bool, deci cand booleanul e true pun 1, altfel 0
        //il voi citi asa:
        //bool achievedRangedWeapon = (PlayerPrefs.GetInt("RangeWeapon") != 0);
        //SceneManager.LoadScene(sceneId);
        Time.timeScale = 1;
        levelChangerScriptLevel3.FadeToScene(sceneId);
    }

    public void PlayFightMusic()
    {
        //Debug.Log("A intrat aici");
        fightMusic.Play();
        backroundSound.Stop();
    }

    public void SetPlayerPrefsForCheckpoint1()
    {
        PlayerPrefs.SetFloat("playerXLevel3", playerTransform.position.x);
        PlayerPrefs.SetFloat("playerYLevel3", playerTransform.position.y);
        PlayerPrefs.SetFloat("playerZLevel3", playerTransform.position.z);
        if (GameObject.FindGameObjectWithTag("Chest1Level3").GetComponent<Chest1ScriptLevel3>().isOpened)
        {
            PlayerPrefs.SetInt("chest1Opened", 1);
        }
        else
        {
            PlayerPrefs.SetInt("chest1Opened", 0);
        }
        if (GameObject.FindGameObjectWithTag("Chest2Level3").GetComponent<Chest2Level3Script>().isOpened)
        {
            PlayerPrefs.SetInt("chest2Opened", 1);
        }
        else
        {
            PlayerPrefs.SetInt("chest2Opened", 0);
        }
        PlayerPrefs.SetFloat("box1X", box1Transform.position.x);
        PlayerPrefs.SetFloat("box1Y", box1Transform.position.y);
        PlayerPrefs.SetFloat("box1Z", box1Transform.position.z);
        PlayerPrefs.SetInt("checkpointReachedLevel3", 1);
        Debug.Log(box1Transform.position.y);
    }

    public void SetPlayerPrefsForCheckpoint2()
    {
        PlayerPrefs.SetFloat("playerXLevel3", playerTransform.position.x);
        PlayerPrefs.SetFloat("playerYLevel3", playerTransform.position.y);
        PlayerPrefs.SetFloat("playerZLevel3", playerTransform.position.z);

        PlayerPrefs.SetFloat("box2X", box2Transform.position.x);
        PlayerPrefs.SetFloat("box2Y", box2Transform.position.y);
        PlayerPrefs.SetFloat("box2Z", box2Transform.position.z);

        PlayerPrefs.SetFloat("box3X", box3Transform.position.x);
        PlayerPrefs.SetFloat("box3Y", box3Transform.position.y);
        PlayerPrefs.SetFloat("box3Z", box3Transform.position.z);

        if (GameObject.FindGameObjectWithTag("Chest3Level3").GetComponent<Chest3ScriptLevel3>().isOpened)
        {
            PlayerPrefs.SetInt("chest3Opened", 1);
        }
        else
        {
            PlayerPrefs.SetInt("chest3Opened", 0);
        }

        PlayerPrefs.SetInt("checkpointReachedLevel3", 2);
    }
}
