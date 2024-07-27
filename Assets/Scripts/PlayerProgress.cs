using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; private set; }
    public int currentLevel = 0; // Nivel actual del jugador
    public int collectedFoodCount = 0; // Comida recolectada por el jugador
    private bool foodDelivered = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelCompleted()
    {
        currentLevel++;
        Debug.Log("Level completed! Current level: " + currentLevel);
    }

    public bool HasDeliveredFood()
    {
        return foodDelivered;
    }

    public void SetFoodDelivered(bool delivered)
    {
        foodDelivered = delivered;
    }

    public void ResetCollectedFood()
    {
        collectedFoodCount = 0;
        foodDelivered = false;
    }
}