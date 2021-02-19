using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class BigBoyController : MonoBehaviour
    {
        public float bigBoySpeed;
        public float speedPO;

        public Transform[] moveSpots;
        private int randomSpot;

        public bool isPatrolling;
        public bool isRunning;
        public bool isGrabbing;
        public bool bBcanMove;

        [SerializeField] float waitTime;
        float startWaitTime;

        public GameObject spamInput;
        GameObject player;
        [SerializeField] GameObject warning;
        

        private void Start()
        {
            isPatrolling = true;
            bBcanMove = true;

            player = GameObject.FindGameObjectWithTag("Player");


            waitTime = startWaitTime;
            randomSpot = Random.Range(0, moveSpots.Length);
        }

        private void Update()
        {
            if (isPatrolling && bBcanMove)
            {
                BBMPA();
            }

            if (isRunning)
            {
                BBMPO();
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "PlayerTrigger" && isRunning)
            {
                BBMG();
            }
        }

        public void OnBecameVisible()
        {
            isRunning = true;
            isPatrolling = false;
        }


        public void BBMPA()
        {
            Debug.Log("Big Boy is patrolling");

            transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, bigBoySpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }

        void BBMPO()
        {
            Debug.Log("Pursuit of happiness");

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speedPO * Time.deltaTime);

            //si le joueur sort de son champ de vision/distance alors il va à sa dernière position
        }


        public void BBMG()
        {
            Debug.Log("Mode Grab");
            //playerController.canMove = false;
            isRunning = false;
            warning.SetActive(false);
            spamInput.SetActive(true);
            
        }

        public void Stunned()
        {
            Debug.Log("BB is stunned");

            spamInput.SetActive(false);
            //playerController.canMove = true;
            StartCoroutine(MobCantMove());   
        }

        IEnumerator MobCantMove()
        {
            bBcanMove = false;
            yield return new WaitForSeconds(5f);
            bBcanMove = true;
            isPatrolling = true;
        }

    }
}
