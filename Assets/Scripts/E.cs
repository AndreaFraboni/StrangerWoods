using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class E : MonoBehaviour
{
    private Transform cameraTransform;
    public GameObject eInput;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        eInput.SetActive(false);
        eInput.transform.LookAt(cameraTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        eInput.transform.LookAt(cameraTransform);
        eInput.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        eInput.SetActive(true);
        // Trova tutti gli oggetti con il tag "Enemy" nella scena e chiama il metodo di reset

    }
    private void OnTriggerExit(Collider other)
    {
        eInput.SetActive(false);
    }
}
