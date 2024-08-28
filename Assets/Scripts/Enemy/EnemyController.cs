using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefabbricato del nemico da istanziare
    public Transform player; // Riferimento al trasform del giocatore
    public float spawnDistance = 10f; // Distanza dal giocatore in cui istanziare i nemici
    public float spawnInterval = 5f; // Intervallo di tempo tra le istanze di nemici

    private float nextSpawnTime; // Tempo per la prossima istanza di nemico

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval; // Inizializza il tempo per la prossima istanza
    }

    private void Update()
    {
        // Controlla se è il momento di istanziare un nemico
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy(); // Istanzia un nemico
            nextSpawnTime = Time.time + spawnInterval; // Aggiorna il tempo per la prossima istanza
        }
    }

    private void SpawnEnemy()
    {
        // Ottieni la posizione del giocatore e della fotocamera
        Vector3 playerPosition = player.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        // Calcola la distanza tra il giocatore e la schermata a destra
        float distanceToRightScreenEdge = (cameraPosition.x + Camera.main.orthographicSize * Camera.main.aspect) - playerPosition.x;

        // Se il giocatore si trova a destra del bordo destro della schermata, spawnare a destra della schermata,
        // altrimenti spawnare a destra del giocatore
        Vector3 spawnPosition;
        if (distanceToRightScreenEdge < 0)
        {
            spawnPosition = cameraPosition + new Vector3(spawnDistance, 0f, Random.Range(0f, 15f));
        }
        else
        {
            spawnPosition = playerPosition + new Vector3(spawnDistance, 0f, Random.Range(0f, 15f));
        }

        // Imposta la coordinata Y della posizione del nemico alla stessa altezza del giocatore
        spawnPosition.y = playerPosition.y;
        spawnPosition.z = Random.Range(0f, 15f);

        // Istanzia un nemico al spawnPosition
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

}
