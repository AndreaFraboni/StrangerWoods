using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollisionObject : MonoBehaviour
{
    // Start is called before the first frame update

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION ENTER");
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TIGGER COLLISION");
        Debug.Log("nome =>" + other.tag);

    }

}
