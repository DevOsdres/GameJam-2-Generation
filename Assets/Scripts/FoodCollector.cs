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
        if (other.CompareTag("Food"))
        {
            foodCollected=10;
            Destroy(other.gameObject);

            if (foodCollected >= foodRequired)
            {
                LevelCompleted();
            }
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
        SceneManager.LoadScene("Home");
    }
}