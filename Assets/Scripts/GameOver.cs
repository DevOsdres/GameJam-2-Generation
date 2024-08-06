using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        //Time.timeScale = 1f;
        PlayerProgress.Instance.ResetProgress(); // Reiniciar el progreso del jugador
        SceneManager.LoadScene(0); //Cargo la escena MainMenu
    }
}
