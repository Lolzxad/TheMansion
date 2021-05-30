using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


namespace TheMansion
{


    public class BigBoyController : MonoBehaviour
    {
        public Animator bigBoyAnimator;

        [Space]
        [Header("Floats")]
        public float bigBoySpeed;
        public float speedPO;
        public float continueRunning = 8;
        [SerializeField] float waitTime;
        float startWaitTime;
        public float distance;
        public float rechercheTime;
        private float playerDistance;
        private float lastPlayerDistance = 100f;
        [SerializeField] float bbVision;

        float defaultSpeed;
        float defaultSpeedPO;

        [Space]
        [Header("Int")]
        public int detectZonePatrol;
        private int randomSpot;
        public int baseChance = 1;

        [Space]
        [Header("Bools")]
        public bool isPatrolling;
        public bool isRunning;
        public bool isGrabbing;
        public bool bBcanMove;
        public bool playerInVision;
        public bool hideCheck;
        public bool isFacingRight;
        public bool canBeCalled;
        public bool isCalled;
        public bool movingLeft = true;
        public bool hasDetected;
    
        
        

        [Space]
        [Header("GameObjects")]
        [SerializeField] Transform target1;
        [SerializeField] Transform target2;
        [SerializeField] Transform crawler;
        public GameObject playerSprite;
        public GameObject spamInput;
        public GameObject triggerBB;
        public GameObject detection;
        public Transform[] moveSpots;
        private GameObject player;

        PlayerController playerScript;
        TutoManager tuto;
        AudioManagerVEVO audioManager;
        private Vector2 bigBoyDirection;
       

    

       
        //[SerializeField] GameObject warning;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
           // tuto = FindObjectOfType<TutoManager>();
        }

        private void Start()
        {
            isPatrolling = true;
            bBcanMove = true;

            player = GameObject.FindGameObjectWithTag("Player");
            //target = GameObject.FindGameObjectWithTag("Patrol Point").transform;

            waitTime = startWaitTime;
            randomSpot = Random.Range(0, moveSpots.Length);

            defaultSpeed = bigBoySpeed;
            defaultSpeedPO = speedPO;
        }

        private void Update()
        {            playerDistance = Vector2.Distance(player.transform.position, transform.position);

            if (!playerScript.isHiding && playerInVision)
            {
                lastPlayerDistance = playerDistance;
            }
            //Debug.Log(lastPlayerDistance);

            if (bigBoyDirection.x < transform.position.x)
            {
                //Debug.Log("Moving Right");

                if (!isFacingRight)
                {
                    Flip();
                }               
            }

            if (bigBoyDirection.x > transform.position.x)
            {
                //Debug.Log("Moving Left");

                if (isFacingRight)
                {
                    Flip();
                }

            }

            if (isGrabbing == true)
            {
                Handheld.Vibrate();
            }
            bigBoyDirection.x = transform.position.x;
            //Debug.Log(isRunning);

            /* if (gameObject.GetComponent<Renderer>().isVisible && bBcanMove && !playerScript.isGrabbed && !playerScript.isHiding)
             {
                 isRunning = true;
                 isPatrolling = false;
             }*/
            if (!transform.hasChanged)
            {
                bigBoyAnimator.SetBool("isWalking", false);
            }
            transform.hasChanged = false;

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
                playerScript.heartBeat += 1 * Time.deltaTime;
                playerScript.hidingFactor += 1 * Time.deltaTime;
                playerScript.heartbeatSpeed += 0.1f * Time.deltaTime;
                playerScript.heartOpacity += 0.05f * Time.deltaTime;
            }

            if (isCalled)
            {
                if (canBeCalled && !isGrabbing && !isRunning)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(crawler.transform.position.x, transform.position.y), speedPO * Time.deltaTime);

                }
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player" && isRunning && !playerScript.isHiding)
            {
                Debug.Log("Gotcha");
                //triggerBB.SetActive(false);
                BBMG();
            }

            if (other.gameObject.tag == "Player" && playerScript.isHiding)
            {
                Debug.Log("Big boy Touching hiding spot");

                if (playerScript.heartBeat >= 120 && !hideCheck && !isRunning)
                {
                    bBcanMove = false;
                    isPatrolling = false;
                    StartCoroutine(ModeRecherche());
                }

                if (isRunning && lastPlayerDistance < 10f)
                {
                    //Le big boy a vu le joueur se cacher
                    isRunning = false;
                    playerScript.isHiding = false;
                    playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    playerScript.gameObject.GetComponent<Collider2D>().enabled = true;
                    playerScript.playerAnimator.SetBool("isHiding", false);
                    playerScript.playerRb.gravityScale = playerScript.defaultGravity;
                    BBMG();
                }

                if (isRunning && (lastPlayerDistance > 10f && lastPlayerDistance <= 15f))
                {
                    isRunning = false;
                    playerScript.hidingFactor += 10f;
                    StartCoroutine(ModeRecherche());
                }

                if (isRunning && lastPlayerDistance > 15f)
                {
                    isRunning = false;
                    bBcanMove = true;
                    isPatrolling = true;
                }
            }
        }

        IEnumerator ModeRecherche()
        {
                      
            //lance anim recherche
            Debug.Log("Mode recherche en cours");
            bigBoySpeed = 0;
            speedPO = 0;
            bigBoyAnimator.SetBool("isSearching", true);
            yield return new WaitForSecondsRealtime(rechercheTime);

            Debug.Log("Mode recherche terminé");
            bigBoySpeed = defaultSpeed;
            speedPO = defaultSpeedPO;
            bigBoyAnimator.SetBool("isSearching", false);
            HideCheck();
            yield return new WaitForSecondsRealtime(5f);
            hideCheck = false;
        }

        public void TriggerPoursuite()
        {
            
            isRunning = true;
            isPatrolling = false;
            playerInVision = true;

            if (!hasDetected)
            {
                StartCoroutine("Detection");
                hasDetected = true;
            }           
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
            hasDetected = false;

            //
            //Debug.Log("Big Boy is patrolling");
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
                float crawlerDis = Vector3.Distance(crawler.position, transform.position);
                //transform.Translate(Vector2.left * bigBoySpeed * Time.deltaTime);

                if (movingLeft && bBcanMove)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target1.position.x, transform.position.y), bigBoySpeed * Time.deltaTime);
                    bigBoyAnimator.SetBool("isWalking", true);                                
                }

                if (!movingLeft && bBcanMove)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target2.position.x, transform.position.y), bigBoySpeed * Time.deltaTime);
                    bigBoyAnimator.SetBool("isWalking", true);           
                }

                if (distance1 <= detectZonePatrol)
                {
                    if (movingLeft == true)
                    {
                        Debug.Log("Target 1 detected");
                        //transform.eulerAngles = new Vector3(0, -180, 0);
                        Flip();
                        movingLeft = false;
                    }

                }
                if (distance2 <= detectZonePatrol)
                {
                    if (movingLeft == false)
                    {
                        Debug.Log("Target 2 detected");
                        //transform.eulerAngles = new Vector3(0, 0, 0);
                        Flip();
                        movingLeft = true;
                    }
                }

                if(crawlerDis <= detectZonePatrol)
                {
                     CrawlerReached();
                }
            
           
           
        }

        void BBMPO()
        {
            Debug.Log("Pursuit of happiness");

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speedPO * Time.deltaTime);
            bigBoyAnimator.SetBool("isWalking", true);

            if (playerScript.usingLadder)
            {
                isPatrolling = true;
                isRunning = false;
            }


            //si le joueur sort de son champ de vision/distance alors il va à sa dernière position
        }


        public void BBMG()
        {
            Debug.Log("Mode Grab");
            isGrabbing = true;

            if (playerSprite.transform.position.x < transform.position.x && playerSprite.transform.rotation.eulerAngles.y >= 180)
            {
                playerScript.Flip();
            }

            if (playerSprite.transform.position.x > transform.position.x && playerSprite.transform.rotation.eulerAngles.y < 180)
            {
                playerScript.Flip();
            }
            playerScript.playerAnimator.SetBool("isGrabbed", true);
            bigBoyAnimator.SetTrigger("hasGrabbed");
            bBcanMove = false;

            ProCamera2DShake.Instance.ConstantShake("GrabBigBoy");           

            playerScript.isGrabbed = true;
            playerScript.heartBeat += 20f;
            playerScript.hidingFactor += 20f;
            playerScript.heartbeatSpeed += 2f;
            playerScript.heartOpacity += 1f;

            audioManager.PlayAudio(AudioType.Player_Choke_SFX);
            playerScript.canMove = false;
            /*playerSprite.transform.position = grabSpot.transform.position;
            grabSpot.SetActive(true);*/

            
            isRunning = false;
            
            spamInput.SetActive(true);
        }

        public void Stunned()
        {
            Debug.Log("BB is stunned");

            

            bigBoyAnimator.ResetTrigger("hasGrabbed");
            bigBoyAnimator.SetBool("isStunned", true);
            isGrabbing = false;

            ProCamera2DShake.Instance.StopConstantShaking();
            gameObject.GetComponent<Collider2D>().enabled = false;
            //grabSpot.SetActive(false);
            spamInput.SetActive(false);
            ProCamera2DShake.Instance.Shake("BigBoyStunned");
            ProCamera2D.Instance.CenterOnTargets();
            playerScript.playerAnimator.SetBool("isGrabbed", false);
            playerScript.isGrabbed = false;         
            playerScript.canMove = true;
            playerScript.playerLives -= 1;
            StartCoroutine(MobCanMove());   
        }
        public void HideCheck() 
        {
            hideCheck = true;
            //Formule pour calculer la probabilité de se faire chopper
            if (playerScript.hidingFactor * Random.Range(1, 6) >= 100)
            {              
                playerScript.isHiding = false;
                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 7;
                playerScript.gameObject.GetComponent<Collider2D>().enabled = true;
                playerScript.playerAnimator.SetBool("isHiding", false);
                playerScript.playerRb.gravityScale = playerScript.defaultGravity;
                BBMG();
                Debug.Log("AAAAAAAAAAAH");
            }
            else
            {
                Debug.Log("hide passed");
                isPatrolling = true;
                bBcanMove = true;
                isRunning = false;
            }
        }

        IEnumerator MobCanMove()
        {
            
            yield return new WaitForSeconds(5f);
            gameObject.GetComponent<Collider2D>().enabled = true;
            bigBoyAnimator.SetBool("isStunned", false);
            bBcanMove = true;           
            isPatrolling = true;
        }

        IEnumerator Detection()
        {
            detection.SetActive(true);
            detection.GetComponent<Animator>().SetBool("hasDetected", true);
            yield return new WaitForSeconds(1f);
            detection.SetActive(false);
            detection.GetComponent<Animator>().SetBool("hasDetected", false);
            yield break;
        }

        public void Flip()
        {
            transform.Rotate(new Vector3(0, 180, 0));
            isFacingRight = !isFacingRight;
            //transform.Rotate(new Vector3(0, 180, 0));
        }


      

        public void CrawlerReached()
        {
            isCalled = false;
            canBeCalled = false;
            StartCoroutine(WaitBeforeBeCalled());
        }

        IEnumerator WaitBeforeBeCalled()
        {
            yield return new WaitForSeconds(5);            audioManager.StopAudio(AudioType.Phonograph);
            canBeCalled = true;
        }
        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectZonePatrol);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, bbVision);
        }
    }
}
