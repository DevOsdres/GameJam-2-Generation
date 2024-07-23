using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public int currentLevel = 0; // Nivel actual del jugador

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
    }

    public void LevelCompleted()
    {
        currentLevel++;
        Debug.Log("Level completed! Current level: " + currentLevel);
    }
}
