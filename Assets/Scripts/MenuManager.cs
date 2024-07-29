using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject mainMenuCanvas;
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    public GameObject pauseMenuCanvas;
    public GameObject pauseMenuPanel;
    public GameObject pauseOptionsPanel;
    public GameObject pauseControlsPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1f)
            {
                ShowPauseMenu();
            }
            else if (Time.timeScale == 0f && pauseMenuCanvas.activeSelf)
            {
                HidePauseMenu();
            }
        }
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    public void HideMainMenu()
    {
        mainMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowPauseMenu()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HidePauseMenu()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnPlayButton()
    {
        HideMainMenu();
    }

    public void OnContinueButton()
    {
        HidePauseMenu();
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(0);
        ShowMainMenu();
    }

    public void OnOptionsButton()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnControlsButton()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void OnCreditsButton()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnPauseOptionsButton()
    {
        pauseMenuPanel.SetActive(false);
        pauseOptionsPanel.SetActive(true);
    }

    public void OnPauseControlsButton()
    {
        pauseMenuPanel.SetActive(false);
        pauseControlsPanel.SetActive(true);
    }

    public void OnReturnButton()
    {
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else if (controlsPanel.activeSelf)
        {
            controlsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else if (creditsPanel.activeSelf)
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else if (pauseOptionsPanel.activeSelf)
        {
            pauseOptionsPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
        }
        else if (pauseControlsPanel.activeSelf)
        {
            pauseControlsPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
        }
    }
}