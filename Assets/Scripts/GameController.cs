using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using Cinemachine;
using Unity.VisualScripting;
using System.Net.Http.Headers;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    private CinemachineVirtualCamera cinemaVirtualCamera;

    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public float musicVolume;
        public float sfxVolume;
    }
    PlayerData mPlayerData = new PlayerData();

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
    Highscores mHighscores = new Highscores();
    private List<Transform> highscoreEntryTransformList;

    private bool isPlaying = true;
    public bool IsPlaying { get { return isPlaying; } }
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    public int CurrentScore = 0;
    public int Lives = 3;
    public int MaxLives = 3;
    public int CurrentBooks = 0;

    private float CountDown = 600f;// Durata del timer in secondi = 10 minuti
    private float tempoTrascorso;

    public int CurrentLevel = 1;
    public int LevelToDeactive;
    private bool CurrentLevelFinished = false;

    public UIController UIController;
    public GameObject TileManager;

    public Material DarkSkyboxMaterial;
    public GameObject DarkDirectionalLight;
    public Material SunsetSkyboxMaterial;          // not used in this version 
    public GameObject SunsetDirectionalLight;      // not used in this version 

    public bool GameFinished = false;

    //************************************************************************************//
    //************************** GAMECONTROLLER SINGLETON ********************************//
    //************************************************************************************//
    public static GameController Instance
    {
        get
        {
            // Se non esiste già un'istanza, cerca nel gioco
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();

                // Se non è stato trovato, crea un nuovo oggetto GameController
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameController");
                    _instance = singletonObject.AddComponent<GameController>();
                    singletonObject.tag = "GameController"; // Assegna il tag "GameController"
                }
            }

            return _instance;
        }
    }

    //************************************************************************************//
    //********************** PLAYER COLLIDE ENEMY ****************************************//
    //************************************************************************************//
    public void PlayerCollidedWithEnemy()
    {
        Debug.Log("Il giocatore ha colpito un nemico!");
        // Puoi aggiungere qui la logica per gestire l'evento quando il giocatore colpisce un nemico
    }

    //************************************************************************************//
    //************** DON'T DESTROY SINGLETON ON LOAD LEVEL *******************************//
    //************************************************************************************//
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se esiste già un'istanza singleton, distruggi questo oggetto
            Destroy(gameObject);
        }

        tempoTrascorso = CountDown;
        LoadSaveDataPlayer(); // Carica o Salva/Crea DATA Player
        LoadSaveLeaderboard(); // Carica Ledearboard
        GameFinished = false;
        isPlaying = true;
        isPaused = false;
        Time.timeScale = 1;

        AudioManager.Instance.PlayMusic("ThemeGame");
    }

    //*******************************************************************************************************//
    //************************* FIXED UPDATE ****************************************************************//
    //*******************************************************************************************************//
    private void FixedUpdate()
    {
        tempoTrascorso -= Time.deltaTime;
        Timer();

        //if (tempoTrascorso == 200)
        //{
        //    AudioManager.Instance.PlayItemsSFX("Bells");
        //}
        //if (tempoTrascorso == 100)
        //{
        //    AudioManager.Instance.PlayItemsSFX("Bells");
        //}

        if (tempoTrascorso <= 0)
        {
            // Metti il gameover
            tempoTrascorso = 0;
            Invoke("GameOver", 1);
        }
    }

    //*******************************************************************************************************//
    //************************* TIMER ***********************************************************************//
    //*******************************************************************************************************//
    private void Timer()
    {
        int secondiTrascorsi = Mathf.FloorToInt(tempoTrascorso);
        UIController.TimerText.text = "Countdown : " + secondiTrascorsi + "s";
    }

    //************************************************************************************//
    //****************************** SETTING SKYBOX **************************************//
    //************************************************************************************//
    public void SetSkyBoxDark()
    {
        SunsetDirectionalLight.SetActive(false);
        DarkDirectionalLight.SetActive(true);
        RenderSettings.skybox = DarkSkyboxMaterial;
        //RenderSettings.subtractiveShadowColor = new Color(0, 57, 157, 255);
        //RenderSettings.fog = true;
        //RenderSettings.fogColor = new Color(63, 100, 132, 255);
        //RenderSettings.fogDensity = 0.05f;
    }

    public void SetSunsetSkybox()
    {
        DarkDirectionalLight.SetActive(false);
        SunsetDirectionalLight.SetActive(true);
        RenderSettings.skybox = SunsetSkyboxMaterial;
        //RenderSettings.subtractiveShadowColor = new Color(0, 52, 191, 255);
        //RenderSettings.fog = false;
        //RenderSettings.fogColor = new Color(255, 83, 30, 255);
        //RenderSettings.fogDensity = 0.014f;
    }

    //*******************************************************************************************************//
    //************************* PLAYER ADD SCORE ************************************************************//
    //*******************************************************************************************************//
    public void AddScore(int _value)
    {
        CurrentScore += _value;
        UIController.UpdateScoreText(CurrentScore);
    }

    public void AddCoins(int value)
    {
        CurrentScore += value;
        UIController.UpdateScoreText(CurrentScore);
    }

    //*******************************************************************************************************//
    //************************* ADDING BOOKS : NOT USED IN THIS VERSION *************************************//
    //*******************************************************************************************************//
    public void AddBooks(int value)
    {
        CurrentBooks += 1;
        if (CurrentBooks > 3) CurrentBooks = 3;
        UIController.UpdateBookstext(CurrentBooks);
    }

    //*******************************************************************************************************//
    //************************* PLAYER LIVE LOST ************************************************************//
    //*******************************************************************************************************//
    public void LiveLost()
    {
        // lose life
        Lives = Lives - 1;

        UIController.UpdateLives(Lives);

        //if (Lives < 0) // not used in this version
        //{
        //   Invoke("GameOver", 1);
        //}
    }

    public void LiveUp()
    {
        Lives = Lives + 1;
        if (Lives > MaxLives) Lives = MaxLives;
        UIController.UpdateLives(Lives);
    }

    public void PlayerCallGameOver()
    {
        Invoke("GameOver", 1);
        //Destroy(gameObject);
        //SavePlayerData();
        //AudioManager.Instance.musicSource.Stop();
        //SceneManager.LoadScene("MainMenu");
    }

    //*******************************************************************************************************//
    //************************* LEVEL FINISHED **************************************************************//
    //*******************************************************************************************************//
    public void CheckLevelFinished()
    {
        if (CurrentLevelFinished)
        {
            Debug.Log("START TILEMANAGER FOR NEW LEVEL .....");
            CurrentLevelFinished = false;
        }
    }

    public void LevelFinished()
    {
        CurrentLevelFinished = true;
        Debug.Log("FINE LIVELLO !!!");
    }

    public void LevelUp(int Value)
    {
        if (Value == 2)
        {
            UIController.UpdateLevel2Text();
        }
        if (Value == 3)
        {
            UIController.UpdateLevel3Text();
        }

    }
    //*******************************************************************************************************//
    //************************** PLAYER WIN THE GAME !!!! ***************************************************//
    //*******************************************************************************************************//
    public void GranFinal()
    {
        if (!GameFinished)
        {
            GameFinished = true;
            AudioManager.Instance.PlayMusic("FinalMusic");
            AddScore(500);
            UIController.ShowGameFinal();
            UIController.UpdateFinalScore(CurrentScore);
            AddHighscoreEntry(CurrentScore, mPlayerData.UserName);
            SavePlayerData();
            isPlaying = false;
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void GameFinaleExitButton()
    {
        AudioManager.Instance.StopSources();
        // UIController.HideGranFinal();
        UIController.DestroyUIController();
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    //*******************************************************************************************************//
    //*************************** RESPAWN PLAYER ************************************************************//
    //*******************************************************************************************************//
    public void RespawnPlayer()
    {
        TileManager.GetComponent<TileManager>().RestartLevel();
    }

    //*******************************************************************************************************//
    //************************* MANAGE GAME OVER !!! ********************************************************//
    //*******************************************************************************************************//
    public void GameOverGameExitButton()
    {
        UIController.HideGameOver();
        UIController.DestroyUIController();
        Destroy(gameObject);
        SavePlayerData();
        AudioManager.Instance.musicSource.Stop();
        SceneManager.LoadScene("MainMenu");
    }
    public void ReStartGameOverBUtton()
    {
        UIController.HideGameOver();
        UIController.DestroyUIController();
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isPlaying = false;
        isPaused = true;
        Time.timeScale = 0;
        AddHighscoreEntry(CurrentScore, mPlayerData.UserName);
        AudioManager.Instance.PlayMusic("ThemeMenu");
        UIController.ShowGameOver();
    }

    //*******************************************************************************************************//
    //************************* MANAGE UI CALLING ***********************************************************//
    //*******************************************************************************************************//
    public void OnPause(InputAction.CallbackContext context)
    {
        isPlaying = false;
        isPaused = true;
        Time.timeScale = 0;
        UIController.ShowPausePanel();
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        isPlaying = true;
        UIController.HidePausePanel();
    }

    public void PauseGameExitButton()
    {
        UIController.HidePausePanel();
        isPlaying = false;
        isPaused = true;
        UIController.DestroyUIController();
        Destroy(gameObject);
        SavePlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    //**************************************************************************************************//
    //**************************************************************************************************//
    //**************************************************************************************************//
    public void InGameShowAudioSettings()
    {
        UIController.HidePausePanel();
        UIController.ShowAudioSettings();
    }

    public void InGameAudioSettingsBackButton()
    {
        UIController.HideAudioSettings();
        UIController.ShowPausePanel();
        mPlayerData.musicVolume = AudioManager.Instance.musicSource.volume;
        mPlayerData.sfxVolume = AudioManager.Instance.sfxSource.volume;
        SavePlayerData();
    }

    //**************************************************************************************************//
    //******************************** LOAD_SAVE_PLAYER_DATA *******************************************//
    //**************************************************************************************************//
    public void LoadSaveDataPlayer()
    {
        string saveFile = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(saveFile))
        {
            //Debug.Log("FILE EXISTS !!!");
            string loadPlayerData = File.ReadAllText(saveFile);
            mPlayerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
        }
        else
        {
            mPlayerData.UserName = "Player0";
            mPlayerData.musicVolume = 0.5f;
            mPlayerData.sfxVolume = 0.3f;
            //Debug.Log("FILE DOES NOT EXISTS !!!");
            string json = JsonUtility.ToJson(mPlayerData);
            File.WriteAllText(saveFile, json);
        }

    }
    public void SavePlayerData()
    {
        string saveFile = Application.persistentDataPath + "/PlayerData.json";
        string json = JsonUtility.ToJson(mPlayerData);
        File.WriteAllText(saveFile, json);
    }

    //****************************************************************************************//
    //****************************************************************************************//
    //****************************************************************************************//
    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList.Add(highscoreEntry);
        // Save updated Highscores     
        string saveFile = Application.persistentDataPath + "/LeaderboardData.json";
        string json = JsonUtility.ToJson(highscores);
        File.WriteAllText(saveFile, json);
        PlayerPrefs.SetString("highscoreTable", json);
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        //SaveLeaderboard();
    }
    public void LoadSaveLeaderboard()
    {
        string saveFile = Application.persistentDataPath + "/LeaderboardData.json";
        if (File.Exists(saveFile))
        {
            //Debug.Log("GAME CONTROLLER - LoadSaveLeaderboard : FILE EXISTS !!!");
            string loadPlayerData = File.ReadAllText(saveFile);
            Highscores highscores = JsonUtility.FromJson<Highscores>(loadPlayerData);
            PlayerPrefs.SetString("highscoreTable", loadPlayerData);
            //Debug.Log(PlayerPrefs.GetString("highscoreTable"));
            // Sort entry list by score
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                    {
                        // Swap
                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }
        }
        else
        {
            //Debug.Log("GAME CONTROLLER - LoadSaveLeaderboard : FILE DOES NOT EXISTS !!!");
            Highscores highscores = new Highscores();
            highscores.highscoreEntryList = new List<HighscoreEntry>() {
             new HighscoreEntry { score = 0, name = "AAA" }
            };
            string json = JsonUtility.ToJson(highscores);
            File.WriteAllText(saveFile, json);
            PlayerPrefs.SetString("highscoreTable", json);
            //Debug.Log(PlayerPrefs.GetString("highscoreTable"));
            // Sort entry list by score
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                    {
                        // Swap
                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }
        }
    }

}



