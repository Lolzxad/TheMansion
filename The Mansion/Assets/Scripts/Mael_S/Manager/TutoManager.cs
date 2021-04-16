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

        [Space]
        [Header("Core GameObjects-")]
        public GameObject player;
        public GameObject bigBoy;
        public GameObject heart;

        [Space]
        [Header("Inputs")]
        public GameObject movementInput;
        public GameObject hideInput;
        public GameObject heartInput;

        [Space]
        [Header("Des bools")]
        public bool isDoorLocked;
        public bool readyToHide;
        bool playerIsHiding;

        public void Start()
        {
            isTuto = true;
            playerIsHiding = true;

            playerController = FindObjectOfType<PlayerController>();
            bigBoyController = FindObjectOfType<BigBoyController>();

            

            player.GetComponent<PlayerController>().enabled = false;
            bigBoy.GetComponent<BigBoyController>().enabled = false;
            bigBoy.GetComponent<SpriteRenderer>().enabled = false;
            frontBbTrigger.SetActive(false);
            backBbTrigger.SetActive(false);
            bBCameraHandlerTrigger.SetActive(false);



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

            if(bigBoyController.bBcanMove == false)
            {
                Debug.Log("BB can't move AAAAAAAAAh");

                bigBoy.GetComponent<BigBoyController>().enabled = false;
                //stunTexte.SetActive(true);
            }

            if(readyToHide == true)
            {
                StartCoroutine(WaitForMove());
            }
        }

        IEnumerator WaitForMove()
        {
            yield return new WaitForSeconds(5);
            player.GetComponent<PlayerController>().enabled = true;
        }


        IEnumerator SpawnInputMove()
        {
            //Debug.Log("Spawn input move");
            introTexte.SetActive(true);
            
            yield return new WaitForSeconds(7);
            movementInput.SetActive(true);

            StartCoroutine(WaitToRemoveInputMove());

        }

        IEnumerator WaitToRemoveInputMove()
        {
            yield return new WaitForSeconds(3);
            movementInput.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;
        }

        IEnumerator WaitForInput()
        {
            
            Debug.Log("Hide input");
            

            yield return new WaitForSeconds(5);
            hideInput.SetActive(true);
            Debug.Log("Show input hide");


            StartCoroutine(WaitToRemoveInputHide());
            Debug.Log("Remove input hide");
            
            player.GetComponent<PlayerController>().enabled = true;
        }

        IEnumerator WaitToRemoveInputHide()
        {
            yield return new WaitForSeconds(5);
            hideInput.SetActive(false);

        }

        IEnumerator WaitForHeart()
        {
            Debug.Log("Heart");
            playerIsHiding = false;

            heartTexte.SetActive(true);
            heart.SetActive(true);
            heart.GetComponent<HeartAnimation>().enabled = false;
            yield return new WaitForSeconds(5);

            heartInput.SetActive(true);

            StartCoroutine(WaitToRemoveInputHeart());

            heart.GetComponent<HeartAnimation>().enabled = true;  


        }

        IEnumerator WaitToRemoveInputHeart()
        {
            yield return new WaitForSeconds(5);
            heartInput.SetActive(false);
            hideTrigger.SetActive(false);

            playerController.heartBeat = 300;
            playerController.hidingFactor = 300;
            bigBoy.GetComponent<BigBoyController>().enabled = true;
            bigBoy.GetComponent<SpriteRenderer>().enabled = true;
            frontBbTrigger.SetActive(true);
            backBbTrigger.SetActive(true);
            //bBCameraHandlerTrigger.SetActive(true);
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
            hideTrigger.SetActive(false);
            timeToHideTexte.SetActive(true);
            readyToHide = false;
            player.GetComponent<PlayerController>().enabled = false;
            bigBoy.SetActive(true);
            bigBoy.GetComponent<BigBoyController>().enabled = false;
            
            
            StartCoroutine(WaitForInput());
        }
    }
}

