using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TileManager : MonoBehaviour
{
    public int NumOfTilesCounter = 0; // Conteggia numero Tiles generati

    public GameObject StartTilePrefab;
    public GameObject[] tilePrefabs;
    public GameObject FinalPrefab;

    private float xSpawn = 0;
    public float tileLenght = 90;
    public int numberOfTiles;
    public List<GameObject> activeTiles = new List<GameObject>();

    public GameObject player;
    public Transform playerTransform;
    public Transform TileTransform;
    public Transform SpawnerTarget;

    public NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        SpawnTile(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.x - 150 > xSpawn - (numberOfTiles * tileLenght))
        {
            //Debug.Log("TILE  UPDATE");
            if (NumOfTilesCounter == 1 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(Random.Range(1, 2)); }
            if (NumOfTilesCounter == 2 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(3); }
            if (NumOfTilesCounter == 3 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(4); } // ENTRI NEL LIVELLO 2 !!
            if (NumOfTilesCounter == 4 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(Random.Range(5, 6)); }
            if (NumOfTilesCounter == 5 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(7); }
            if (NumOfTilesCounter == 6 && playerTransform.position.x > xSpawn - (numberOfTiles * tileLenght)) { SpawnTile(8); }
        }

        if (player.transform.position.x > (tileLenght * 3)) GameController.Instance.LevelUp(2);
        if (player.transform.position.x > (tileLenght * 6)) GameController.Instance.LevelUp(3);
    }

    public void SpawnTile(int tileIndex)
    {
        if (tileIndex == 3)
        {
            Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
            GameObject go = Instantiate(tilePrefabs[tileIndex], transform.right * (xSpawn + tileLenght), rotation);
            activeTiles.Add(go);

        }
        else
        {
            GameObject go = Instantiate(tilePrefabs[tileIndex], transform.right * xSpawn, transform.rotation);
            activeTiles.Add(go);
        }
        //surface.BuildNavMesh();
        xSpawn += tileLenght;
        NumOfTilesCounter++;
    }

    public void RestartLevel()
    {
        if (player.transform.position.x < (tileLenght * 3)) // LEVEL 1
        {
            player.transform.position = SpawnerTarget.transform.position;
            player.GetComponent<PlayerController>().RespawnPlayer();
            xSpawn = 0;
            NumOfTilesCounter = 0;

            // Distruggi tutti i tile attivi
            foreach (GameObject tile in activeTiles)
            {
                Destroy(tile);
            }
            activeTiles.Clear(); // Cleaning List of Tiles
            SpawnTile(0);
        }

        if (player.transform.position.x >= (tileLenght * 3) && player.transform.position.x < tileLenght * 5) // LEVEL 2
        {
            Vector3 CurrentPosition = player.transform.position;
            Vector3 NewPos = SpawnerTarget.transform.position + Vector3.right * (tileLenght * 3);
            player.transform.position = NewPos;
            player.GetComponent<PlayerController>().RespawnPlayer();
            xSpawn = tileLenght * 3;
            NumOfTilesCounter = 4;

            // Distruggi tutti i tile attivi
            foreach (GameObject tile in activeTiles)
            {
                Destroy(tile);
            }
            activeTiles.Clear(); // Cleaning List of Tiles
            SpawnTile(4);
        }

        if (player.transform.position.x >= (tileLenght * 5)) // LEVEL 3 FINALE
        {
            Vector3 CurrentPosition = player.transform.position;
            Vector3 NewPos = SpawnerTarget.transform.position + Vector3.right * (tileLenght * 5);
            player.transform.position = NewPos;
            player.GetComponent<PlayerController>().RespawnPlayer();
            xSpawn = tileLenght * 5;
            NumOfTilesCounter = 6;

            // Distruggi tutti i tile attivi
            foreach (GameObject tile in activeTiles)
            {
                Destroy(tile);
            }
            activeTiles.Clear(); // Cleaning List of Tiles
            SpawnTile(8);
        }
    }

}

