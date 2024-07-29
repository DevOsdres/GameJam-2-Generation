using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   



    //En caso de requerir un boton Exit
    /*public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }*/
}