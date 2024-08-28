using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    public Material DarkSkyboxMaterial;
    public GameObject DarkDirectionalLight;
    public Material SunsetSkyboxMaterial;
    public GameObject SunsetDirectionalLight;

    public void SetSkyBoxDark()
    {
        SunsetDirectionalLight.SetActive(false);
        DarkDirectionalLight.SetActive(true);
        RenderSettings.skybox = DarkSkyboxMaterial;
        RenderSettings.subtractiveShadowColor = new Color(0, 57, 157, 255);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(63, 100, 132, 255);
        RenderSettings.fogDensity = 0.05f;
    }

    public void SetSunsetSkybox()
    {
        DarkDirectionalLight.SetActive(false);
        SunsetDirectionalLight.SetActive(true);
        RenderSettings.skybox = SunsetSkyboxMaterial;
        RenderSettings.subtractiveShadowColor = new Color(0, 31, 116, 255);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(255, 83, 30, 255);
        RenderSettings.fogDensity = 0.014f;
    }

}
