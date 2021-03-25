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

        PlayerController playerScript;

        [SerializeField] float waitTime;
        float startWaitTime;

        public GameObject spamInput;
        GameObject player;
        [SerializeField] GameObject warning;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
        }

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
            Debug.Log(isRunning);

            if (gameObject.GetComponent<Renderer>().isVisible && bBcanMove && !playerScript.isGrabbed)
            {
                isRunning = true;
                isPatrolling = false;
            }

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
            if (other.gameObject.tag == "Player" && isRunning)
            {
                BBMG();
            }

            if (other.gameObject.tag == "Hard Hiding Spot" && isRunning && playerScript.isHiding)
            {
                if (playerScript.heartBeat >= 1500)
                {
                    playerScript.isHiding = false;
                    playerScript.gameObject.GetComponent<Collider2D>().enabled = true;
                    BBMG();
                }
                else
                {
                    isPatrolling = true;
                    bBcanMove = true;
                    isRunning = false;                  
                }                
            }
        }

        public void OnBecameVisible()
        {
            isRunning = true;
            isPatrolling = false;
        }

        public void OnBecameInvisible()
        {
            isRunning = false;
            isPatrolling = true;
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
            //canClimb = false;

            playerScript.isGrabbed = true;
            isRunning = false;
            warning.SetActive(false);
            spamInput.SetActive(true);            
        }

        public void Stunned()
        {
            Debug.Log("BB is stunned");

            spamInput.SetActive(false);
            playerScript.isGrabbed = false;
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
