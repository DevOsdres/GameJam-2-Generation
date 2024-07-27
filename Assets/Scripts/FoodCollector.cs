using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (other.CompareTag("Food"))
        {
            foodCollected++; // Incrementar el contador de comida recolectada
            Destroy(other.gameObject);

            if (foodCollected >= foodRequired)
            {
                Time.timeScale = 0f; // Pausar el juego
                LevelCompleted();
            }
        }
    }

    void LevelCompleted()
    {
        if (levelCompleteCanvas != null)
        {
            Cursor.visible = true;
            levelCompleteCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Level Complete Canvas not assigned in the inspector.");
        }

        PlayerProgress.Instance.LevelCompleted(); // Registrar la finalización del nivel en el progreso del jugador
        PlayerProgress.Instance.collectedFoodCount = foodCollected; // Guardar la cantidad de comida recolectada
    }

    void ReturnHome()
    {
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
        SceneManager.LoadScene(0);
    }
}