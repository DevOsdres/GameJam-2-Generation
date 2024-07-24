using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject PauseMenu;
    public GameObject CreditsMenu;
    public Button acceptButton; 
    private PlayerProgress playerProgress;
    public int targetLevel;

    void Start()
    {
        playerProgress = FindObjectOfType<PlayerProgress>(); // Encuentra el script PlayerProgress en la escena

        acceptButton.onClick.AddListener(OnAccept);
    }
    public void OpenPanel(GameObject panel)
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        panel.SetActive(true);
    }

    public void OnAccept()
    {
        MainMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        if (playerProgress.currentLevel == targetLevel - 1)
        {
            Debug.Log($"Loading scene: Level{targetLevel}");
            SceneManager.LoadScene($"Level{targetLevel}");
        }
        else
        {
            Debug.Log("Cannot load the level. Current level progress does not match the target level.");
        }
    }

    // Update is called once per frame
   
}
