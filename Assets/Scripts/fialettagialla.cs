using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fialettagialla : MonoBehaviour
{
    public float RotationSpeed;

    void Update()
    {
        transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

                other.gameObject.GetComponent<PlayerController>().GetFialettaGialla();
                Destroy(gameObject);

        }
    }
}
