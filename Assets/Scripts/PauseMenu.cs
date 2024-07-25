using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas; // Referencia al Canvas del menú de pausa
    public GameObject optionsPanel; // Referencia al panel de opciones en el menú de pausa
    public GameManager gameManager; // Referencia al GameManager para poder reiniciar el juego

    void Start()
    {
        pauseCanvas.SetActive(false); // Asegura que el menú de pausa esté desactivado al iniciar el juego
    }

    public void PauseGame()
    {
        pauseCanvas.SetActive(true); // Activa el Canvas del menú de pausa
        Time.timeScale = 0f; // Pausa el tiempo de juego
        Cursor.visible = true; // Hace visible el cursor
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false); // Desactiva el Canvas del menú de pausa
        Time.timeScale = 1f; // Reanuda el tiempo de juego
        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
    }

    public void RestartGame()
    {
        ResumeGame(); // Reanuda el juego antes de reiniciar
        gameManager.RestartGame(); // Llama a la función para reiniciar el juego en el GameManager
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true); // Activa el panel de opciones en el menú de pausa
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false); // Desactiva el panel de opciones en el menú de pausa
    }
}