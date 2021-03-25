using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class RunnerController : MonoBehaviour
    {
        [SerializeField] float runSpeed;
        [SerializeField] float walkSpeed;
        [SerializeField] float waitForRun;
        [SerializeField] float waitForIdle;

        public GameObject idlePos;
        public GameObject limitLeft;
        public GameObject limitRight;


        public GameObject spamInput;
        GameObject player;
        PlayerController playerScript;

        [SerializeField] bool isIdle;
        [SerializeField] bool isLoading;
        [SerializeField] bool isRunning;
        public bool isGrabbing;
        public bool isTired;
        [SerializeField] bool isComingBack;


        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            isIdle = true;
        }

        private void Update()
        {
            if(gameObject.GetComponent<Renderer>().isVisible && isIdle)
            {
                Steady();             
            }

            if (isRunning)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime);
            }

            if (isTired)
            {
                StartCoroutine(Tired());
            }

            if (isComingBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, idlePos.transform.position, walkSpeed * Time.deltaTime);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
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

            if(other.gameObject.tag == "Player")
            {
                RunnerGrab();
            }
        }

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
            playerScript.isGrabbed = true;
            spamInput.SetActive(true);
        }

        IEnumerator Tired()
        {
            Debug.Log("is tired");
            yield return new WaitForSeconds(waitForIdle);

            isComingBack = true;
        }


        void LimitReached()
        {
            Debug.Log("Limit reached");

            isRunning = false;
            isTired = true;
        }
    }
}
