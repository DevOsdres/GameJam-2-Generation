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
        confirmationCanvas.gameObject.SetActive(true);
        
        if (PlayerProgress.Instance.currentLevel == 0)
        {
            confirmationText.text = "Enter to Level 1. Are you sure you want to leave home for the first time?";
        }
        else if (PlayerProgress.Instance.currentLevel == 3)
        {
            confirmationText.text = "You have completed all levels. Do you want to end the game and go to GameWin?";
        }
        else
        {
            int targetLevel = PlayerProgress.Instance.currentLevel + 1;
            confirmationText.text = $"Enter to Level {targetLevel}. You are about to leave your home, you will leave your children without knowing if you will return, are you sure about this?";
        }
    }

    void OnAccept()
    {
        Cursor.visible = false;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena

        int targetLevel = PlayerProgress.Instance.currentLevel + 1;

        // Depuración para verificar las condiciones
        Debug.Log("Current Level: " + PlayerProgress.Instance.currentLevel);
        Debug.Log("Has Completed Level: " + PlayerProgress.Instance.hasCompletedLevel);
        Debug.Log("Food Delivered: " + PlayerProgress.Instance.foodDelivered);
        Debug.Log("First Time Leaving Home: " + PlayerProgress.Instance.firstTimeLeavingHome);

        // Permitir avanzar si es la primera vez que se sale del "Home" o si se ha completado el nivel y entregado comida
        if (PlayerProgress.Instance.firstTimeLeavingHome || 
            (PlayerProgress.Instance.hasCompletedLevel && PlayerProgress.Instance.HasDeliveredFood()))
        {
            PlayerProgress.Instance.firstTimeLeavingHome = false; // Solo se debe establecer en falso si se está saliendo por primera vez
            PlayerProgress.Instance.IncrementLevel();
            Debug.Log("Loading Level: " + PlayerProgress.Instance.currentLevel);

            if (PlayerProgress.Instance.currentLevel <= 3)
            {
                // Cargar la siguiente escena según el índice
                SceneManager.LoadScene(targetLevel + 1);
            }
            else
            {
                // Cargar GameWin si se completaron todos los niveles
                SceneManager.LoadScene(6);
            }
        }
        else
        {
            Debug.Log("Cannot load the level. You must complete the level and deliver food to your children before proceeding.");
        }
    }

    void OnCancel()
    {
        Cursor.visible = false;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
}