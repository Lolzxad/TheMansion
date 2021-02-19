using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class SpamInput : MonoBehaviour
    {
        [SerializeField] int spamRight;
        [SerializeField] int spamLeft;

        [SerializeField] float timeLimit;

        bool spamLeftDone;
        bool spamRightDone;
        bool timeIsRunning;

        BigBoyController bbController;

        private void Start()
        {
            spamLeft = 0;
            spamRight = 0;

            timeIsRunning = true;

            bbController = FindObjectOfType<BigBoyController>();
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


            if(spamLeft == 2)
            {
                spamLeftDone = true;
                Debug.Log(spamLeftDone);
            }

            if(spamRight == 2)
            {
                spamRightDone = true;
                Debug.Log(spamRightDone);
            }


            if(spamRightDone && spamLeftDone)
            {
                Debug.Log("YOU FREE TO GO");

                bbController.Stunned();
                spamLeft = 0;
                spamRight = 0;
                gameObject.SetActive(false);
            }

        }

        public void AddSpamLeft()
        {
            spamLeft += 1;
        }

        public void AddSpamRight()
        {
            spamRight += 1;
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
        }
    } 
}
