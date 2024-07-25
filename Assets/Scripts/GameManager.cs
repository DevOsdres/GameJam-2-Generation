using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Referencia al Canvas del menú principal
    public GameObject pauseCanvas; // Referencia al Canvas del menú de pausa
    public PauseMenu pauseMenu; // Referencia al script PauseMenu asignado a PauseCanvas

    private bool isGameRunning = false; // Variable para verificar si el juego está en curso

    void Start()
    {
        ShowMainMenu(); // Muestra el menú principal al iniciar el juego
    }

    void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true); // Activa el Canvas del menú principal
        Time.timeScale = 0f; // Pausa el juego
        Cursor.visible = true; // Hace visible el cursor
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false); // Desactiva el Canvas del menú principal
        Time.timeScale = 1f; // Reanuda el juego
        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
        isGameRunning = true; // Marca el juego como en curso
    }

    public void OpenPauseMenu()
    {
        if (isGameRunning && Time.timeScale == 1f) // Verifica si el juego está en curso y no está en pausa
        {
            pauseMenu.PauseGame(); // Llama a la función para pausar el juego
        }
    }

    void Update()
    {
        if (isGameRunning && Input.GetKeyDown(KeyCode.Escape)) // Verifica si el juego está en curso y se presiona la tecla Escape
        {
            if (Time.timeScale == 1f)
            {
                OpenPauseMenu(); // Abre el menú de pausa si el juego está corriendo
            }
            else
            {
                pauseMenu.ResumeGame(); // Reanuda el juego si está en pausa
            }
        }
    }

    public void RestartGame()
    {
        isGameRunning = false; // Marca el juego como no en curso
        Time.timeScale = 1f; // Asegura que el tiempo de juego esté en marcha
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }
}