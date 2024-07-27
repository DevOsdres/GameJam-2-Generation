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
    bool isPlayerFrontDoor;
    void Start()
    {
        confirmationCanvas.gameObject.SetActive(false); // Esconde el Canvas al inicio
        acceptButton.onClick.AddListener(OnAccept);
        cancelButton.onClick.AddListener(OnCancel);
    }

    private void Update() {


        if(Input.GetKeyDown(KeyCode.F) && isPlayerFrontDoor)
        {
            Time.timeScale = 0f; // Pausar el juego
            ShowConfirmation();
        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerFrontDoor = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerFrontDoor = false;
        }
    }

    void ShowConfirmation()
    {
        Cursor.visible = true;
        int targetLevel = PlayerProgress.Instance.currentLevel; 
        confirmationCanvas.gameObject.SetActive(true);
        confirmationText.text = $"Enter to Level {targetLevel}. You are about to leave your home, you will leave your children without knowing if you will return, are you sure about this?";
    }

    void OnAccept()
    {
        Cursor.visible = false;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego antes de cambiar de escena
        int targetLevel = PlayerProgress.Instance.currentLevel+1; 
         // Determinar automáticamente el nivel objetivo
        if (PlayerProgress.Instance.currentLevel == targetLevel - 1)
        {
            SceneManager.LoadScene(targetLevel);
        }
        else
        {
            Debug.Log("Cannot load the level. Current level progress does not match the target level.");
        }
    }

    void OnCancel()
    {
        Cursor.visible = false;
        confirmationCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
}