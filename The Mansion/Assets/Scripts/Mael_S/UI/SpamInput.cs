using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class SpamInput : MonoBehaviour
    {
        public int spamL;
        public int spamR;

        public int playerLives = 4;
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

        public bool isTuto;

        public GameObject menuDefaite;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
        }

        private void Start()
        {

            spamL = 0;
            spamR = 0;

            timeIsRunning = true;

            bbController = FindObjectOfType<BigBoyController>();
            runnerController = FindObjectOfType<RunnerController>();
            tuto = FindObjectOfType<TutoManager>();
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
                    audioManager.PlayAudio(AudioType.Bb_Stun_SFX);
                    bbController.Stunned();

                    spamL = 0;
                    spamR = 0;

                    spamDone_L = false;
                    spamDone_R = false;
                    spamDone = false;
                    gameObject.SetActive(false);
                    playerLives--;
                }

               

              


                if (tuto)
                {
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

            if (playerLives == 0)
            {
                GameOver();
            }

        }


        public void AddSpamL()
        {
            spamL += 1;
            audioManager.PlayAudio(AudioType.Spam_Hit_SFX);
        }

        public void AddSpamR()
        {
            spamR += 1;
            audioManager.PlayAudio(AudioType.Spam_Hit_SFX);
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
            menuDefaite.SetActive(true);
        }
    } 
}
