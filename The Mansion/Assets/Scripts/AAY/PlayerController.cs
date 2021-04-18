using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TheMansion
{
    public class PlayerController : MonoBehaviour
    {
        public Animator playerAnimator;

        public float stamina = 100f;
        public float heartBeat = 100f;
        public float hidingFactor = 1f;
        private float defaultGravity;

        private bool canHide;
        private bool canUseLadder;
        public bool canMove = true;
        public bool isMouse;
        public bool isHiding;
        public bool isGrabbed;
        public bool usingLadder;

        public Transform BasePosition;
        public Transform WalkRight;
        public Transform WalkLeft;
        public Transform RunRight;
        public Transform RunLeft;

        public GameObject playerSprite;
        public Rigidbody2D playerRb;
        public Vector3 baseSpritePosition;
        public Vector3 basePosition;
        public Vector3 ladderTop;
        public Vector3 ladderBottom;

        TouchPhase touchPhase = TouchPhase.Ended;

        private void Awake()
        {
            playerRb = GetComponent<Rigidbody2D>();
            defaultGravity = playerRb.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(stamina);            

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
                                if (touchedObject.tag == "Hard Hiding Spot")
                                {
                                    isHiding = false;
                                    gameObject.GetComponent<Collider2D>().enabled = true;
                                }
                            }
                            else
                            {
                                if (touchedObject.tag == "Hard Hiding Spot" && canHide && !isGrabbed)
                                {
                                    isHiding = true;
                                    playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                    Debug.Log("Is Hiding");
                                    transform.position = touchedObject.transform.position;
                                    StartCoroutine(Hiding());
                                }
                            }
                        }
                    }

                    if (touchPosition.x > BasePosition.position.x && touchPosition.x <= WalkRight.position.x && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Walk Right");
                        transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x < BasePosition.position.x && touchPosition.x >= WalkLeft.position.x && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Walk Left");
                        transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x > WalkRight.position.x && touchPosition.x <= RunRight.position.x && stamina > 0 && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Run Right");
                        transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                        StartCoroutine(StaminaLoss());
                    }

                    if (touchPosition.x < WalkLeft.position.x && touchPosition.x >= RunLeft.position.x && stamina > 0 && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Run Left");
                        transform.Translate((Vector3.left * Time.deltaTime) * 10f);
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
                StartCoroutine(StandingRegen());
                playerAnimator.SetBool("isWalkingRight", false);
                playerAnimator.SetBool("isRunningRight", false);
                playerAnimator.SetBool("isWalkingLeft", false);
                playerAnimator.SetBool("isRunningLeft", false);
            }
            transform.hasChanged = false;

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


        }
        void OnTriggerEnter2D(Collider2D InteractableObject)
        {
            if (InteractableObject.tag == "Hard Hiding Spot")
            {
                if (isHiding)
                {
                    InteractableObject.GetComponent<OutlineActivator>().DisableOutline();
                }
                else
                {
                    canHide = true;
                    InteractableObject.GetComponent<OutlineActivator>().EnableOutline();
                }                              
            }

            if (InteractableObject.tag == "LadderDown" || InteractableObject.tag == "LadderUp")
            {
                if (usingLadder)
                {
                    InteractableObject.transform.parent.GetComponent<OutlineActivator>().DisableOutline();
                }
                else
                {
                    canUseLadder = true;
                    InteractableObject.transform.parent.GetComponent<OutlineActivator>().EnableOutline();
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
                            if (touchedObject.tag == "Hard Hiding Spot")
                            {
                                isHiding = false;
                                canMove = true;
                                playerAnimator.SetBool("isHiding", false);
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 3;
                                playerRb.gravityScale = defaultGravity;
                                gameObject.GetComponent<Collider2D>().enabled = true;
                                transform.position = basePosition;
                                playerSprite.transform.position = baseSpritePosition;
                            }
                        }
                        else
                        {
                            if (touchedObject.tag == "Hard Hiding Spot" && canHide && !isGrabbed)
                            {
                                isHiding = true;
                                canMove = false;
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                playerRb.gravityScale = 0;
                                playerAnimator.SetTrigger("playerHides");
                                playerAnimator.SetBool("isHiding", true);
                                Debug.Log("Is Hiding");
                                basePosition = transform.position;
                                baseSpritePosition = playerSprite.transform.position;
                                transform.position = touchedObject.transform.position;
                                playerSprite.transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }

                        if (touchedObject.tag == "LadderDown" && canUseLadder)
                        {
                            ladderBottom = touchedObject.transform.parent.gameObject.transform.GetChild(0).transform.position;
                            ladderTop = touchedObject.transform.parent.gameObject.transform.GetChild(1).transform.position;                                                     
                            StartCoroutine(DownLadder());                          
                        }

                        if (touchedObject.tag == "LadderUp" && canUseLadder)
                        {
                            ladderBottom = touchedObject.transform.parent.gameObject.transform.GetChild(0).transform.position;
                            ladderTop = touchedObject.transform.parent.gameObject.transform.GetChild(1).transform.position;
                            StartCoroutine(UpLadder());
                        }
                    }
                }

                if (clickPosition2D.x > BasePosition.position.x && clickPosition2D.x <= WalkRight.position.x && canMove)
                {
                    Debug.Log("Walk Right");
                    playerAnimator.SetBool("isLookingRight", true);
                    playerAnimator.SetBool("isLookingLeft", false);
                    playerAnimator.SetBool("isWalkingRight", true);
                    playerAnimator.SetBool("isRunningRight", false);
                    transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                }

                if (clickPosition2D.x < BasePosition.position.x && clickPosition2D.x >= WalkLeft.position.x && canMove)
                {
                    Debug.Log("Walk Left");
                    playerAnimator.SetBool("isLookingLeft", true);
                    playerAnimator.SetBool("isLookingRight", false);
                    playerAnimator.SetBool("isWalkingLeft", true);
                    playerAnimator.SetBool("isRunningLeft", false);
                    transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                }

                if (clickPosition2D.x > WalkRight.position.x && clickPosition2D.x <= RunRight.position.x && stamina > 0 && canMove)
                {
                    Debug.Log("Run Right");
                    playerAnimator.SetBool("isLookingRight", true);
                    playerAnimator.SetBool("isLookingLeft", false);
                    playerAnimator.SetBool("isRunningRight", true);
                    playerAnimator.SetBool("isWalkingRight", false);
                    transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                    StartCoroutine(StaminaLoss());
                }

                if (clickPosition2D.x < WalkLeft.position.x && clickPosition2D.x >= RunLeft.position.x && stamina > 0 && canMove)
                {
                    Debug.Log("Run Left");
                    playerAnimator.SetBool("isLookingLeft", true);
                    playerAnimator.SetBool("isLookingRight", false);
                    playerAnimator.SetBool("isRunningLeft", true);
                    playerAnimator.SetBool("isWalkingLeft", false);
                    transform.Translate((Vector3.left * Time.deltaTime) * 10f);
                    StartCoroutine(StaminaLoss());
                }
            }
        }

        public void CalmingHeart()
        {
            if (stamina > 0 && isHiding)
            {
                stamina = stamina - 10f;
                heartBeat = heartBeat - 5f;
                hidingFactor = hidingFactor - 5;
            }           
        }

        public IEnumerator StaminaLoss()
        {
            stamina = stamina - 0.1f;
            heartBeat = heartBeat + 2 * Time.deltaTime;
            hidingFactor = hidingFactor + 2 * Time.deltaTime;
            yield return new WaitForSeconds(1f);
        }

        IEnumerator Hiding()
        {
            //Hiding stuff
            gameObject.GetComponent<Collider2D>().enabled = false;
            yield return null;
        }

        IEnumerator StandingRegen()
        {
            yield return new WaitForSeconds(5f);

            if (stamina < 100f)
            {
                stamina = stamina + 0.05f;
            }

            if (heartBeat > 100f)
            {
                heartBeat = heartBeat - 0.25f * Time.deltaTime;
                hidingFactor = hidingFactor - 0.25f * Time.deltaTime;
            }
        }

        IEnumerator DownLadder()
        {
            canMove = false;
            usingLadder = true;
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
        }
    }
}
            
 


       
