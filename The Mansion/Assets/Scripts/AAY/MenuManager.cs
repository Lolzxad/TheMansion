﻿using System.Collections;
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

        [Space]
        [Header("Game Objects")]
        [SerializeField] GameObject selectionLevel;
        [SerializeField] GameObject selectionStory;
        [SerializeField] GameObject storyList1;
        [SerializeField] GameObject storyList2;

        [SerializeField] GameObject storyButton;
        [SerializeField] GameObject levelsButton;
        [SerializeField] GameObject optionsButton;
        [SerializeField] GameObject unlockButton;
        


        [Space]
        [Header ("Lore")]
        [SerializeField] GameObject story1Button;
        [SerializeField] GameObject story1;
        [SerializeField] GameObject story2Button;
        [SerializeField] GameObject story2;
        [SerializeField] GameObject story3Button;
        [SerializeField] GameObject story3;
        [SerializeField] GameObject story4Button;
        [SerializeField] GameObject story4;
        [SerializeField] GameObject story5Button;
        [SerializeField] GameObject story6Button;
        [SerializeField] GameObject story7Button;
        [SerializeField] GameObject story8Button;
        [SerializeField] GameObject story5;
        [SerializeField] GameObject story6;
        [SerializeField] GameObject story7;
        [SerializeField] GameObject story8;


        [SerializeField] GameObject credits;
        [SerializeField] GameObject options;

        [Space]
        [Header("Level Bools")]
        public bool level2Unlocked;
        public bool level3Unlocked;
        public bool level4Unlocked;
        public bool level5Unlocked;

        [Space]
        [Header("Level Real")]
        public GameObject realLevel2;
        public GameObject realLevel3;
        public GameObject realLevel4;
        public GameObject realLevel5;

        [Space]
        [Header("Level Fakes")]
        public GameObject fakeLevel2;
        public GameObject fakeLevel3;
        public GameObject fakeLevel4;
        public GameObject fakeLevel5;

        [Space]
        [Header("Story Bool and Sprites")]
        public bool story1Get;
        public Sprite story1Sprite;
        public bool story2Get;
        public Sprite story2Sprite;
        public bool story3Get;
        public Sprite story3Sprite;
        public bool story4Get;
        public Sprite story4Sprite;
        public bool story5Get;
        public Sprite story5Sprite;
        public bool story6Get;
        public Sprite story6Sprite;
        public bool story7Get;
        public Sprite story7Sprite;
        public bool story8Get;
        public Sprite story8Sprite;
        [SerializeField] Sprite unlockSprite;


        [SerializeField] GameObject lettersMenu;
        [SerializeField] GameObject notebookMenu;


        [SerializeField] GameObject fullLevels;
        [SerializeField] GameObject lockedLevels;
        [SerializeField] GameObject musicButton;
        [SerializeField] GameObject soundButton;
        [SerializeField] GameObject noMusicButton;
        [SerializeField] GameObject noSoundButton;



        public bool cannotPlayMusic;
        public bool cannotPlaySFX;

        [SerializeField] bool isMainMenu;
        

        AudioManagerVEVO audioController;

        private void Awake()
        {
            audioController = FindObjectOfType<AudioManagerVEVO>();
            story1Get = (PlayerPrefs.GetInt("Story1") != 0);
            story2Get = (PlayerPrefs.GetInt("Story2") != 0);
            story3Get = (PlayerPrefs.GetInt("Story3") != 0);
            story4Get = (PlayerPrefs.GetInt("Story4") != 0);
            story5Get = (PlayerPrefs.GetInt("Story5") != 0);
            story6Get = (PlayerPrefs.GetInt("Story6") != 0);
            story7Get = (PlayerPrefs.GetInt("Story7") != 0);
            story8Get = (PlayerPrefs.GetInt("Story8") != 0);

            level2Unlocked = (PlayerPrefs.GetInt("Level2Unlocked") != 0);
            level3Unlocked = (PlayerPrefs.GetInt("Level3Unlocked") != 0);
            level4Unlocked = (PlayerPrefs.GetInt("Level4Unlocked") != 0);
            level5Unlocked = (PlayerPrefs.GetInt("Level5Unlocked") != 0);
        }

        private void Start()
        {

            cannotPlayMusic = (PlayerPrefs.GetInt("CannotPlayMusic") != 0);
            cannotPlaySFX = (PlayerPrefs.GetInt("CannotPlaySFX") != 0);

            Time.timeScale = 1;
        }

        private void Update()
        {
            //PlayerPrefs.SetInt("CannotPlaySFX", (cannotPlaySFX ? 1 : 0));            
        }

        public void NoMoreMusic()
        {         
            audioController.StopAudio(AudioType.Main_Music_ST);
            musicButton.SetActive(true);
            noMusicButton.SetActive(false);

            if (!cannotPlayMusic)
            {
                cannotPlayMusic = true;
                PlayerPrefs.SetInt("CannotPlayMusic", (cannotPlayMusic ? 1 : 0));
            }
            else
            {
                cannotPlayMusic = false;
                PlayerPrefs.SetInt("CannotPlayMusic", (cannotPlayMusic ? 0 : 1));
            }
        }

        public void PutMusic()
        {
            audioController.PlayAudio(AudioType.Main_Music_ST);
            musicButton.SetActive(false);
            noMusicButton.SetActive(true);

            if (!cannotPlayMusic)
            {
                cannotPlayMusic = true;
                PlayerPrefs.SetInt("CannotPlayMusic", (cannotPlayMusic ? 1 : 0));
            }
            else
            {
                cannotPlayMusic = false;
                PlayerPrefs.SetInt("CannotPlayMusic", (cannotPlayMusic ? 0 : 1));
            }
        }


        public void NoMoreSFX()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            soundButton.SetActive(true);
            noSoundButton.SetActive(false);

            if (!cannotPlayMusic)
            {
                cannotPlaySFX = true;
                PlayerPrefs.SetInt("CannotPlaySFX", (cannotPlaySFX ? 1 : 0));
            }
            else
            {
                cannotPlaySFX = false;
                PlayerPrefs.SetInt("CannotPlaySFX", (cannotPlaySFX ? 0 : 1));
            }
        }

        public void PutSFX()
        {
            
            soundButton.SetActive(false);
            noSoundButton.SetActive(true);

            if (!cannotPlaySFX)
            {
                cannotPlaySFX = true;
                PlayerPrefs.SetInt("CannotPlaySFX", (cannotPlaySFX ? 1 : 0));
            }
            else
            {
                cannotPlaySFX = false;
                PlayerPrefs.SetInt("CannotPlaySFX", (cannotPlaySFX ? 0 : 1));
            }
        }


        public void ChangeLevel(string levelName)
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            
            if (isMainMenu)
            {
                //audioController.StopAudio(AudioType.Main_Music_ST, true, 1f);
            }
            SceneManager.LoadScene(levelName);
            
        }

        

        public void LoadLevel1()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            SceneManager.LoadScene("Niveau_1_TUTO");
        }

        public void ReloadLevel()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            Application.Quit();
        }

        #region Level Menu

        public void SetLevelSelection()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            Debug.Log("Menu selection");
            // fonds.SetActive(true);
            //levels.SetActive(true);
            selectionLevel.SetActive(true);

            storyButton.SetActive(false);
            levelsButton.SetActive(false);
            optionsButton.SetActive(false);

            if (level2Unlocked)
            {
                fakeLevel2.SetActive(false);
                realLevel2.SetActive(true);
            }

            if (level3Unlocked)
            {
                fakeLevel3.SetActive(false);
                realLevel3.SetActive(true);
            }

            if (level4Unlocked)
            {
                fakeLevel4.SetActive(false);
                realLevel4.SetActive(true);
            }

            if (level5Unlocked)
            {
                fakeLevel5.SetActive(false);
                realLevel5.SetActive(true);
            }
        }

        public void RemoveSelectionLevel()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            //fonds.SetActive(false);
            //levels.SetActive(false);
            selectionLevel.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void SetStorySelection()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
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

            if (story5Get)
            {
                story5Button.GetComponent<Image>().sprite = story5Sprite;
            }

            if (story6Get)
            {
                story6Button.GetComponent<Image>().sprite = story6Sprite;
            }

            if (story7Get)
            {
                story7Button.GetComponent<Image>().sprite = story7Sprite;
            }

            if (story8Get)
            {
                story8Button.GetComponent<Image>().sprite = story8Sprite;
            }

            
        }

        public void RemoveStorySelection()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            selectionStory.SetActive(false);

            storyButton.SetActive(true);
            levelsButton.SetActive(true);
            optionsButton.SetActive(true);
        }

        public void StoriesClose()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            story1.SetActive(false);
            story2.SetActive(false);
            story3.SetActive(false);
            story4.SetActive(false);

            storyList1.SetActive(false);
            selectionStory.SetActive(true);
        }


        #region story opened functions
        public void Story1Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story1Get)
            {
                story1.SetActive(true);
                //selectionStory.SetActive(false);
                storyList1.SetActive(true);
            }
        }

        public void Story2Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story2Get)
            {
                story2.SetActive(true);
                //selectionStory.SetActive(false);
                storyList1.SetActive(true);
            }
        }

        public void Story3Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story3Get)
            {
                story3.SetActive(true);
                //selectionStory.SetActive(false);
                storyList1.SetActive(true);
            }
        }

        public void Story4Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story4Get)
            {
                story4.SetActive(true);
                //selectionStory.SetActive(false);
                storyList1.SetActive(true);
            }
        }

        public void Story5Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story5Get)
            {
                story5.SetActive(true);
                //selectionStory.SetActive(false);
                storyList2.SetActive(true);
            }
        }

        public void Story6Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story6Get)
            {
                story6.SetActive(true);
                //selectionStory.SetActive(false);
                storyList2.SetActive(true);
            }
        }

        public void Story7Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story7Get)
            {
                story7.SetActive(true);
                //selectionStory.SetActive(false);
                storyList2.SetActive(true);
            }
        }

        public void Story8Opened()
        {
            audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            if (story8Get)
            {
                story8.SetActive(true);
                //selectionStory.SetActive(false);
                storyList2.SetActive(true);
            }
        }
        #endregion


        public void GoToNotebooks()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            lettersMenu.SetActive(false);
            notebookMenu.SetActive(true);
        }

        public void GoToLetters()
        {
            if (!cannotPlayMusic)
            {
                audioController.PlayAudio(AudioType.Click_Button_SFX, false);
            }
            notebookMenu.SetActive(false);
            lettersMenu.SetActive(true);           
        }

        #endregion

        public void Options()
        {
            options.SetActive(true);
        }

        public void RemoveOptions()
        {
            options.SetActive(false);
        }

        public void Credits()
        {
            credits.SetActive(true);
        }

        public void RemoveCredits()
        {
            credits.SetActive(false);
        }

        public void UnlockAllLevels()
        {
            level2Unlocked = true;
            level3Unlocked = true;
            level4Unlocked = true;
            level5Unlocked = true;
            PlayerPrefs.SetInt("Level2Unlocked", (level2Unlocked ? 1 : 0));
            PlayerPrefs.SetInt("Level3Unlocked", (level3Unlocked ? 1 : 0));
            PlayerPrefs.SetInt("Level4Unlocked", (level4Unlocked ? 1 : 0));
            PlayerPrefs.SetInt("Level5Unlocked", (level5Unlocked ? 1 : 0));
            unlockButton.GetComponent<Image>().sprite = unlockSprite;
        }
    }
}

