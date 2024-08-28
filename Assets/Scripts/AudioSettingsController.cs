using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AudioSettingsController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public float musicVolume;
        public float sfxVolume;
    }
    PlayerData mPlayerData = new PlayerData();

    public Slider _musicSlider, _sfxSlider;

    private void Awake()
    {
        LoadSaveDataPlayer();

        AudioManager.Instance.MusicVolume(mPlayerData.musicVolume);
        AudioManager.Instance.SFXVolume(mPlayerData.sfxVolume);
        _musicSlider.value = AudioManager.Instance.musicSource.volume;
        _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
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
