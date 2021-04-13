using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject player;

    public GameObject pauseMenuUI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

       
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;

        
    }

    public void LoadMenu()
    {
        //SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}