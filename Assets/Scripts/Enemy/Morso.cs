using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class Morso : MonoBehaviour
{
    public BoxCollider boxColliderTesta;

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit");
            //other.gameObject.GetComponent<PlayerController>().TakeDamage(20);
            // Disattiva il collider della testa
            boxColliderTesta.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit");
            //other.gameObject.GetComponent<PlayerController>().TakeDamage(20);
            // Disattiva il collider della testa
            boxColliderTesta.enabled = false;
        }
    }
}
