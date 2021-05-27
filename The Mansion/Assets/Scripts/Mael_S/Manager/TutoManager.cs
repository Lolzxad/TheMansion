using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheMansion
{
    public class TutoManager : MonoBehaviour
    {

        public bool isTuto;
        PlayerController playerController;
        BigBoyController bigBoyController;
        VictoryManager victoryManager;
        AudioManagerVEVO audioManager;

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
       // public GameObject hideInput;
        public GameObject hideInput2;
        public GameObject heartInput;
        public GameObject newMovementInput;

        [Space]
        [Header("Des bools")]
        public bool isDoorLocked;
        public bool readyToHide;
        bool playerIsHiding;
        bool bbIsHere;
        public bool timeToHeart;
        bool canHeart;

        

        public void Start()
        {
            isTuto = true;
            playerIsHiding = true;

            playerController = FindObjectOfType<PlayerController>();
            bigBoyController = FindObjectOfType<BigBoyController>();
            audioManager = FindObjectOfType<AudioManagerVEVO>();


            // player.GetComponent<PlayerController>().enabled = false;

              bigBoy.GetComponent<BigBoyController>().enabled = false;
              bigBoy.GetComponent<SpriteRenderer>().enabled = false;
              bigBoy.GetComponent<Collider2D>().enabled = false;
              frontBbTrigger.SetActive(false);
              backBbTrigger.SetActive(false);

              canWinTrigger.SetActive(false);

              heart.GetComponent<Image>().enabled = false;
              heart.GetComponent<Button>().enabled = false;

              audioManager.PlayAudio(AudioType.Ambience_1_ST);
            
        }

        public void okNowMove()
        {
            newMovementInput.SetActive(false);
            
            StartCoroutine(SpawnInputMove());
        }

        public void Update()
        {
            if (playerController.isHiding && playerIsHiding)
            {
                //hideTrigger.SetActive(false);

                //bigBoy.GetComponent<BigBoyController>().enabled = false;
                hideInput2.SetActive(false);
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

            if (playerController.isCalmingHeart)
            {
                heartInput.SetActive(false);
            }

            if(readyToHide == true)
            {
                StartCoroutine(WaitForMove());
            }
        }

        IEnumerator WaitForMove()
        {
            yield return new WaitForSeconds(2);
            //player.GetComponent<PlayerController>().enabled = true;
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
          
            hideInput2.SetActive(true);
            Debug.Log("Show input hide");


            StartCoroutine(WaitToRemoveInputHide());
            Debug.Log("Remove input hide");
            
            
        }

        IEnumerator WaitToRemoveInputHide()
        {
            yield return new WaitForSeconds(5);
            //hideInput.SetActive(false);
            hideInput2.SetActive(false);

            Debug.Log("IL PEUT BOUGER");
            //player.GetComponent<PlayerController>().enabled = true;
        }

        IEnumerator WaitForHeart()
        {
//            player.GetComponent<PlayerController>().enabled = false;

            Debug.Log("Heart");
            //heartInput.SetActive(true);
            playerIsHiding = false;
            heart.GetComponent<Image>().enabled = true;
            heart.GetComponent<Button>().enabled = true;
            heart.GetComponent<Animator>().enabled = true;
            heartTexte.SetActive(true);
            playerController.heartBeat = 300;
            playerController.hidingFactor = 300;
           
           
            yield return new WaitForSeconds(8);

            //heartInput.SetActive(true);
            //player.GetComponent<PlayerController>().enabled = true;

            StartCoroutine(WaitToRemoveInputHeart());
            

            


        }

        IEnumerator WaitToRemoveInputHeart()
        {
            Debug.Log("BeforeRemoveHeart");
            playerController.canMove = false;
            yield return new WaitForSeconds(5);
            Debug.Log("AfterRemoveHeart");
            bigBoy.GetComponent<BigBoyController>().enabled = true;
            heartInput.SetActive(false);
            hideTrigger.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<Collider2D>().enabled = true;

            bigBoy.GetComponent<Collider2D>().enabled = true;
            
            bigBoy.GetComponent<SpriteRenderer>().enabled = true;

            // bigBoy.SetActive(true);
            
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
            
            doorIsLockedTexte.SetActive(true);
            readyToHide = true;
        }

        public void ReadyToHide()
        {
            
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

