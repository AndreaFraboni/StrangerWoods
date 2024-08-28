using UnityEngine;

public class PGMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocità di movimento del personaggio
    public float maxHealth = 100f; // Salute massima del personaggio
    private float currentHealth; // Salute attuale del personaggio

    private void Start()
    {
        // Imposta la posizione di partenza del giocatore
        transform.position = new Vector3(1f, 1f, 1f);

        currentHealth = maxHealth; // All'inizio, la salute attuale è uguale alla salute massima
    }

    // Update viene chiamato una volta per frame
    void Update()
    {
        // Input assi orizzontali e verticali (WASD o frecce direzionali)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calcoliamo il vettore di movimento combinando i due input
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;

        // Applichiamo il movimento al nostro oggetto (il personaggio del giocatore)
        transform.Translate(movement);
    }



    // Funzione per subire danni
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Sottrai i danni dalla salute attuale

        // Controlla se la salute è inferiore o uguale a zero
        if (currentHealth <= 0)
        {
            Die(); // Se sì, il personaggio muore
        }
    }

    // Funzione per far morire il personaggio
    private void Die()
    {
        // Opzionale: Aggiungi qui effetti visivi o sonori per la morte del personaggio

        Destroy(gameObject); // Distruzione dell'oggetto del personaggio
    }
    // Funzione chiamata quando un oggetto collidere con il personaggio
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Recupera l'oggetto GameController e invia un avviso
            GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            if (gameController != null)
            {
                gameController.PlayerCollidedWithEnemy();
            }
        }
    }
}
