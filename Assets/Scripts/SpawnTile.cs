using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnTile : MonoBehaviour
{
    public Transform TileTransform;
    public Vector3 TilePosition;
    public Quaternion TileRotation;

    private void Awake()
    {
        TileTransform = gameObject.transform;
        TilePosition = TileTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    GameController.Instance.CheckLevelFinished();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{

        //}
    }


}
