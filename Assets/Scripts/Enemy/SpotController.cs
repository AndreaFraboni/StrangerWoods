using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpotController : MonoBehaviour
{
    public EnemyFieldOfView enemyFieldOfView;
    public EnemyMovement enemyMovement;
    private Transform cameraTransform;
    public GameObject bianco;
    public GameObject giallo;
    public GameObject rosso;
    //[SerializeField] private SpriteRenderer punto;
    //[SerializeField] private new CinemachineVirtualCamera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;
    //public void UpdateHealthBar(float currentValue, float maxValue)
    //{
    //    slider.value = currentValue / maxValue;
    //}
    void Start()
    {
        // Trova la telecamera nella scena
        cameraTransform = Camera.main.transform;
        bianco.SetActive(false);
        giallo.SetActive(false);
        rosso.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
        //transform.position = target.position + offset;
        if (enemyFieldOfView.IsPlayerVisible)
        {
            bianco.SetActive (true);
            giallo.SetActive (false);
        }
        else
        {
            bianco.SetActive (false);
        }
        if (enemyFieldOfView.playerSeen)
        {
            bianco.SetActive(false);
            giallo.SetActive (false);
            rosso.SetActive(true);
        }
        if (enemyFieldOfView.PlayerNear && !enemyFieldOfView.IsPlayerVisible && !enemyFieldOfView.playerSeen)
        {
            giallo.SetActive(true);
        }
        else
        {
            giallo.SetActive (false);
        }
        if (enemyMovement.resetting)
        {
            bianco.SetActive(false);
            giallo.SetActive(false);
            rosso.SetActive(false);
            enemyMovement.resetting = false;
        }
    }
}
