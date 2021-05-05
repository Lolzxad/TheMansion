using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TheMansion
{
    public class MenuManager : MonoBehaviour
    {
        //  [SerializeField] GameObject fonds;
        //[SerializeField] GameObject levels;

        [SerializeField] GameObject selectionLevel;
        [SerializeField] GameObject selectionStory;
        [SerializeField] GameObject storyList;

        [SerializeField] GameObject storyButton;
        [SerializeField] GameObject levelsButton;
        [SerializeField] GameObject optionsButton;

        [SerializeField] GameObject story1Button;
        [SerializeField] GameObject story1;

        public bool story1Get;
        public Sprite story1Sprite;

        [SerializeField] bool isMainMenu;
        AudioManager audioManager;

        private void Awake()
        {
            audioManager = AudioManager.instance;
        }

        private void Start()
        {
            if (isMainMenu)
            {
                audioManager.PlaySound("MainMenu_Music");
            }
            else
            {
                audioManager.StopSound("MainMenu_Music");
            }
        }

        public void ChangeLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
            Time.timeScale = 1;
        }

        public void LoadLevel1()
        {
            SceneManager.LoadScene("Niveau_1_TUTO");
        }

        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SetLevelSelection()
        {
            Debug.Log("Menu selection");
            // fonds.SetActive(true);
            //levels.SetActive(true);
            selectionLevel.SetActive(true);

            storyButton.SetActive(false);
            levelsButton.SetActive(false);
            optionsButton.SetActive(false);
        }

        public void RemoveSelectionLevel()
        {
            //fonds.SetActive(false);
            //levels.SetActive(false);
            selectionLevel.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void SetStorySelection()
        {
            selectionStory.SetActive(true);

            storyButton.SetActive(false);
            levelsButton.SetActive(false);
            optionsButton.SetActive(false);

            if (story1Get)
            {
                story1Button.GetComponent<Image>().sprite = story1Sprite;
            }
        }

        public void RemoveStorySelection()
        {
            selectionStory.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void StoriesClose()
        {
            story1.SetActive(false);

            storyList.SetActive(false);
            selectionStory.SetActive(true);
        }

        public void Story1Opened()
        {
            if (story1Get)
            {
                story1.SetActive(true);
                selectionStory.SetActive(false);
                storyList.SetActive(true);
            }
        }
    }
}

