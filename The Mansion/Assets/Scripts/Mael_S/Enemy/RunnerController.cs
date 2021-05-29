using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace TheMansion
{


    public class RunnerController : MonoBehaviour
    {

        [Space]
        [Header("Floats")]
        [SerializeField] float runSpeed;
        [SerializeField] float walkSpeed;
        [SerializeField] float waitForRun;
        [SerializeField] float waitForIdle;

        [Space]
        [Header("Int")]
        [SerializeField] int detectZone;

        [Space]
        [Header("GameObjects")]
        public GameObject idlePos;
        public GameObject limitLeft;
        public GameObject limitRight;
        public GameObject spamInput;

        GameObject player;
        PlayerController playerScript;
        SpamInputRunner spamInputController;
        AudioManagerVEVO audioManager;
        Animator animator;


        [Space]
        [Header("Bools")]
        [SerializeField] bool isIdle;
        [SerializeField] bool isLoading;
        [SerializeField] bool isRunning;
        public bool isGrabbing;
        public bool isTired;
        private bool isFacingRight = true;
        [SerializeField] bool isComingBack;
        bool runAgain;

        private Vector3 runnerDirection;


        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            spamInputController = FindObjectOfType<SpamInputRunner>();
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            isIdle = true;

            StartCoroutine(RunnerLaugh());
        }

        private void Update()
        {
            float distance1 = Vector3.Distance(limitLeft.transform.position, transform.position);
            float distance2 = Vector3.Distance(limitRight.transform.position, transform.position);
            float distanceIdle = Vector3.Distance(idlePos.transform.position, transform.position);
            float distancePlayer = Vector3.Distance(player.transform.position, transform.position);

            if (runnerDirection.x < transform.position.x && (isRunning || isComingBack))
            {
                if (!isFacingRight)
                {
                    Flip();
                }
            }

            if (runnerDirection.x > transform.position.x && (isRunning || isComingBack))
            {
                if (isFacingRight)
                {
                    Flip();
                }
            }
            runnerDirection.x = transform.position.x;



            if (gameObject.GetComponent<Renderer>().isVisible && isIdle)
            {
                Steady();             
            }

            if (isRunning /*&& !playerScript.isHiding*/)
            {
                animator.SetBool("isRunning", true);
                animator.ResetTrigger("isPreparing");
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime);
                audioManager.PlayAudio(AudioType.Runner_Run);
                

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

                if (distancePlayer <= detectZone && !isComingBack)
                {
                    RunnerGrab();
                    
                }
            }

            /*if(isRunning && playerScript.isHiding)
            {
                if(distancePlayer > detectZone)
                {
                    isComingBack = true;
                    isRunning = false;
                }
            }*/

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
                animator.SetBool("isWalking", true);

                if (distanceIdle <= detectZone)
                {
                    isIdle = true;
                    isComingBack = false;
                    animator.SetBool("isWalking", false);
                    Debug.Log("COli bien arrivé");
                }
            }

            if (isGrabbing)
            {

                /*if (spamInputController.spamDone)
                {
                    Debug.Log("IsStunned");

                    spamInputController.spam = 0;

                    spamInput.SetActive(false);
                    spamInputController.playerLives--;

                    Stunned();
                    spamInputController.spamDone = false;
                }
                else
                {
                    Debug.Log("Not done");
                }*/
                
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
            animator.SetTrigger("isPreparing");
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
            if (playerScript.isHiding)
            {
                isRunning = false;
                StartCoroutine(Tired());
                //anim fatigue
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetTrigger("hasAttacked");
                audioManager.StopAudio(AudioType.Runner_Run);
                audioManager.PlayAudio(AudioType.Runner_Attack);

                playerScript.isGrabbed = true;
                playerScript.canMove = false;
                playerScript.heartBeat += 20f;
                playerScript.hidingFactor += 20f;
                playerScript.heartbeatSpeed += 2f;
                playerScript.heartOpacity += 1f;
                //playerScript.playerAnimator.SetBool("isGrabbed", true);
                ProCamera2DShake.Instance.ConstantShake("GrabBigBoy");
                isGrabbing = true;
                
                spamInput.SetActive(true);
                isRunning = false;

                Debug.Log("GRAAAAAAAAAAB");
            }
            
        }

        IEnumerator Tired()
        {
            Debug.Log("is tired");
            animator.SetBool("isRunning", false);
            animator.ResetTrigger("hasAttacked");
            audioManager.StopAudio(AudioType.Runner_Run);
            audioManager.PlayAudio(AudioType.RUnner_Fatigue, false, 0.5f);
            yield return new WaitForSeconds(waitForIdle);
            audioManager.StopAudio(AudioType.RUnner_Fatigue, false, 0.5f);
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

            audioManager.PlayAudio(AudioType.Runner_Stun);
            animator.SetBool("isStunned", true);

            ProCamera2DShake.Instance.StopConstantShaking();
            gameObject.GetComponent<Collider2D>().enabled = false;
            spamInput.SetActive(false);
            ProCamera2DShake.Instance.Shake("BigBoyStunned");
            playerScript.isGrabbed = false;
            playerScript.canMove = true;
            //playerScript.playerAnimator.SetBool("isGrabbed", false);           
            StartCoroutine(MobCanMove());
        }

        public void Flip()
        {
            transform.Rotate(new Vector3(0, 180, 0));
            isFacingRight = !isFacingRight;
            //transform.Rotate(new Vector3(0, 180, 0));
        }

        IEnumerator MobCanMove()
        {

            yield return new WaitForSeconds(5f);
            animator.SetBool("isStunned", false);
            gameObject.GetComponent<Collider2D>().enabled = true;
            isComingBack = true;
            isGrabbing = false;
            
        }

        IEnumerator RunnerLaugh()
        {
            yield return new WaitForSeconds(3f);
            audioManager.PlayAudio(AudioType.Runner_Laugh_SFX, false, 0.6f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectZone);
        }
    }
}
