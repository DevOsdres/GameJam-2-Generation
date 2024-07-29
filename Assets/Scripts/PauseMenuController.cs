using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuCanvas;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuCanvas.SetActive(true);
        isPaused = true;
    }

    public void Play()
    {
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    /*public void MainMenu()
    {
        SceneManager.LoadScene(0); // Cargar la escena principal (menú)
    }*/

    public void RestartGame()
    {
        PlayerProgress.Instance.ResetProgress(); // Reiniciar el progreso del jugador
        SceneManager.LoadScene(0); // Cargar la escena principal (menú)
    }
}