using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class SettingsScript : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public float musicVolume;
        public float sfxVolume;
    }
    PlayerData mPlayerData = new PlayerData();

    public TMP_Text PlayerNameText;
    public TMP_InputField InputFieldObj;

    public void Awake()
    {
        InputFieldObj.ActivateInputField();

        LoadSaveDataPlayer();

        PlayerNameText.text = "CURRENT PLAYER NAME : " + mPlayerData.UserName;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputFieldObj.ActivateInputField();
        InputFieldObj.text = "";
        InputFieldObj.caretBlinkRate = 1000;
    }

    public void ReadTextField(string s)
    {
        string input = s;
        //Debug.Log(input);
        mPlayerData.UserName = input;
        PlayerNameText.text = "PLAYER NAME : " + mPlayerData.UserName;

        SavePlayerData();

        InputFieldObj.ActivateInputField();
    }

    public void BackButtonPressed()
    {
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
}
