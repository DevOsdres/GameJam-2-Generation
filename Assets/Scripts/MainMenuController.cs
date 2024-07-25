using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;

    void Start()
    {
        ShowMainMenu();
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        //mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        //creditsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        //mainMenuPanel.SetActive(false);
        //optionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        ShowMainMenu();
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}