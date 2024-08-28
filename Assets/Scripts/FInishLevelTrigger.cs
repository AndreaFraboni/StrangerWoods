using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishLevelTrigger : MonoBehaviour
{
    public Transform teleportTarget; //Variable for TP position
    public GameObject player; //Variable for teleporting P
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = teleportTarget.transform.position;
            GameController.Instance.LevelFinished();
        }
    }



}
