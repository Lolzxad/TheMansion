﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMansion
{
    public class PauseMenu : MonoBehaviour
    {
        GameObject player;
        AudioManagerVEVO audioManager;

        public GameObject pauseMenuUI;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


        public void Resume()
        {
            audioManager.PlayAudio(AudioType.Click_Button_SFX);
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;


        }

        public void Pause()
        {
            audioManager.PlayAudio(AudioType.Click_Button_SFX);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;


        }

        public void LoadMenu()
        {
            audioManager.PlayAudio(AudioType.Click_Button_SFX);
            audioManager.PlayAudio(AudioType.Main_Music_ST, true, 0.7f);
            SceneManager.LoadScene("Menu Principal");
            audioManager.PlayAudio(AudioType.Main_Music_ST, true, 0.7f);
        }

        public void QuitGame()
        {
            audioManager.PlayAudio(AudioType.Click_Button_SFX);
            Application.Quit();
        }
    }
}
