using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMansion
{
    public class PauseMenu : MonoBehaviour
    {
        GameObject player;
        AudioManagerVEVO audioManager;
        MenuManager menu;

        public GameObject pauseMenuUI;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            menu = FindObjectOfType<MenuManager>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


        public void Resume()
        {
            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Click_Button_SFX);
            }
            
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;


        }

        public void Pause()
        {
            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Click_Button_SFX);
            }
            
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;


        }

        public void LoadMenu()
        {
            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Click_Button_SFX);
            }

            if (!menu.cannotPlayMusic)
            {
                audioManager.PlayAudio(AudioType.Main_Music_ST, true, 0.7f);
            }
            
            
            SceneManager.LoadScene("Menu Principal");

            if (!menu.cannotPlayMusic)
            {
                audioManager.PlayAudio(AudioType.Main_Music_ST, true, 0.7f);
            }
            
        }

        public void QuitGame()
        {

            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Click_Button_SFX);
            }
            
            Application.Quit();
        }
    }
}
