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
        [SerializeField] GameObject story2Button;
        [SerializeField] GameObject story2;
        [SerializeField] GameObject story3Button;
        [SerializeField] GameObject story3;
        [SerializeField] GameObject story4Button;
        [SerializeField] GameObject story4;

        public bool story1Get;
        public Sprite story1Sprite;
        public bool story2Get;
        public Sprite story2Sprite;
        public bool story3Get;
        public Sprite story3Sprite;
        public bool story4Get;
        public Sprite story4Sprite;

        [SerializeField] bool isMainMenu;

        AudioManagerVEVO audioController;

        private void Awake()
        {
            audioController = FindObjectOfType<AudioManagerVEVO>();
        }

        private void Start()
        {
            story1Get = (PlayerPrefs.GetInt("Story1") != 0);
            story2Get = (PlayerPrefs.GetInt("Story2") != 0);
            story3Get = (PlayerPrefs.GetInt("Story3") != 0);
            story4Get = (PlayerPrefs.GetInt("Story4") != 0);
            
            if (isMainMenu)
            {
                audioController.PlayAudio(AudioType.Main_Music_ST, true, 0.7f);
            }
        }

        public void ChangeLevel(string levelName)
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (isMainMenu)
            {
                audioController.StopAudio(AudioType.Main_Music_ST, true, 1f);
            }
            SceneManager.LoadScene(levelName);
            Time.timeScale = 1;
        }

        public void LoadLevel1()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            SceneManager.LoadScene("Niveau_1_TUTO");
        }

        public void ReloadLevel()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            Application.Quit();
        }

        public void SetLevelSelection()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
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
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            //fonds.SetActive(false);
            //levels.SetActive(false);
            selectionLevel.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void SetStorySelection()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            selectionStory.SetActive(true);

            storyButton.SetActive(false);
            levelsButton.SetActive(false);
            optionsButton.SetActive(false);

            if (story1Get)
            {
                story1Button.GetComponent<Image>().sprite = story1Sprite;
            }

            if (story2Get)
            {
                story2Button.GetComponent<Image>().sprite = story2Sprite;
            }

            if (story3Get)
            {
                story3Button.GetComponent<Image>().sprite = story3Sprite;
            }

            if (story4Get)
            {
                story4Button.GetComponent<Image>().sprite = story4Sprite;
            }
        }

        public void RemoveStorySelection()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            selectionStory.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void StoriesClose()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            story1.SetActive(false);
            story2.SetActive(false);
            story3.SetActive(false);
            story4.SetActive(false);

            storyList.SetActive(false);
            selectionStory.SetActive(true);
        }

        public void Story1Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story1Get)
            {
                story1.SetActive(true);
                selectionStory.SetActive(false);
                storyList.SetActive(true);
            }
        }

        public void Story2Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story2Get)
            {
                story2.SetActive(true);
                selectionStory.SetActive(false);
                storyList.SetActive(true);
            }
        }

        public void Story3Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story3Get)
            {
                story3.SetActive(true);
                selectionStory.SetActive(false);
                storyList.SetActive(true);
            }
        }

        public void Story4Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story4Get)
            {
                story4.SetActive(true);
                selectionStory.SetActive(false);
                storyList.SetActive(true);
            }
        }
    }
}

