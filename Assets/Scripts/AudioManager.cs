using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public Sound[] musicSounds, sfxSounds;
    public Sound[] sfxWolfSounds, sfxItemsSound, sfxGameSound;
    public AudioSource musicSource, sfxSource, sfxWolfSource, sfxItemsSource, sfxGameSource;

    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public float musicVolume;
        public float sfxVolume;
    }
    PlayerData mPlayerData = new PlayerData();

    public static AudioManager Instance
    {
        get
        {
            // Se non esiste già un'istanza, cerca nel gioco
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                // Se non è stato trovato, crea un nuovo oggetto AudioManager
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("AudioManager");
                    _instance = singletonObject.AddComponent<AudioManager>();
                    singletonObject.tag = "AudioManager"; // Assegna il tag "AudioManager"
                }
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadSaveDataPlayer();
        MusicVolume(mPlayerData.musicVolume);
        SFXVolume(mPlayerData.sfxVolume);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (musicSource.isPlaying) musicSource.Stop();
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (sfxSource.isPlaying) sfxSource.Stop();
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayWolfSFX(string name)
    {
        Sound s = Array.Find(sfxWolfSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (sfxWolfSource.isPlaying) sfxWolfSource.Stop();
            sfxWolfSource.PlayOneShot(s.clip);
        }
    }
    public void PlayItemsSFX(string name)
    {
        Sound s = Array.Find(sfxItemsSound, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (sfxItemsSource.isPlaying) sfxItemsSource.Stop();
            sfxItemsSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySoundWolfFXClip(string name, Transform spawnTransform)
    {
        Sound s = Array.Find(sfxWolfSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (sfxWolfSource.isPlaying) sfxWolfSource.Stop();
            AudioSource audioSource = Instantiate(sfxWolfSource, spawnTransform.position, Quaternion.identity);
            audioSource.clip = s.clip;
            audioSource.volume = sfxWolfSource.volume;
            audioSource.Play();
            float clipLenght = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLenght);
        }
    }

    public void PlaySoundPlayerFXClip(string name, Transform spawnTransform)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (sfxSource.isPlaying) sfxSource.Stop();
            AudioSource audioSource = Instantiate(sfxSource, spawnTransform.position, Quaternion.identity);
            audioSource.clip = s.clip;
            audioSource.volume = sfxSource.volume;
            audioSource.Play();
            float clipLenght = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLenght);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        sfxWolfSource.mute = !sfxWolfSource.mute;
        sfxItemsSource.mute = !sfxItemsSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        sfxWolfSource.volume = volume;
        sfxItemsSource.volume = volume;
    }

    public void StopSources()
    {
        if (musicSource.isPlaying) musicSource.Stop();
        if (sfxSource.isPlaying) sfxSource.Stop();
        if (sfxWolfSource.isPlaying) sfxWolfSource.Stop();
        if (sfxItemsSource.isPlaying) sfxItemsSource.Stop();
        if (sfxGameSource.isPlaying) sfxGameSource.Stop();
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

    public void DestroyAudioManager()
    {
        Destroy(gameObject);
    }

}
