using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FoodCollector : MonoBehaviour
{
    public GameObject levelCompleteCanvas; // Canvas que muestra al completar el nivel
    public Button returnHomeButton; // Botón para regresar al hogar
    private int foodRequired = 10; // Cantidad de comida requerida
    private int foodCollected = 0; // Cantidad de comida recolectada

    void Start()
    {
        foodCollected = 0; // Reiniciar el contador de comida recolectada al inicio de la escena

        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.SetActive(false); // Esconder el Canvas al inicio
        }
        else
        {
            Debug.LogError("Level Complete Canvas not assigned in the inspector.");
        }

        if (returnHomeButton != null)
        {
            returnHomeButton.onClick.AddListener(ReturnHome);
        }
        else
        {
            Debug.LogError("Return Home Button not assigned in the inspector.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name); // Depuración: Nombre del objeto con el que se colisiona

        if (other.CompareTag("Food"))
        {
            foodCollected += 10; // Cada objeto comida vale 10 puntos
            Debug.Log("Food collected: " + foodCollected);
            Destroy(other.gameObject);

            if (foodCollected >= foodRequired)
            {
                Time.timeScale = 0f; // Pausar el juego
                LevelCompleted();
            }
        }
        else
        {
            Debug.Log("Collided object is not tagged as Food");
        }
    }

    void LevelCompleted()
    {
        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Level Complete Canvas not assigned in the inspector.");
        }

        PlayerProgress playerProgress = FindObjectOfType<PlayerProgress>();
        if (playerProgress != null)
        {
            playerProgress.LevelCompleted();
        }
        else
        {
            Debug.LogError("PlayerProgress script not found in the scene.");
        }
    }

    void ReturnHome()
    {
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
        SceneManager.LoadScene("Home");
    }
}