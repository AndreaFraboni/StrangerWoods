using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float RotateSpeed = 1.2f;
    private Material _skybox;

    private void Start()
    {
        _skybox = Instantiate(RenderSettings.skybox);
        RenderSettings.skybox = _skybox;
        AudioManager.Instance.PlayMusic("ThemeMenu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
    }

    private void FixedUpdate()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    public void QuitGame()
    {
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		     Application.Quit();
#endif
        }

    }

}
