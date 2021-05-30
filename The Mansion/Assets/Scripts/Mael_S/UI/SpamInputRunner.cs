using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class SpamInputRunner : MonoBehaviour
    {
        public int spam;
        [SerializeField] int spamNeeded = 15;

        [SerializeField] float timeLimit;
        float timeLimitDefault;

        public bool spamDone;
        bool timeIsRunning;

        BigBoyController bbController;
        RunnerController runnerController;
        TutoManager tuto;
        AudioManagerVEVO audioManager;
        SpamInput spamBigboy;
        PlayerController playerScript;
        MenuManager menu;

        public bool isTuto;

        public GameObject menuDefaite;

        [SerializeField] float speed;
        public Transform[] spots;
        int randomSpot;

        private void Start()
        {
            randomSpot = Random.Range(0, spots.Length);

            spam = 0;

            timeIsRunning = true;

            bbController = FindObjectOfType<BigBoyController>();
            runnerController = FindObjectOfType<RunnerController>();
            tuto = FindObjectOfType<TutoManager>();
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            spamBigboy = FindObjectOfType<SpamInput>();
            playerScript = FindObjectOfType<PlayerController>();
            menu = FindObjectOfType<MenuManager>();


        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, spots[randomSpot].position, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, spots[randomSpot].position) < 0.2f)
            {
                randomSpot = Random.Range(0, spots.Length);
            }

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
                    playerScript.playerLives -= 1;
                    gameObject.SetActive(false);                   
                }
                else if (runnerController.isGrabbing)
                {
                    runnerController.Stunned();
                    spam = 0;

                    spamDone = false;
                    playerScript.playerLives -= 1;
                    gameObject.SetActive(false);                 
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

            if (playerScript.playerLives == 0)
            {
                GameOver();
            }

        }


        public void AddSpam()
        {
            spam += 1;

            if (!menu.cannotPlaySFX)
            {
                audioManager.PlayAudio(AudioType.Spam_Hit_SFX);
            }
            
        }

        public void GameOver()
        {
            Debug.Log("U DEAD DEAD DEAD JUST DIE ALREADY");
            menuDefaite.SetActive(true);
        }
    } 
}
