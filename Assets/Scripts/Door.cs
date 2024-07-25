using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Canvas confirmationCanvas; // Canvas de confirmación
    public TextMeshProUGUI confirmationText; // Texto en el Canvas
    public Button acceptButton; // Botón de aceptar
    public Button cancelButton; // Botón de cancelar

    private PlayerProgress playerProgress; // Progreso del jugador

    void Start()
    {
        confirmationCanvas.gameObject.SetActive(false); // Esconde el Canvas al inicio
        playerProgress = FindObjectOfType<PlayerProgress>(); // Encuentra el script PlayerProgress en la escena

        acceptButton.onClick.AddListener(OnAccept);
        cancelButton.onClick.AddListener(OnCancel);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 0f; // Pausar el juego
            ShowConfirmation();
        }
    }

    void ShowConfirmation()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        int targetLevel = playerProgress.currentLevel + 1; // Determinar automáticamente el nivel objetivo
        confirmationCanvas.gameObject.SetActive(true);
        confirmationText.text = $"Enter to Level {targetLevel}. You are about to leave your home, you will leave your children without knowing if you will return, are you sure about this?";
    }

    void OnAccept()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
        int targetLevel = playerProgress.currentLevel + 1; // Determinar automáticamente el nivel objetivo
        if (playerProgress.currentLevel == targetLevel - 1)
        {
            SceneManager.LoadScene($"Level{targetLevel}");
        }
        else
        {
            Debug.Log("Cannot load the level. Current level progress does not match the target level.");
        }
    }

    void OnCancel()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
}