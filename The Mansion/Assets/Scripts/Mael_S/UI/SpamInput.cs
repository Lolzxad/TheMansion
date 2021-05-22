﻿using System.Collections;
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
        bool timeIsRunning;

        BigBoyController bbController;
        RunnerController runnerController;
        TutoManager tuto;

        public bool isTuto;

        public GameObject menuDefaite;

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
                    if (!tuto.isTuto)
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


            if(spamL == spamNeeded && spamR == spamNeeded)
            {
                spamDone = true;
                Debug.Log(spamDone);
            }


            if(spamDone)
            {
                Debug.Log("YOU FREE TO GO");

                if (bbController.isGrabbing)
                {
                    Debug.Log("IsStunned");
                    bbController.Stunned();

                    spamL = 0;
                    spamR = 0;

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
        }

        public void AddSpamR()
        {
            spamR += 1;
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
            menuDefaite.SetActive(true);
        }
    } 
}