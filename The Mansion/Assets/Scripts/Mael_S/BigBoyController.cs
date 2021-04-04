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
        public int baseChance = 1;

        public bool isPatrolling;
        public bool isRunning;
        public bool isGrabbing;
        public bool bBcanMove;
        public bool playerInVision;
        public bool hideFail;

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

            if (gameObject.GetComponent<Renderer>().isVisible && bBcanMove && !playerScript.isGrabbed && !playerScript.isHiding)
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

            if (playerInVision)
            {
                playerScript.heartBeat = playerScript.heartBeat + 1 * Time.deltaTime;
                playerScript.hidingFactor = playerScript.hidingFactor + 1 * Time.deltaTime;
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

                Debug.Log("Touching hiding spot");
                Debug.Log(isRunning);
                HideCheck();

                if (hideFail)
                {
                    playerScript.isHiding = false;
                    playerScript.gameObject.GetComponent<Collider2D>().enabled = true;
                    playerScript.transform.position = playerScript.basePosition;
                    playerScript.playerSprite.transform.position = playerScript.baseSpritePosition;
                    BBMG();
                    hideFail = false;
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
            playerInVision = true;
        }

        public void OnBecameInvisible()
        {
            isRunning = false;
            isPatrolling = true;
            playerInVision = false;
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
            playerScript.heartBeat = playerScript.heartBeat + 20f;
            playerScript.hidingFactor = playerScript.hidingFactor + 20f;
            StartCoroutine(MobCantMove());   
        }
        public void HideCheck()
        {
            //Formule pour calculer la probabilité de se faire chopper
            if (playerScript.hidingFactor * Random.Range(1, 6) >= 100)
            {
                hideFail = true;
            }
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
