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
        AudioManagerVEVO audioManager;

        public Animator playerAnimator;
        public Animator heartAnimator;

        public float stamina = 100f;
        public float heartBeat = 100f;
        public float hidingFactor = 1f;
        public float defaultGravity;
        public float heartOpacity = 0f;
        public float heartbeatSpeed = 0.1f;
        private float lastStamina = 100f;

        private bool canHide;
        private bool canUseLadder;
        public bool isGrounded;
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
        public GameObject heartFeedback;
        public GameObject feet;
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
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            playerRb = GetComponent<Rigidbody2D>();
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            defaultGravity = playerRb.gravityScale;
            Physics2D.IgnoreLayerCollision(2, 9, false);
        }

        // Update is called once per frame
        void Update()
        {

            heartFeedback.GetComponent<Image>().color = new Color(1f, 1f, 1f, heartOpacity);

            if (isGrabbed)
            {
                ProCamera2D.Instance.CenterOnTargets();
            }

            if (usingLadder)
            {
                ProCamera2D.Instance.CenterOnTargets();
            }

            if (!isGrounded)
            {
                canUseLadder = false;
            }

            //Debug.Log(stamina);
            if (heartbeatSpeed <= 1)
            {
                heartAnimator.SetFloat("speed", 1 + heartbeatSpeed);
            }

            if (heartOpacity > 1f)
            {
                heartOpacity = 1f;
            }

            staminaBarScript.SetStamina(stamina);

            if (lastStamina == stamina && staminaBar.activeSelf && !isRunning && !isCalmingHeart && !isRegening)
            {
                StartCoroutine(StaminaBarDisappearance());
            }
            lastStamina = stamina;

            //HeartFeedback();

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
                                    audioManager.PlayAudio(AudioType.Player_Hide);
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




                                if (!usingLadder && !isHiding && !isGrabbed && canUseLadder)
                                {
                                    StartCoroutine(DownLadder());
                                }
                                else
                                {
                                    if (usingLadder)
                                    {
                                        StartCoroutine(OffLadder());
                                    }
                                }
                            }

                            if (touchedObject.tag == "LadderUp")
                            {
                                ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                                ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;



                                if (!usingLadder && !isHiding && !isGrabbed && canUseLadder)
                                {
                                    StartCoroutine(UpLadder());
                                }
                                else
                                {
                                    if (usingLadder)
                                    {
                                        StartCoroutine(OffLadder());
                                    }
                                }
                            }

                            if (touchedObject.tag == "StoryLore")
                            {
                                if (touchedObject.name == "Story 1")
                                {

                                    //MenuManagerScript.story1Get = true;
                                    PlayerPrefs.SetInt("Story1", (MenuManagerScript.story1Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                    Debug.Log("STORY 1");

                                }

                                if (touchedObject.name == "Story 2")
                                {
                                    //MenuManagerScript.story2Get = true;
                                    PlayerPrefs.SetInt("Story2", (MenuManagerScript.story2Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 3")
                                {
                                    PlayerPrefs.SetInt("Story3", (MenuManagerScript.story3Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    MenuManagerScript.story3Get = true;
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 4")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story4", (MenuManagerScript.story4Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 5")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story5", (MenuManagerScript.story5Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 6")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story6", (MenuManagerScript.story6Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 7")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story7", (MenuManagerScript.story7Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                    touchedObject.SetActive(false);
                                }

                                if (touchedObject.name == "Story 8")
                                {
                                    //MenuManagerScript.story4Get = true;
                                    PlayerPrefs.SetInt("Story8", (MenuManagerScript.story8Get ? 1 : 0));
                                    audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
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
                        transform.Translate((Vector2.right * Time.deltaTime) * 5f);
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
                        transform.Translate((Vector2.left * Time.deltaTime) * 5f);
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
                            transform.Translate((Vector2.right * Time.deltaTime) * 10f);
                        }

                        else
                        {
                            playerAnimator.SetBool("isWalking", true);
                            transform.Translate((Vector2.right * Time.deltaTime) * 5f);
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
                            transform.Translate((Vector2.left * Time.deltaTime) * 10f);
                        }

                        else
                        {
                            playerAnimator.SetBool("isWalking", true);
                            transform.Translate((Vector2.left * Time.deltaTime) * 5f);
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
                isRunning = false;
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("isRunning", false);
            }
            transform.hasChanged = false;

            if (!isRunning && !isGrabbed && !usingLadder)
            {
                StartCoroutine("StandingRegen");
            }
            else
            if (isRunning || isGrabbed || usingLadder)
            {
                StopCoroutine("StandingRegen");
                isRegening = false;
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

            if (heartbeatSpeed > 1)
            {
                heartbeatSpeed = 1;
            }
        }

        private void OnCollisionEnter2D(Collision2D ground)
        {
            if (ground.collider.tag == "Ground")
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D ground)
        {
            if (ground.collider.tag == "Ground")
            {
                isGrounded = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Story 1")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story1", (MenuManagerScript.story1Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 1");

            }

            if (other.gameObject.name == "Story 2")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story2", (MenuManagerScript.story2Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 2");

            }

            if (other.gameObject.name == "Story 3")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story3", (MenuManagerScript.story3Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 3");

            }

            if (other.gameObject.name == "Story 4")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story4", (MenuManagerScript.story4Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 4");

            }

            if (other.gameObject.name == "Story 5")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story5", (MenuManagerScript.story5Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 5");

            }

            if (other.gameObject.name == "Story 6")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story6", (MenuManagerScript.story6Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 6");

            }

            if (other.gameObject.name == "Story 7")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story7", (MenuManagerScript.story7Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 7");

            }

            if (other.gameObject.name == "Story 8")
            {

                //MenuManagerScript.story1Get = true;
                PlayerPrefs.SetInt("Story8", (MenuManagerScript.story8Get ? 1 : 0));
                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                other.gameObject.SetActive(false);
                Debug.Log("STORY 8");

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
                    if (isGrounded)
                    {
                        canUseLadder = true;
                        InteractableObject.transform.parent.GetComponent<OutlineActivator>().EnableOutline();
                        InteractableObject.transform.parent.gameObject.transform.Find("UseLadderDown").gameObject.SetActive(true);
                    }                   
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
                //audioManager.StopAudio(AudioType.Player_Ladder);
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

                            if (!usingLadder && !isHiding && !isGrabbed && canUseLadder)
                            {
                                StartCoroutine(DownLadder());
                            }
                            else
                            {
                                if (usingLadder)
                                {
                                    StartCoroutine(OffLadder());
                                }
                            }
                        }

                        if (touchedObject.tag == "LadderUp")
                        {
                            ladderBottom = touchedObject.transform.parent.gameObject.transform.Find("BotLadder").transform.position;
                            ladderTop = touchedObject.transform.parent.gameObject.transform.Find("TopLadder").transform.position;

                            if (!usingLadder && !isHiding && !isGrabbed && canUseLadder)
                            {
                                StartCoroutine(UpLadder());
                            }
                            else
                            {
                                if (usingLadder)
                                {
                                    StartCoroutine(OffLadder());
                                }
                            }
                        }

                        if (touchedObject.tag == "StoryLore")
                        {
                            if (touchedObject.name == "Story 1")
                            {

                                //MenuManagerScript.story1Get = true;
                                PlayerPrefs.SetInt("Story1", (MenuManagerScript.story1Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                                Debug.Log("STORY 1");

                            }

                            if (touchedObject.name == "Story 2")
                            {
                                //MenuManagerScript.story2Get = true;
                                PlayerPrefs.SetInt("Story2", (MenuManagerScript.story2Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 3")
                            {
                                PlayerPrefs.SetInt("Story3", (MenuManagerScript.story3Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                MenuManagerScript.story3Get = true;
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 4")
                            {
                                //MenuManagerScript.story4Get = true;
                                PlayerPrefs.SetInt("Story4", (MenuManagerScript.story4Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 5")
                            {
                                //MenuManagerScript.story4Get = true;
                                PlayerPrefs.SetInt("Story5", (MenuManagerScript.story5Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 6")
                            {
                                //MenuManagerScript.story4Get = true;
                                PlayerPrefs.SetInt("Story6", (MenuManagerScript.story6Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 7")
                            {
                                //MenuManagerScript.story4Get = true;
                                PlayerPrefs.SetInt("Story7", (MenuManagerScript.story7Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }

                            if (touchedObject.name == "Story 8")
                            {
                                //MenuManagerScript.story4Get = true;
                                PlayerPrefs.SetInt("Story8", (MenuManagerScript.story8Get ? 1 : 0));
                                audioManager.PlayAudio(AudioType.Recup_Narra_SFX);
                                touchedObject.SetActive(false);
                            }
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
                    transform.Translate((Vector2.right * Time.deltaTime) * 5f);
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
                    transform.Translate((Vector2.left * Time.deltaTime) * 5f);
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
                        transform.Translate((Vector2.right * Time.deltaTime) * 10f);
                    }
                    else
                    {
                        playerAnimator.SetBool("isWalking", true);
                        transform.Translate((Vector2.right * Time.deltaTime) * 5f);
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
                        transform.Translate((Vector2.left * Time.deltaTime) * 10f);
                    }

                    else
                    {
                        playerAnimator.SetBool("isWalking", true);
                        transform.Translate((Vector2.left * Time.deltaTime) * 5f);
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
            if (!isGrabbed && !usingLadder)
            {
                isCalmingHeart = true;
                StartCoroutine(CalmingHeart());
            }           
        }

        public void CalmingHeartStop()
        {           
            if (!isHiding && !isGrabbed)
            {
                canMove = true;                
            }
            isCalmingHeart = false;
        }

        /*public void HeartFeedback()
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
        }*/

        public IEnumerator StaminaLoss()
        {
            stamina -= 15f * Time.deltaTime;
            heartBeat += 1 * Time.deltaTime;
            hidingFactor += 1 * Time.deltaTime;
            heartbeatSpeed += 0.1f * Time.deltaTime;
            heartOpacity += 0.05f * Time.deltaTime;
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
                    hidingFactor -= 2.5f * Time.deltaTime;
                    heartbeatSpeed -= 0.5f * Time.deltaTime;
                    heartOpacity -= 0.25f * Time.deltaTime;
                    staminaBar.SetActive(true);
                    yield return null;
                }
            }
      
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

            if (!usingLadder && !isCalmingHeart && stamina < 100f)
            {
                isRegening = true;
                stamina += 20f * Time.deltaTime;
                staminaBar.SetActive(true);
            }

            if (heartBeat > 100f && isRegening)
            {
                isRegening = true;
                heartBeat -= 1f * Time.deltaTime;
                hidingFactor -= 1f * Time.deltaTime;
                heartbeatSpeed -= 0.1f * Time.deltaTime;
                heartOpacity -= 0.05f * Time.deltaTime;
            }
        }

        IEnumerator DownLadder()
        {
            audioManager.PlayAudio(AudioType.Player_Ladder);
            canMove = false;
            usingLadder = true;
            playerAnimator.SetBool("isUsingLadder", true);
            transform.position = ladderTop;
            playerRb.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(2, 9, true);

            while (transform.position.y != ladderBottom.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, ladderBottom.y), Time.deltaTime * 5f);
                yield return null;
            }

            playerRb.gravityScale = defaultGravity;
            Physics2D.IgnoreLayerCollision(2, 9, false);
            canMove = true;
            usingLadder = false;
        }
        IEnumerator UpLadder()
        {
            audioManager.PlayAudio(AudioType.Player_Ladder);
            canMove = false;
            usingLadder = true;
            playerAnimator.SetBool("isUsingLadder", true);
            transform.position = ladderBottom;
            playerRb.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(2, 9, true);

            while (transform.position.y != ladderTop.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, ladderTop.y), Time.deltaTime * 5f);
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
            
 


       
