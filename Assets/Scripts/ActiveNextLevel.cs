using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNextLevel : MonoBehaviour
{
    public int NextLevel;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //GameController.Instance.ActiveNextLevel(NextLevel);
        }
    }



}
