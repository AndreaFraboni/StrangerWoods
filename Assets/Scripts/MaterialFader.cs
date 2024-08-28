using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material[] Mats;
    public bool Dofade = false;

    // Start is called before the first frame update
    void Start()
    {
        Mats = GetComponent<Renderer>().materials;
        foreach (Material mat in Mats)
        {
            originalOpacity = mat.color.a;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Dofade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    void FadeNow()
    {
        foreach (Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            smoothColor.a = 0.3f;
            mat.color = smoothColor;
        }
    }

    void ResetFade()
    {
        foreach (Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                                          Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().isInBushes = true;
            Dofade = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().isInBushes = true;
            Dofade = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().isInBushes = false;
            Dofade = false;
        }
    }

}
