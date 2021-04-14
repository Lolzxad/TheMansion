using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class SpamInput : MonoBehaviour
    {
        [SerializeField] int spam;

        [SerializeField] int playerLives = 4;
        [SerializeField] int spamNeeded = 15;

        [SerializeField] float timeLimit;


        bool spamDone;
        bool timeIsRunning;

        BigBoyController bbController;
        RunnerController runnerController;

        private void Start()
        {

            spam = 0;

            timeIsRunning = true;

            bbController = FindObjectOfType<BigBoyController>();
            runnerController = FindObjectOfType<RunnerController>();
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
                    Debug.Log("TIME OUT");
                    GameOver();

                    timeLimit = 0;
                    timeIsRunning = false;
                }
            }


            if(spam == spamNeeded)
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

                    spam = 0;
  
                    spamDone = false;
                    gameObject.SetActive(false);
                    playerLives--;
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


        public void AddSpam()
        {
            spam += 1;
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
        }
    } 
}
