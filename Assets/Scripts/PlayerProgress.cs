using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; private set; }
    public int currentLevel = 0; // Nivel actual del jugador
    public int collectedFoodCount = 0; // Cantidad de comida recolectada
    public bool foodDelivered = false; // Si la comida ha sido entregada
    public List<GameObject> foodPrefabs; // Lista de prefabs de comida
    public bool hasCompletedLevel = false; // Si el nivel ha sido completado
    public bool firstTimeLeavingHome = true; // Indicador si es la primera vez que se sale del Home

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LevelCompleted()
    {
        hasCompletedLevel = true;
        Debug.Log("Level completed! Current level: " + currentLevel);
    }

    public bool HasDeliveredFood()
    {
        return foodDelivered;
    }

    public void DeliverFood()
    {
        foodDelivered = true;
        collectedFoodCount = 0; // Reiniciar la cantidad de comida recolectada al entregar
    }

    public void IncrementLevel()
    {
        currentLevel++;
        foodDelivered = false; // Resetear al avanzar de nivel
        hasCompletedLevel = false; // Resetear al avanzar de nivel
        Debug.Log("Incremented to Level: " + currentLevel);
    }

    public void ResetProgress()
    {
        currentLevel = 0;
        collectedFoodCount = 0;
        foodDelivered = false;
        hasCompletedLevel = false;
        firstTimeLeavingHome = true; // Restablecer a verdadero
        Debug.Log("Player progress has been reset.");
    }
}