using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Riferimento al giocatore o all'oggetto che la telecamera segue
    public float xOffset = 5f; // Offset orizzontale rispetto al giocatore lungo l'asse X
    public float centerThreshold = 0.5f; // Soglia per determinare quando il giocatore è al centro della telecamera
    public float transitionSpeed = 5f; // Velocità di transizione della telecamera

    private bool isFollowing = false; // Flag per indicare se la telecamera sta seguendo il giocatore

    void LateUpdate()
    {
        if (target != null)
        {
            // Ottieni la direzione del movimento del giocatore lungo l'asse X
            float moveDirection = Input.GetAxisRaw("Horizontal");

            // Se il giocatore è al centro della telecamera
            if (isFollowing && Mathf.Abs(target.position.x - transform.position.x) < centerThreshold)
            {
                isFollowing = false; // Smetti di seguire il giocatore
            }

            // Se la telecamera non sta seguendo il giocatore
            if (!isFollowing)
            {
                // Se il giocatore esce dal centro della telecamera
                if (Mathf.Abs(target.position.x - transform.position.x) > centerThreshold)
                {
                    isFollowing = true; // Inizia a seguire il giocatore
                }
            }

            // Se la telecamera sta seguendo il giocatore
            if (isFollowing)
            {
                // Riprendi a seguire il giocatore solo se si sta muovendo verso destra
                if (moveDirection > 0f)
                {
                    // Calcola la nuova posizione lungo l'asse X basata sulla posizione del target
                    float newXPosition = target.position.x + xOffset;

                    // Interpolazione graduale della posizione della telecamera
                    Vector3 targetPosition = new Vector3(newXPosition, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
                }
                else
                {
                    isFollowing = false; // Smette di seguire il giocatore se si muove verso sinistra
                }
            }
        }
    }
}







