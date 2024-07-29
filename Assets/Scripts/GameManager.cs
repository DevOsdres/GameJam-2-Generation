using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject mainMenuCanvas;
    public GameObject pauseCanvas;

    private bool isGameStarted = false;
    private bool isPaused = false;

    void Awake()
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

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
            pauseCanvas.SetActive(false);
            Time.timeScale = 0f;
            Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogError("MainMenuCanvas no encontrado");
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        mainMenuCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void CheckPause()
    {
        if (!isGameStarted) return;

        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
        //Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void Update()
    {
        if (isGameStarted && Input.GetKeyDown(KeyCode.P))
        {
            CheckPause();
        }
    }

    public void RestartGame()
    {
        isGameStarted = false;
        isPaused = false;
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Recarga los canvas en cada escena si no se encuentran
        if (mainMenuCanvas == null || pauseCanvas == null)
        {
            mainMenuCanvas = GameObject.Find("MainMenuCanvas");
            pauseCanvas = GameObject.Find("PauseCanvas");
        }

        if (scene.buildIndex == 0)
        {
            ShowMainMenu();
            // Descomentar si deseas quitar el listener después de usarlo
            // SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}