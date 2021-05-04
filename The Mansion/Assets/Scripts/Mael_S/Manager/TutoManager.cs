using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class TutoManager : MonoBehaviour
    {

        public bool isTuto;
        PlayerController playerController;
        BigBoyController bigBoyController;
        VictoryManager victoryManager;

        [Space]
        [Header("Texte")]
        public GameObject introTexte;
        public GameObject doorIsLockedTexte;
        public GameObject timeToHideTexte;
        public GameObject heartTexte;
        public GameObject stunTexte;


        [Space]
        [Header("Triggers")]
        public GameObject doorLockedTrigger;
        public GameObject hideTrigger;
        public GameObject frontBbTrigger;
        public GameObject backBbTrigger;
        public GameObject bBCameraHandlerTrigger;
        public GameObject canWinTrigger;

        [Space]
        [Header("Core GameObjects-")]
        public GameObject player;
        public GameObject bigBoy;
        public GameObject heart;

        [Space]
        [Header("Inputs")]
        //public GameObject movementInput;
        public GameObject hideInput;
        public GameObject hideInput2;
        public GameObject heartInput;
        public GameObject newMovementInput;

        [Space]
        [Header("Des bools")]
        public bool isDoorLocked;
        public bool readyToHide;
        bool playerIsHiding;
        bool bbIsHere;

        public void Start()
        {
            isTuto = true;
            playerIsHiding = true;

            playerController = FindObjectOfType<PlayerController>();
            bigBoyController = FindObjectOfType<BigBoyController>();



            // player.GetComponent<PlayerController>().enabled = false;

              bigBoy.GetComponent<BigBoyController>().enabled = false;
              bigBoy.GetComponent<SpriteRenderer>().enabled = false;
              bigBoy.GetComponent<Collider2D>().enabled = false;
              frontBbTrigger.SetActive(false);
              backBbTrigger.SetActive(false);

              canWinTrigger.SetActive(false);

            Time.timeScale = 0;
            
        }

        public void okNowMove()
        {
            newMovementInput.SetActive(false);
            Time.timeScale = 1;
            StartCoroutine(SpawnInputMove());
        }

        public void Update()
        {
            if(playerController.isHiding && playerIsHiding)
            {
                //hideTrigger.SetActive(false);
                
                bigBoy.GetComponent<BigBoyController>().enabled = false;
                StartCoroutine(WaitForHeart());
            }

            if (bbIsHere)
            {
                if (bigBoyController.bBcanMove == false)
                {
                    Debug.Log("BB can't move AAAAAAAAAh");

                    bigBoy.GetComponent<BigBoyController>().enabled = false;
                    //stunTexte.SetActive(true);
                    player.GetComponent<PlayerController>().enabled = false;
                    StartCoroutine(WaitForMove());
                }
            }

           

            if(readyToHide == true)
            {
                StartCoroutine(WaitForMove());
            }
        }

        IEnumerator WaitForMove()
        {
            yield return new WaitForSeconds(2);
            player.GetComponent<PlayerController>().enabled = true;
        }


        IEnumerator SpawnInputMove()
        {
            //Debug.Log("Spawn input move");
            introTexte.SetActive(true);
            
            yield return new WaitForSeconds(7);
            //movementInput.SetActive(true);

            StartCoroutine(WaitToRemoveInputMove());

        }

        IEnumerator WaitToRemoveInputMove()
        {
            yield return new WaitForSeconds(2);
            //movementInput.SetActive(false);
           // player.GetComponent<PlayerController>().enabled = true;
            Debug.Log("LA vie est une pute");
            introTexte.SetActive(false);
        }

        IEnumerator WaitForInput()
        {
            
            Debug.Log("Hide input");
            

            yield return new WaitForSeconds(5);
            hideInput.SetActive(true);
            hideInput2.SetActive(true);
            Debug.Log("Show input hide");


            StartCoroutine(WaitToRemoveInputHide());
            Debug.Log("Remove input hide");
            
            
        }

        IEnumerator WaitToRemoveInputHide()
        {
            yield return new WaitForSeconds(5);
            hideInput.SetActive(false);
            hideInput2.SetActive(false);

            Debug.Log("IL PEUT BOUGER");
            player.GetComponent<PlayerController>().enabled = true;
        }

        IEnumerator WaitForHeart()
        {
            player.GetComponent<PlayerController>().enabled = false;

            Debug.Log("Heart");
            playerIsHiding = false;

            heartTexte.SetActive(true);
            playerController.heartBeat = 300;
            playerController.hidingFactor = 300;
            heart.SetActive(true);
            heart.GetComponent<HeartAnimation>().enabled = false;
            yield return new WaitForSeconds(8);

            heartInput.SetActive(true);

            StartCoroutine(WaitToRemoveInputHeart());

            heart.GetComponent<HeartAnimation>().enabled = true;  


        }

        IEnumerator WaitToRemoveInputHeart()
        {
            yield return new WaitForSeconds(5);
            heartInput.SetActive(false);
            hideTrigger.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;

            bigBoy.GetComponent<Collider2D>().enabled = true;
            bigBoy.GetComponent<BigBoyController>().enabled = true;
            bigBoy.GetComponent<SpriteRenderer>().enabled = true;

            // bigBoy.SetActive(true);
            bbIsHere = true;
            frontBbTrigger.SetActive(true);
            backBbTrigger.SetActive(true);
            bBCameraHandlerTrigger.SetActive(true);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {


            if(other.gameObject.tag == "Enemy")
            {
                /*bigBoy.GetComponent<BigBoyController>().enabled = false;
                StartCoroutine(WaitForHeart());*/
            }
        }

        public void DoorLocked()
        {
            doorLockedTrigger.SetActive(false);
            isDoorLocked = false;
            player.GetComponent<PlayerController>().enabled = false;
            doorIsLockedTexte.SetActive(true);
            readyToHide = true;
        }

        public void ReadyToHide()
        {
            player.GetComponent<PlayerController>().enabled = false;
            hideTrigger.SetActive(false);
            timeToHideTexte.SetActive(true);
            readyToHide = false;
            
            Debug.Log("PEUT PAS BOUGER");

            bigBoy.SetActive(true);
            bigBoy.GetComponent<BigBoyController>().enabled = false;
            
            
            StartCoroutine(WaitForInput());
        }
    }
}

