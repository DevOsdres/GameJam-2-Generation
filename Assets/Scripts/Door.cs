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
    private bool isPlayerNearDoor = false; // Estado para saber si el jugador está cerca de la puerta

    void Start()
    {
        if (confirmationCanvas != null)
        {
            confirmationCanvas.gameObject.SetActive(false); // Esconde el Canvas al inicio
        }
        else
        {
            Debug.LogError("Confirmation Canvas not assigned in the inspector.");
        }

        if (acceptButton != null)
        {
            acceptButton.onClick.AddListener(OnAccept);
        }
        else
        {
            Debug.LogError("Accept Button not assigned in the inspector.");
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnCancel);
        }
        else
        {
            Debug.LogError("Cancel Button not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerNearDoor)
        {
            Time.timeScale = 0f; // Pausar el juego
            ShowConfirmation();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearDoor = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearDoor = false;
        }
    }

    void ShowConfirmation()
    {
        Cursor.visible = true;
        int targetLevel = PlayerProgress.Instance.currentLevel + 1; 
        confirmationCanvas.gameObject.SetActive(true);
        confirmationText.text = $"Enter to Level {targetLevel}. You are about to leave your home, you will leave your children without knowing if you will return, are you sure about this?";
    }

    void OnAccept()
    {
        if (PlayerProgress.Instance.HasDeliveredFood())
        {
            Cursor.visible = false;
            confirmationCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
            int targetLevel = PlayerProgress.Instance.currentLevel + 1; // Determinar automáticamente el nivel objetivo
            SceneManager.LoadScene(targetLevel);
        }
        else
        {
            Debug.Log("Cannot load the level. You must deliver food to your children before proceeding.");
        }
    }

    void OnCancel()
    {
        Cursor.visible = false;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
}