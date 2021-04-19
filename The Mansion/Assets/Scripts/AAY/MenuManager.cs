using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMansion
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] GameObject fonds;
        [SerializeField] GameObject levels;

        [SerializeField] GameObject storyButton;
        [SerializeField] GameObject levelsButton;
        [SerializeField] GameObject optionsButton;

        public void ChangeLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadLevel1()
        {
            SceneManager.LoadScene("Niveau_1_TUTO");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SetLevelSelection()
        {
            fonds.SetActive(true);
            levels.SetActive(true);

            storyButton.SetActive(false);
            levelsButton.SetActive(false);
            optionsButton.SetActive(false);
        }

        public void RemoveSelectionLevel()
        {
            fonds.SetActive(false);
            levels.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }
    }
}

