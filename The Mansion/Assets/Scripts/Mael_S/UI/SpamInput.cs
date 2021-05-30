using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheMansion
{


    public class SpamInput : MonoBehaviour
    {
        public int spamL;
        public int spamR;
        [SerializeField] int spamNeeded = 15;

        [SerializeField] float timeLimit;
        float timeLimitDefault;

        public bool spamDone;
        [SerializeField] bool spamDone_L;
        [SerializeField] bool spamDone_R;
        bool timeIsRunning;

        BigBoyController bbController;
        RunnerController runnerController;
        TutoManager tuto;
        AudioManagerVEVO audioManager;
        SpamInputRunner spamRunner;
        PlayerController playerScript;
        MenuManager menu;
        SecondBigBoy bb_2;


        [SerializeField] Sprite heart_1;
        [SerializeField] Sprite heart_2;
        [SerializeField] Sprite heart_3;
        [SerializeField] GameObject heart;

        [SerializeField] bool isSecondBb;
        public bool isTuto;

        public GameObject menuDefaite;

        [SerializeField] GameObject bigboy;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            menu = FindObjectOfType<MenuManager>();
            bb_2 = FindObjectOfType<SecondBigBoy>();
        }

        private void Start()
        {

            spamL = 0;
            spamR = 0;

            timeIsRunning = true;

            bbController = FindObjectOfType<BigBoyController>();
            runnerController = FindObjectOfType<RunnerController>();
            tuto = FindObjectOfType<TutoManager>();
            spamRunner = FindObjectOfType<SpamInputRunner>();
            playerScript = FindObjectOfType<PlayerController>();
            menu = FindObjectOfType<MenuManager>();


            heart.GetComponent<Image>();
        }

        private void Update()
        {
            if (timeIsRunning)
            {
                if (timeLimit > 0)
                {
                    timeLimit -= Time.deltaTime;
                }
                else
                {
                    if (!isTuto)
                    {
                        Debug.Log("TIME OUT");
                        GameOver();

                        timeLimit = 0;
                        timeIsRunning = false;
                    }
                    else
                    {
                        spamDone = true;
                    }
                    
                }
            }


            if(spamL >= spamNeeded)
            {
                spamDone_L = true;
                Debug.Log("Done spam_L");
            }

            if (spamR >= spamNeeded)
            {
                spamDone_R = true;
                Debug.Log("Done spam_R");
            }
            
            if(spamDone_L && spamDone_R)
            {
                spamDone = true;
            }


            if (spamDone)
            {
                Debug.Log("YOU FREE TO GO");

                if (bbController.isGrabbing)
                {
                    Debug.Log("IsStunned");

                    if (!menu.cannotPlaySFX)
                    {
                        audioManager.PlayAudio(AudioType.Bb_Stun_SFX);
                    }
                    
                    bbController.Stunned();

                    spamL = 0;
                    spamR = 0;

                    spamDone_L = false;
                    spamDone_R = false;
                    spamDone = false;
                    gameObject.SetActive(false);
                }


                if (bb_2)
                {
                    if (bb_2.isGrabbing)
                    {
                        Debug.Log("IsStunned");

                        if (!menu.cannotPlaySFX)
                        {
                            audioManager.PlayAudio(AudioType.Bb_Stun_SFX);
                        }

                        bb_2.Stunned();

                        spamL = 0;
                        spamR = 0;

                        spamDone_L = false;
                        spamDone_R = false;
                        spamDone = false;
                        gameObject.SetActive(false);
                    }
                }
              


                if (tuto)
                {
                    bigboy.SetActive(false);
                    tuto.stunTexte.SetActive(true);
                    tuto.canWinTrigger.SetActive(true);
                    bbController.GetComponent<BigBoyController>().enabled = false;
                }
                

                /*if (runnerController.isGrabbing)
                {
                    runnerController.isTired = true;
                    spamLeft = 0;
                    spamRight = 0;
                    spamLeftDone = false;
                    spamRightDone = false;
                    gameObject.SetActive(false);
                    playerLives--;
                }*/


            }           
        }


        public void AddSpamL()
        {
            spamL += 1;

            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Spam_Hit_SFX);
            }
            
        }

        public void AddSpamR()
        {
            spamR += 1;
            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Spam_Hit_SFX);
            }
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
            menuDefaite.SetActive(true);
            Time.timeScale = 0;
        }
    } 
}
