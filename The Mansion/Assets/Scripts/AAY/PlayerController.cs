using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Com.LuisPedroFonseca.ProCamera2D;

namespace TheMansion
{
    public class PlayerController : MonoBehaviour
    {
        private MenuManager MenuManagerScript;

        public Animator playerAnimator;
        public Animator heartAnimator;

        public float stamina = 100f;
        public float heartBeat = 100f;
        public float hidingFactor = 1f;
        public float defaultGravity;
        private float heartbeatSpeed = 0.1f;
        private float lastStamina = 100f;
        

        private bool canHide;
        private bool canUseLadder;
        public bool isCalmingHeart;
        public bool isRegening;
        public bool canMove = true;
        public bool isFacingRight = true;
        public bool isMouse;
        public bool isHiding;
        public bool isGrabbed;
        public bool usingLadder;
        public bool isRunning;

        public Transform BasePosition;
        public Transform WalkRight;
        public Transform WalkLeft;
        public Transform RunRight;
        public Transform RunLeft;

        
        public GameObject playerSprite;
        public GameObject hideFeedback;
        public GameObject heartFeedbackLevel1;
        public GameObject heartFeedbackLevel2;
        public GameObject heartFeedbackLevel3;
        public Rigidbody2D playerRb;
        public Vector3 baseSpritePosition;
        public Vector3 basePosition;
        public Vector3 ladderTop;
        public Vector3 ladderBottom;

        public StaminaBar staminaBarScript;
        public GameObject staminaBar;

        TouchPhase touchPhase = TouchPhase.Ended;

        private void Awake()
        {
            MenuManagerScript = FindObjectOfType<MenuManager>();
            playerRb = GetComponent<Rigidbody2D>();
            defaultGravity = playerRb.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {

            if (isGrabbed)
            {
                ProCamera2D.Instance.CenterOnTargets();
            }

            if (usingLadder)
            {
                ProCamera2D.Instance.CenterOnTargets();
            }

            //Debug.Log(stamina);
            if (heartbeatSpeed < 1)
            {
                heartAnimator.SetFloat("speed", 1 + heartbeatSpeed);
            }

            staminaBarScript.SetStamina(stamina);

            if (lastStamina == stamina && staminaBar.activeSelf && !isRunning && !isCalmingHeart && !isRegening)
            {
                StartCoroutine(StaminaBarDisappearance());                
            }
            lastStamina = stamina;

            HeartFeedback();

            if (!usingLadder)
            {
                playerAnimator.SetBool("isUsingLadder", false);
            }

            if (!isMouse)
            {
                if (Input.touchCount > 0)
                {

                    Touch touch = Input.GetTouch(0);

                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);

                    if (Input.GetTouch(0).phase == touchPhase)
                    {
                        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosition2D, Camera.main.transform.forward);

                        if (hitInformation.collider != null)
                        {
                            //We should have hit something with a 2D Physics collider!
                            GameObject touchedObject = hitInformation.transform.gameObject;
                            //touchedObject should be the object someone touched.
                            Debug.Log("Touched " + touchedObject.transform.name);

                            if (isHiding)
                            {
                                if (touchedObject.tag == "Hard Hiding Spot" && touchedObject.GetComponent<SpriteMask>().enabled)
                                {
                                    touchedObject.GetComponent<SpriteMask>().enabled = !touchedObject.GetComponent<SpriteMask>().enabled;
                                    isHiding = false;
                                    canMove = true;
                                    hideFeedback.SetActive(false);
                                    playerAnimator.SetBool("isHiding", false);
                                    playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 3;
                                    playerRb.gravityScale = defaultGravity;
                                    gameObject.GetComponent<Collider2D>().enabled = true;
                                    transform.position = touchedObject.transform.GetChild(0).transform.position;
                                    //gameObject.transform.Find("Sprite").GetComponent<Collider2D>().enabled = true;
                                    /*transform.position = basePosition;
                                    playerSprite.transform.position = baseSpritePosition;*/
                                }
                            }
                            else
                            {
                                if (touchedObject.tag == "Hard Hiding Spot" && touchedObject.GetComponent<SpriteMask>().enabled && canHide && !isGrabbed && !usingLadder)
                                {
                                    isHiding = true;
                                    canMove = false;
                                    hideFeedback.SetActive(true);
                                    playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                    playerRb.gravityScale = 0;

                                    if (playerSprite.transform.position.x < touchedObject.transform.position.x && playerSprite.transform.rotation.eulerAngles.y >= 180)
                                    {
                                        //Debug.Log("You're left from the closet");
                                        Flip();
                                    }

                                    if (playerSprite.transform.position.x > touchedObject.transform.position.x && playerSprite.transform.rotation.eulerAngles.y < 180)
                                    {
                                        //Debug.Log("You're right from the closet");
                                        Flip();
                                    }
                                    playerAnimator.SetBool("isHiding", true);

                                    //Debug.Log("Is Hiding");
                                    basePosition = transform.position;
                                    baseSpritePosition = playerSprite.transform.position;
                                    transform.position = touchedObject.transform.position;
                                    playerSprite.transform.position = touchedObject.transform.position;
                                    StartCoroutine(Hiding());
                                }
                            }

                            if (touchedObject.tag == "LadderDown")
                            {
                                ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                                ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;

                                if (!usingLadder && !isHiding && canUseLadder)
                                {
                                    StartCoroutine(DownLadder());
                                }
                                else
                                {
                                    StartCoroutine(OffLadder());
                                }
                            }

                            if (touchedObject.tag == "LadderUp")
                            {
                                ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                                ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;

                                if (!usingLadder && !isHiding && canUseLadder)
                                {
                                    StartCoroutine(UpLadder());
                                }
                                else
                                {
                                    StartCoroutine(OffLadder());
                                }
                            }

                            if (touchedObject.tag == "StoryLore")
                            {
                                if (touchedObject.name == "Story 1")
                                {
                   
                                    //MenuManagerScript.story1Get = true;
                                    PlayerPrefs.SetInt("Story1", (MenuManagerScript.story1Get ? 1 : 0));
                                    touchedObject.SetActive(false);
                                    Debug.Log("STORY 1");

                                }

                                if (touchedObject.name == "Story 2")
                                {
                                    //MenuManagerScript.story2Get = true;
                                    PlayerPrefs.SetInt("Story2", (MenuManagerScript.story2Get ? 1 : 0));
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 3")
                                {                                   
                                    PlayerPrefs.SetInt("Story3", (MenuManagerScript.story3Get ? 1 : 0));
                                    MenuManagerScript.story3Get = true;
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 4")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story4", (MenuManagerScript.story4Get ? 1 : 0));
                                    touchedObject.SetActive(false);
                                }

                            }


                        }
                    }

                    if (touchPosition.x > BasePosition.position.x && touchPosition.x <= WalkRight.position.x && canMove)
                    {
                        //Debug.Log("Walk Right");
                        if (!isFacingRight)
                        {
                            Flip();
                        }
                        playerAnimator.SetBool("isWalking", true);
                        isRunning = false;
                        transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x < BasePosition.position.x && touchPosition.x >= WalkLeft.position.x && canMove)
                    {
                        //Debug.Log("Walk Left");
                        if (isFacingRight)
                        {
                            Flip();
                        }
                        playerAnimator.SetBool("isWalking", true);
                        isRunning = false;
                        transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x > WalkRight.position.x && touchPosition.x <= RunRight.position.x && stamina > 0 && canMove)
                    {
                        //Debug.Log("Run Right");
                        if (!isFacingRight)
                        {
                            Flip();
                        }
                        if (stamina > 0f)
                        {
                            playerAnimator.SetBool("isRunning", true);
                            transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                        }

                        else
                        {
                            playerAnimator.SetBool("isWalking", true);
                            transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                        }
                        isRunning = true;
                        StartCoroutine(StaminaLoss());
                    }
                    

                    if (touchPosition.x < WalkLeft.position.x && touchPosition.x >= RunLeft.position.x && stamina > 0 && canMove)
                    {
                        //Debug.Log("Run Left");
                        if (isFacingRight)
                        {
                            Flip();
                        }
                        playerAnimator.SetBool("isRunning", true);

                        if (stamina > 0f)
                        {
                            playerAnimator.SetBool("isRunning", true);
                            transform.Translate((Vector3.left * Time.deltaTime) * 10f);
                        }

                        else
                        {
                            playerAnimator.SetBool("isWalking", true);
                            transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                        }
                        StartCoroutine(StaminaLoss());
                    }                   
                }
            }

            if (isMouse)
            {
                playerMouse();
            }

          
            if (!transform.hasChanged)
            {
                Debug.Log("hasn'tChanged");
                StartCoroutine("StandingRegen");
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("isRunning", false);               
            }
            //transform.hasChanged = false;

            if (transform.hasChanged)
            {
                Debug.Log("hasChanged");
                StopCoroutine("StandingRegen");
                isRegening = false;
                transform.hasChanged = false;
            }
            

            

            if (heartBeat < 100f)
            {
                heartBeat = 100f;   
            }

            if (stamina < 0f)
            {
                stamina = 0f;
            }

            if (hidingFactor < 1)
            {
                hidingFactor = 1;
            }

            if (heartbeatSpeed < 0.1f)
            {
                heartbeatSpeed = 0.1f;
            }
        }
        void OnTriggerStay2D(Collider2D InteractableObject)
        {
            if (InteractableObject.tag == "Hard Hiding Spot")
            {
                //var hidingSpot = InteractableObject.gameObject;
                if (isHiding)
                {
                    InteractableObject.GetComponent<OutlineActivator>().DisableOutline();
                }
                else
                {
                    canHide = true;
                    InteractableObject.GetComponent<OutlineActivator>().EnableOutline();
                    if (!InteractableObject.GetComponent<SpriteMask>().enabled)
                    {
                        InteractableObject.GetComponent<SpriteMask>().enabled = !InteractableObject.GetComponent<SpriteMask>().enabled;
                    }                                
                }  
            }

            if (InteractableObject.tag == "LadderDown" || InteractableObject.tag == "LadderUp")
            {

                if (usingLadder)
                {
                    InteractableObject.transform.parent.GetComponent<OutlineActivator>().DisableOutline();
                    InteractableObject.transform.parent.gameObject.transform.Find("UseLadderDown").gameObject.SetActive(false);
                }
                else
                {
                    canUseLadder = true;
                    InteractableObject.transform.parent.GetComponent<OutlineActivator>().EnableOutline();
                    InteractableObject.transform.parent.gameObject.transform.Find("UseLadderDown").gameObject.SetActive(true);
                }                           
            }
        }
        void OnTriggerExit2D(Collider2D InteractableObject)
        {
            if (InteractableObject.tag == "Hard Hiding Spot")
            {
                canHide = false;
                InteractableObject.GetComponent<OutlineActivator>().DisableOutline();
            }

            if (InteractableObject.tag == "LadderDown" || InteractableObject.tag == "LadderUp")
            {
                canUseLadder = false;
                InteractableObject.transform.parent.GetComponent<OutlineActivator>().DisableOutline();
                InteractableObject.transform.parent.gameObject.transform.Find("UseLadderDown").gameObject.SetActive(false);
            }
        }

        void playerMouse()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 clickPosition2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D clickInformation = Physics2D.Raycast(clickPosition2D, Camera.main.transform.forward);

                    if (clickInformation.collider != null)
                    {
                        //We should have hit something with a 2D Physics collider!
                        GameObject touchedObject = clickInformation.transform.gameObject;
                        //touchedObject should be the object someone touched.
                        Debug.Log("Touched " + touchedObject.transform.name);

                        if (isHiding)
                        {
                            if (touchedObject.tag == "Hard Hiding Spot" && touchedObject.GetComponent<SpriteMask>().enabled)
                            {
                                touchedObject.GetComponent<SpriteMask>().enabled = !touchedObject.GetComponent<SpriteMask>().enabled;
                                isHiding = false;
                                canMove = true;
                                hideFeedback.SetActive(false);
                                playerAnimator.SetBool("isHiding", false);
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 3;
                                playerRb.gravityScale = defaultGravity;
                                gameObject.GetComponent<Collider2D>().enabled = true;
                                transform.position = touchedObject.transform.GetChild(0).transform.position;
                                //gameObject.transform.Find("Sprite").GetComponent<Collider2D>().enabled = true;
                                /*transform.position = basePosition;
                                playerSprite.transform.position = baseSpritePosition;*/
                            }
                        }
                        else
                        {
                            if (touchedObject.tag == "Hard Hiding Spot" && touchedObject.GetComponent<SpriteMask>().enabled && canHide && !isGrabbed && !usingLadder)
                            {
                                isHiding = true;
                                canMove = false;
                                hideFeedback.SetActive(true);
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                playerRb.gravityScale = 0;

                                if (playerSprite.transform.position.x < touchedObject.transform.position.x && playerSprite.transform.rotation.eulerAngles.y >= 180)
                                {
                                    Debug.Log("You're left from the closet");
                                    Flip();
                                }

                                if (playerSprite.transform.position.x > touchedObject.transform.position.x && playerSprite.transform.rotation.eulerAngles.y < 180)
                                {
                                    Debug.Log("You're right from the closet");
                                    Flip();
                                }
                                playerAnimator.SetBool("isHiding", true);

                                //Debug.Log("Is Hiding");
                                basePosition = transform.position;
                                baseSpritePosition = playerSprite.transform.position;
                                transform.position = touchedObject.transform.position;
                                playerSprite.transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }

                        if (touchedObject.tag == "LadderDown")
                        {
                            ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                            ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;

                            if (!usingLadder && !isHiding && canUseLadder)
                            {
                                StartCoroutine(DownLadder());
                            }
                            else
                            {
                                StartCoroutine(OffLadder());
                            }
                        }

                        if (touchedObject.tag == "LadderUp")
                        {
                            ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                            ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;

                            if (!usingLadder && !isHiding && canUseLadder)
                            {                               
                                StartCoroutine(UpLadder());
                            }
                            else
                            {
                                StartCoroutine(OffLadder());
                            }
                        }

                        if (touchedObject.name == "Story 1")
                        {

                            MenuManagerScript.story1Get = true;
                            PlayerPrefs.SetInt("Story1", (MenuManagerScript.story1Get ? 1 : 0));
                            touchedObject.SetActive(false);
                            Debug.Log("STORY 1");

                        }

                        if (touchedObject.name == "Story 2")
                        {
                            MenuManagerScript.story2Get = true;
                            PlayerPrefs.SetInt("Story2", (MenuManagerScript.story2Get ? 1 : 0));
                            touchedObject.SetActive(false);
                        }

                        if (touchedObject.name == "Story 3")
                        {
                            touchedObject.SetActive(false);
                            PlayerPrefs.SetInt("Story3", (MenuManagerScript.story3Get ? 1 : 0));
                            MenuManagerScript.story3Get = true;
                        }

                        if (touchedObject.name == "Story 4")
                        {
                            MenuManagerScript.story4Get = true;
                            PlayerPrefs.SetInt("Story4", (MenuManagerScript.story4Get ? 1 : 0));
                            touchedObject.SetActive(false);
                        }
                    }
                }

                if (clickPosition2D.x > BasePosition.position.x && clickPosition2D.x <= WalkRight.position.x && canMove)
                {
                    //Debug.Log("Walk Right");
                    if (!isFacingRight)
                    {
                        Flip();
                    }
                    playerAnimator.SetBool("isWalking", true);
                    transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    isRunning = false;
                }

                if (clickPosition2D.x < BasePosition.position.x && clickPosition2D.x >= WalkLeft.position.x && canMove)
                {
                    //Debug.Log("Walk Left");
                    if (isFacingRight)
                    {
                        Flip();
                    }
                    playerAnimator.SetBool("isWalking", true);
                    transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    isRunning = false;
                }

                if (clickPosition2D.x > WalkRight.position.x && clickPosition2D.x <= RunRight.position.x && canMove)
                {
                    //Debug.Log("Run Right");
                    if (!isFacingRight)
                    {
                        Flip();
                    }

                    if (stamina > 0f)
                    {
                        playerAnimator.SetBool("isRunning", true);
                        transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                    }
                    else
                    {
                        playerAnimator.SetBool("isWalking", true);
                        transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    }
                    isRunning = true;
                    StartCoroutine(StaminaLoss());
                }

                if (clickPosition2D.x < WalkLeft.position.x && clickPosition2D.x >= RunLeft.position.x && canMove)
                {
                    //Debug.Log("Run Left");
                    if (isFacingRight)
                    {
                        Flip();
                    }
                    if (stamina > 0f)
                    {
                        playerAnimator.SetBool("isRunning", true);
                        transform.Translate((Vector3.left * Time.deltaTime) * 10f);
                    }

                    else
                    {
                        playerAnimator.SetBool("isWalking", true);
                        transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    }
                    isRunning = true;
                    StartCoroutine(StaminaLoss());
                }
            }
        }

        public void Flip()
        {
            isFacingRight = !isFacingRight;
            playerSprite.transform.Rotate(new Vector3(0, 180, 0));
        }

        public void CalmingHeartStart()
        {
            isCalmingHeart = true;
            StartCoroutine(CalmingHeart());
        }

        public void CalmingHeartStop()
        {
            isCalmingHeart = false;      
        }

        public void HeartFeedback()
        {
            if (heartBeat <= 110f)
            {
                heartFeedbackLevel1.SetActive(false);
                heartFeedbackLevel2.SetActive(false);
                heartFeedbackLevel3.SetActive(false);
            }

            if (heartBeat >= 110f)
            {
                heartFeedbackLevel1.SetActive(true);
                heartFeedbackLevel2.SetActive(false);
                heartFeedbackLevel3.SetActive(false);
            }

            if (heartBeat >= 120f)
            {
                heartFeedbackLevel1.SetActive(false);
                heartFeedbackLevel2.SetActive(true);
                heartFeedbackLevel3.SetActive(false);
            }

            if (heartBeat >= 130f)
            {
                heartFeedbackLevel1.SetActive(false);
                heartFeedbackLevel2.SetActive(false);
                heartFeedbackLevel3.SetActive(true);
            }
        }

        public IEnumerator StaminaLoss()
        {
            stamina -= 30f * Time.deltaTime;
            heartBeat += 2 * Time.deltaTime;
            hidingFactor += 2 * Time.deltaTime;
            heartbeatSpeed += 0.2f * Time.deltaTime;
            staminaBar.SetActive(true);

            yield return new WaitForSeconds(1f);
        }
        public IEnumerator CalmingHeart()
        {
            canMove = false;
            if (stamina > 0)
            {
                while (isCalmingHeart)
                {
                    //yield return new WaitForSeconds(1);
                   
                    stamina -= 10f * Time.deltaTime;
                    heartBeat -= 5f * Time.deltaTime;
                    hidingFactor -= 5 * Time.deltaTime;
                    heartbeatSpeed -= 0.5f * Time.deltaTime;
                    staminaBar.SetActive(true);
                    yield return null;
                }
            }
            canMove = true;          
        }

        IEnumerator Hiding()
        {
            //Hiding stuff
            gameObject.GetComponent<Collider2D>().enabled = false;
            //gameObject.transform.Find("Sprite").GetComponent<Collider2D>().enabled = false;
            yield return null;
        }

        IEnumerator StandingRegen()
        {          
            yield return new WaitForSeconds(3f);

            if (!isHiding && !usingLadder && !isCalmingHeart && stamina < 100f)
            {
                isRegening = true;
                stamina += 20f * Time.deltaTime;
                staminaBar.SetActive(true);
            }

            if (heartBeat > 100f && isRegening)
            {
                isRegening = true;
                heartBeat -= 0.25f * Time.deltaTime;
                hidingFactor -= 0.25f * Time.deltaTime;
                heartbeatSpeed -= 0.025f * Time.deltaTime;
            }
        }

        IEnumerator DownLadder()
        {
            canMove = false;
            usingLadder = true;
            playerAnimator.SetBool("isUsingLadder", true);
            transform.position = ladderTop;
            playerRb.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(2, 9, true);

            while (transform.position != ladderBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, ladderBottom, Time.deltaTime * 5f);
                yield return null;
            }

            playerRb.gravityScale = defaultGravity;
            Physics2D.IgnoreLayerCollision(2, 9, false);
            canMove = true;
            usingLadder = false;
        }
        IEnumerator UpLadder()
        {
            canMove = false;
            usingLadder = true;
            playerAnimator.SetBool("isUsingLadder", true);
            transform.position = ladderBottom;
            playerRb.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(2, 9, true);

            while (transform.position != ladderTop)
            {
                transform.position = Vector3.MoveTowards(transform.position, ladderTop, Time.deltaTime * 5f);
                yield return null;
            }
            
            playerRb.gravityScale = defaultGravity;
            Physics2D.IgnoreLayerCollision(2, 9, false);
            canMove = true;
            usingLadder = false;
            playerAnimator.SetBool("isUsingLadder", false);
        }

        IEnumerator OffLadder()
        {
            StopAllCoroutines();
            playerRb.gravityScale = defaultGravity;
            Physics2D.IgnoreLayerCollision(2, 9, false);
            canMove = true;
            usingLadder = false;
            playerAnimator.SetBool("isUsingLadder", false);
            yield return null;
        }

        IEnumerator StaminaBarDisappearance()
        {
            yield return new WaitForSeconds(3f);
            staminaBar.SetActive(false);
            yield break;
        }
    }
}
            
 


       
