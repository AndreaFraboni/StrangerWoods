using UnityEngine;

public class Attack : MonoBehaviour
{
    // Funzione chiamata quando un oggetto collidere con il personaggio
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("IL LUPO STA SUBENDO ATTACCO E DANNO !!!!");

        // Verifica se l'oggetto in collisione ha un tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {

        }
    }

}
