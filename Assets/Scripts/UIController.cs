using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    public TMP_Text TimerText;
    public TMP_Text ScoreText;
    public TMP_Text LevelText;
    //public TMP_Text BooksText;
    public List<GameObject> LifeIcons = new List<GameObject>();
    public TMP_Text FinaleScoreText;

    public GameObject PauseGamePanel;
    public GameObject AudioSettingsPanel;
    public GameObject GameOverPanel;
    public GameObject LevelUP;
    public GameObject TimeOut;

    public GameObject GameFinalPanel;

    public static UIController Instance
    {
        get
        {
            // Se non esiste già un'istanza, cerca nel gioco
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIController>();

                // Se non è stato trovato, crea un nuovo oggetto GameController
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("UIController");
                    _instance = singletonObject.AddComponent<UIController>();
                    singletonObject.tag = "UIController"; // Assegna il tag "UIController"
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
            // Se esiste già un'istanza singleton, distruggi questo oggetto
            Destroy(gameObject);
        }
    }

    public void UpdateFinalScore(int _value)
    {
        FinaleScoreText.text = "FINAL SCORE : " + _value.ToString();
    }

    public void UpdateScoreText(int _value)
    {
        ScoreText.text = "SCORE : " + _value.ToString();
    }

    public void UpdateLevel2Text()
    {
        LevelText.text = "LEVEL 2";
    }
    public void UpdateLevel3Text()
    {
        LevelText.text = "FINAL LEVEL";
    }

    public void UpdateBookstext(int _value)
    {
        //BooksText.text = _value.ToString() + "/3";
    }

    public void UpdateLives(int lives)
    {
        // Assicurati che LifeIcons sia stato inizializzato e che abbia abbastanza elementi per rappresentare tutte le vite
        if (LifeIcons != null && LifeIcons.Count >= lives)
        {
            // Disattiva le icone delle vite extra (se ce ne sono)
            for (int i = 0; i < LifeIcons.Count; i++)
            {
                if (i >= lives)
                {
                    LifeIcons[i].SetActive(false);
                }
                else
                {
                    LifeIcons[i].SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("LifeIcons non inizializzato o non contiene abbastanza elementi.");
        }
    }
    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverPanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        PauseGamePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        PauseGamePanel.SetActive(false);
    }

    public void ShowAudioSettings()
    {
        AudioSettingsPanel.SetActive(true);
    }
    public void HideAudioSettings()
    {
        AudioSettingsPanel.SetActive(false);
    }

    public void ShowWinLevel()
    {
        LevelUP.SetActive(true);
    }
    public void HideWinLevel()
    {
        LevelUP.SetActive(false);
    }
    public void ShowGameFinal()
    {
        GameFinalPanel.SetActive(true);
    }
    public void HideGranFinal()
    {
        GameFinalPanel.SetActive(false);
    }
    public void DestroyUIController()
    {
        Destroy(gameObject);
    }

}
