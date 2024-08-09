using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FoodCollector : MonoBehaviour
{
    public GameObject levelCompleteCanvas; // Canvas que muestra al completar el nivel
    public Button returnHomeButton; // Botón para regresar al hogar
    private int foodRequired = 10; // Cantidad de comida requerida
    [SerializeField] private AudioClip foodSound;
    private bool foodTake = false;

    void Start()
    {
        PlayerProgress.Instance.collectedFoodCount = 0; // Reiniciar el contador de comida recolectada al inicio de la escena

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

    private void Update() {
        if (foodTake == true){
            foodTake = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {  
        //Debug.Log("Trigger entered with: " + other.gameObject.name); // Depuración: Nombre del objeto con el que se colisiona

        if (other.CompareTag("Food"))
        { 
            AudioManager2.Instance.PlaySFX(foodSound);
            foodTake = true;
            PlayerProgress.Instance.collectedFoodCount += 1; // Incrementar la cantidad de comida recolectada
            //Debug.Log("Food collected: " + PlayerProgress.Instance.collectedFoodCount);
            Destroy(other.gameObject);

            if (PlayerProgress.Instance.collectedFoodCount >= foodRequired)
            {
                Time.timeScale = 0f; // Pausar el juego
                LevelCompleted();
            }
        }
        else
        {
            //Debug.Log("Collided object is not tagged as Food");
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

        PlayerProgress playerProgress = PlayerProgress.Instance;
        if (playerProgress != null)
        {
            playerProgress.LevelCompleted();
        }
        else
        {
            Debug.LogError("PlayerProgress script not found in the scene.");
        }
    }

    public void ReturnHome()
    {
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
        SceneManager.LoadScene(1);
    }
}