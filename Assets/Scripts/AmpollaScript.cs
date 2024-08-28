using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpollaScript : MonoBehaviour
{
    public float coinSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, coinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance.Lives < GameController.Instance.MaxLives)
            {
                other.gameObject.GetComponent<PlayerController>().GetAmpolla();
                Destroy(gameObject);
            }
        }
    }
}
