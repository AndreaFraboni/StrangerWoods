using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEFINAL : MonoBehaviour
{
    private GameObject player; //Variable for teleporting P

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.GranFinal();
        }
    }
}
