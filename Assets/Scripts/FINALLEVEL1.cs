using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINALLEVEL1 : MonoBehaviour
{
    private GameObject player; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FINE LIVELLO 1 !!!!");
            GameController.Instance.LevelFinished();
            GameController.Instance.CheckLevelFinished();
        }

    }
}
