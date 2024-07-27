using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance {get; private set;}
    public int currentLevel = 0; // Nivel actual del jugador

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public void LevelCompleted()
    {
        currentLevel++;
        Debug.Log("Level completed! Current level: " + currentLevel);
    }
}