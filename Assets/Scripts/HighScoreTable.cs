using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreTable : MonoBehaviour
{
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

    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(1000, "KKK");

        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

        LoadSaveLeaderboard();
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 40f;

        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;

        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TMP_Text>().color = Color.green;
        }
        transformList.Add(entryTransform);
    }

    public void LoadSaveLeaderboard()
    {
        string saveFile = Application.persistentDataPath + "/LeaderboardData.json";

        if (File.Exists(saveFile))
        {
            //Debug.Log("LoadSaveLeaderboard : FILE EXISTS !!!");
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

            highscoreEntryTransformList = new List<Transform>();

            // for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            // {
            //     CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            // }
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                if (i <= 10) CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            }
        }
        else
        {
            //Debug.Log("LoadSaveLeaderboard : FILE DOES NOT EXISTS !!!");
            Highscores highscores = new Highscores();

            highscores.highscoreEntryList = new List<HighscoreEntry>() {
             new HighscoreEntry { score = 0, name = "Player" },
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

            highscoreEntryTransformList = new List<Transform>();

            //for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            // {
            //     CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            // }
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                if (highscores.highscoreEntryList[i] != null)
                {
                    if (i <= 10) CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
                }
            }
        }
    }

}
