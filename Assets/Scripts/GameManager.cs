using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Referencia al Canvas del menú principal
    public GameObject pauseCanvas; // Referencia al Canvas del menú de pausa
    //public PauseMenu pauseMenu; // Referencia al script PauseMenu asignado a PauseCanvas

    private bool isGameRunning = false; // Variable para verificar si el juego está en curso
    private bool isPaused = false; // Variable para verificar si el juego está pausado

    void Start()
    {
        ShowMainMenu(); // Muestra el menú principal al iniciar el juego
    }

    void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true); // Activa el Canvas del menú principal
        Time.timeScale = 0f; // Pausa el juego
        Cursor.visible = true; // Hace visible el cursor
        //Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false); // Desactiva el Canvas del menú principal
        Time.timeScale = 1f; // Reanuda el juego
        Cursor.visible = false; // Oculta el cursor
        //Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
        isGameRunning = true; // Marca el juego como en curso
    }

    public void CheckPause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            pauseCanvas.SetActive(true); // Activa el Canvas del menú de pausa
            Time.timeScale = 0f; // Pausa el juego
            Cursor.visible = true; // Hace visible el cursor
        }
        else
        {
            pauseCanvas.SetActive(false); // Desactiva el Canvas del menú de pausa
            Time.timeScale = 1f; // Reanuda el juego
            //Cursor.visible = false; // Hace visible el cursor
        }
    }

    void Update()
    {
        if (!mainMenuCanvas.activeInHierarchy && Input.GetKeyDown(KeyCode.P)) // Verifica si el juego está en curso y se presiona la tecla Escape
        {
            CheckPause();
        }
    }

    public void RestartGame()
    {
        isGameRunning = false; // Marca el juego como no en curso
        Time.timeScale = 1f; // Asegura que el tiempo de juego esté en marcha
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false); // Desactiva el Canvas del menú de pausa
        Time.timeScale = 1f; // Reanuda el tiempo de juego
        Cursor.visible = false; // Oculta el cursor
        //Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
    }
    public int ActualSceneNumber()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}