﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace TheMansion
{


    public class RunnerController : MonoBehaviour
    {
        [SerializeField] float runSpeed;
        [SerializeField] float walkSpeed;
        [SerializeField] float waitForRun;
        [SerializeField] float waitForIdle;

        [SerializeField] int detectZone;

        public GameObject idlePos;
        public GameObject limitLeft;
        public GameObject limitRight;


        public GameObject spamInput;
        GameObject player;
        PlayerController playerScript;
        SpamInput spamInputController;

        [SerializeField] bool isIdle;
        [SerializeField] bool isLoading;
        [SerializeField] bool isRunning;
        public bool isGrabbing;
        public bool isTired;
        [SerializeField] bool isComingBack;
        bool runAgain;


        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            spamInputController = FindObjectOfType<SpamInput>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            isIdle = true;

            
        }

        private void Update()
        {
            float distance1 = Vector3.Distance(limitLeft.transform.position, transform.position);
            float distance2 = Vector3.Distance(limitRight.transform.position, transform.position);
            float distanceIdle = Vector3.Distance(idlePos.transform.position, transform.position);
            float distancePlayer = Vector3.Distance(player.transform.position, transform.position);



            if (gameObject.GetComponent<Renderer>().isVisible && isIdle)
            {
                Steady();             
            }

            if (isRunning && !playerScript.isHiding)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime);

                

                if(distance1 <= detectZone)
                {
                    Debug.Log("Too far");
                    LimitReached();
                }

                if (distance2 <= detectZone)
                {
                    Debug.Log("Too far");
                    LimitReached();
                }

                if (distancePlayer <= detectZone)
                {
                    RunnerGrab();
                    
                }
            }

            if(isRunning && playerScript.isHiding)
            {
                if(distancePlayer > detectZone)
                {
                    isComingBack = true;
                    isRunning = false;
                }
            }

            if(isRunning && !gameObject.GetComponent<Renderer>().isVisible && runAgain)
            {
                LimitReached();
            }

            if (isTired)
            {
                StartCoroutine(Tired());
            }

            if (isComingBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, idlePos.transform.position, walkSpeed * Time.deltaTime);

                if (distanceIdle > detectZone)
                {
                    isIdle = true;
                    Debug.Log("COli bien arrivé");
                }
            }

            if (isGrabbing && spamInputController.spamDone)
            {
                Debug.Log("IsStunned");
   
                spamInputController.spam = 0;

                spamInput.SetActive(false);
                spamInputController.playerLives--;

                Stunned();
                spamInputController.spamDone = false;
            }

           
                

            
        }

       /* public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Limit Runner" && isRunning )
            {
                Debug.Log("Bro trigger on point");
                LimitReached();
            }



            if(other.gameObject.name == "Idle Position" && isComingBack)
            {
                Debug.Log("Ayyy");
                isIdle = true;
                isComingBack = false;
            }

            
        }*/

        void Steady()
        {
            Debug.Log("Steady");
            isIdle = false;
            //anim steady
            StartCoroutine(Loading());
            
            //la meuf se dirige en direction du joueur
            //si elle touche les limites elle se met en fatigue
            //si elle touche le joueur c'est le mode grab puis fatigue (ou game over)
            //revient a sa posiiton
        }


        IEnumerator Loading()
        {
            Debug.Log("Loading attack");
            yield return new WaitForSeconds(waitForRun);

            isRunning = true;
        }

        void RunnerGrab()
        {
            isGrabbing = true;
            playerScript.isGrabbed = true;
            spamInput.SetActive(true);
            isRunning = false;

            Debug.Log("GRAAAAAAAAAAB");
        }

        IEnumerator Tired()
        {
            Debug.Log("is tired");
            yield return new WaitForSeconds(waitForIdle);

            isComingBack = true;
            isTired = false;
        }


        void LimitReached()
        {
            Debug.Log("Limit reached");

            if (gameObject.GetComponent<Renderer>().isVisible)
            {
                isRunning = true;
                runAgain = true;
            }
            else
            {
                isRunning = false;
                isTired = true;
                runAgain = false;
            }

           
        }

        public void Stunned()
        {
            Debug.Log("runner is stunned");

            ProCamera2DShake.Instance.StopConstantShaking();
            gameObject.GetComponent<Collider2D>().enabled = false;
            spamInput.SetActive(false);
            ProCamera2DShake.Instance.Shake("BigBoyStunned");
            playerScript.isGrabbed = false;
            playerScript.canMove = true;
            playerScript.playerAnimator.SetBool("isGrabbed", false);
            playerScript.heartBeat = playerScript.heartBeat + 20f;
            playerScript.hidingFactor = playerScript.hidingFactor + 20f;
            StartCoroutine(MobCanMove());
        }

        IEnumerator MobCanMove()
        {

            yield return new WaitForSeconds(5f);
            gameObject.GetComponent<Collider2D>().enabled = true;
            isComingBack = true;
            isGrabbing = false;
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectZone);
        }
    }
}
