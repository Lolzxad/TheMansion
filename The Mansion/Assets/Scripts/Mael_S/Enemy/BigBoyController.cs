using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


namespace TheMansion
{


    public class BigBoyController : MonoBehaviour
    {
        public float bigBoySpeed;
        public float speedPO;
        public float continueRunning = 5;

        public Transform[] moveSpots;
        private int randomSpot;
        public int baseChance = 1;

        public bool isPatrolling;
        public bool isRunning;
        public bool isGrabbing;
        public bool bBcanMove;
        public bool playerInVision;
        public bool hideFail;

        public bool movingRight = true;
        public float distance;
        public int detectZonePatrol;
        [SerializeField] Transform target1;
        [SerializeField] Transform target2;

        PlayerController playerScript;

        [SerializeField] float waitTime;
        float startWaitTime;

        public GameObject spamInput;
        public GameObject triggerBB;
        GameObject player;
        //[SerializeField] GameObject warning;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            isPatrolling = true;
            bBcanMove = true;

            player = GameObject.FindGameObjectWithTag("Player");
            //target = GameObject.FindGameObjectWithTag("Patrol Point").transform;

            waitTime = startWaitTime;
            randomSpot = Random.Range(0, moveSpots.Length);



        }

        private void Update()
        {
            //Debug.Log(isRunning);

           /* if (gameObject.GetComponent<Renderer>().isVisible && bBcanMove && !playerScript.isGrabbed && !playerScript.isHiding)
            {
                isRunning = true;
                isPatrolling = false;
            }*/

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
                //triggerBB.SetActive(false);
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

        public void TriggerPoursuite()
        {
            
            isRunning = true;
            isPatrolling = false;
            playerInVision = true;
        }

        /*public void OnBecameVisible()
        {
            isRunning = true;
            isPatrolling = false;
            playerInVision = true;
        }*/

       /* public void OnBecameInvisible()
        {
            if (isRunning)
            {
                StartCoroutine(ContinueRunning());
            }
        }*/

        public void RunningOutsideCamera()
        {
            if (isRunning)
            {
                StartCoroutine(ContinueRunning());
            }
        }

        IEnumerator ContinueRunning()
        {
            yield return new WaitForSeconds(continueRunning);

            isRunning = false;
            isPatrolling = true;
            //playerInVision = false;
        }


        public void BBMPA()
        {
            Debug.Log("Big Boy is patrolling");
           // triggerBB.SetActive(true);

          /*  transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, bigBoySpeed * Time.deltaTime);

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
            }*/

            // distance = Vector3.Distance(target.position, transform.position);
            float distance1 = Vector3.Distance(target1.position, transform.position);
            float distance2 = Vector3.Distance(target2.position, transform.position);
            //transform.Translate(Vector2.left * bigBoySpeed * Time.deltaTime);

            if (movingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, target1.position, bigBoySpeed * Time.deltaTime);
            }

            if (!movingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, target2.position, bigBoySpeed * Time.deltaTime);
            }

            if (distance1 <= detectZonePatrol)
            {
                if(movingRight == true)
                {
                    Debug.Log("Target 1 detected");
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }

            }
            if(distance2 <= detectZonePatrol)
            {
                if (movingRight == false)
                {
                    Debug.Log("Target 2 detected");
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
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
            isGrabbing = true;
            
            //canClimb = false;

            ProCamera2DShake.Instance.ConstantShake("GrabBigBoy");

            playerScript.isGrabbed = true;
            playerScript.canMove = false;
            playerScript.playerAnimator.SetTrigger("playerGrabbed");
            playerScript.playerAnimator.SetBool("isGrabbed", true);
            isRunning = false;
            
            spamInput.SetActive(true);            
        }

        public void Stunned()
        {
            Debug.Log("BB is stunned");

            ProCamera2DShake.Instance.StopConstantShaking();

            spamInput.SetActive(false);
            ProCamera2DShake.Instance.Shake("BigBoyStunned");
            playerScript.isGrabbed = false;
            playerScript.canMove = true;
            playerScript.playerAnimator.SetBool("isGrabbed", false);
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
            isGrabbing = false;
            isPatrolling = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectZonePatrol);
        }
    }
}
