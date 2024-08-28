using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CestinoAttack : MonoBehaviour
{
    // Funzione chiamata quando un oggetto collidere con il personaggio
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<<<<<<<<<<<<<<<<<< COLLISIONE CESTINO !!!! >>>>>>>>>>>>>>>");
        // Verifica se l'oggetto in collisione ha un tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("COLLISIONE CESTINO CON : ENEMY !!!!!!");
        }
    }
}
