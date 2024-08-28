using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform teleportTarget; // Variabile per la posizione del teletrasporto
    private GameObject player; // Variabile per il giocatore da teletrasportare   

    public bool ToPlaneDungeon;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Teleport pressing E !!!!");
            EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
            foreach (EnemyMovement enemy in enemies)
            {
                enemy.ResetPosition(); // Chiama il metodo di reset su ogni nemico trovato
            }

            // Teleporting Player to TeleportTarget
            player.transform.position = teleportTarget.transform.position;

            // Rotate Player if is necessary
            player.transform.rotation = ToPlaneDungeon ? Quaternion.Euler(0.0f, 120, 0.0f) : Quaternion.Euler(0.0f, 90, 0.0f);
        }
    }

}



