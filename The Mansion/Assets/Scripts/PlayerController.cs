using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class PlayerController : MonoBehaviour
    {
        public float stamina = 100f;
        public float heartBeat = 100f;

        private bool isMoving;
        private bool canInteract;
        public bool isMouse;
        public bool isHiding;
        public bool isGrabbed;

        public Transform BasePosition;
        public Transform WalkRight;
        public Transform WalkLeft;
        public Transform RunRight;
        public Transform RunLeft;

        public GameObject playerSprite;
        public Vector3 baseSpritePosition;
        public Vector3 basePosition;

        TouchPhase touchPhase = TouchPhase.Ended;

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
                                if (touchedObject.tag == "Hard Hiding Spot" && canInteract && !isGrabbed)
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
                        isMoving = true;
                        transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x < BasePosition.position.x && touchPosition.x >= WalkLeft.position.x && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Walk Left");
                        isMoving = true;
                        transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    }

                    if (touchPosition.x > WalkRight.position.x && touchPosition.x <= RunRight.position.x && stamina > 0 && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Run Right");
                        isMoving = true;
                        transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                        StartCoroutine(StaminaLoss());
                    }

                    if (touchPosition.x < WalkLeft.position.x && touchPosition.x >= RunLeft.position.x && stamina > 0 && !isHiding && !isGrabbed)
                    {
                        Debug.Log("Run Left");
                        isMoving = true;
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
            }
            transform.hasChanged = false;


        }
        void OnTriggerEnter2D(Collider2D InteractableObject)
        {
            if (InteractableObject.tag == "Hard Hiding Spot")
            {
                Debug.Log("Can hide");
                canInteract = true;
            }
        }
        void OnTriggerExit2D(Collider2D InteractableObject)
        {
            canInteract = false;
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
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 3;
                                gameObject.GetComponent<Collider2D>().enabled = true;
                                transform.position = basePosition;
                                playerSprite.transform.position = baseSpritePosition;
                            }
                        }
                        else
                        {
                            if (touchedObject.tag == "Hard Hiding Spot" && canInteract && !isGrabbed)
                            {
                                isHiding = true;
                                playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                Debug.Log("Is Hiding");
                                basePosition = transform.position;
                                baseSpritePosition = playerSprite.transform.position;
                                transform.position = touchedObject.transform.position;
                                playerSprite.transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }
                    }
                }

                if (clickPosition2D.x > BasePosition.position.x && clickPosition2D.x <= WalkRight.position.x && !isHiding && !isGrabbed)
                {
                    Debug.Log("Walk Right");
                    isMoving = true;
                    transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                }

                if (clickPosition2D.x < BasePosition.position.x && clickPosition2D.x >= WalkLeft.position.x && !isHiding && !isGrabbed)
                {
                    Debug.Log("Walk Left");
                    isMoving = true;
                    transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                }

                if (clickPosition2D.x > WalkRight.position.x && clickPosition2D.x <= RunRight.position.x && stamina > 0 && !isHiding && !isGrabbed)
                {
                    Debug.Log("Run Right");
                    isMoving = true;
                    transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                    StartCoroutine(StaminaLoss());
                }

                if (clickPosition2D.x < WalkLeft.position.x && clickPosition2D.x >= RunLeft.position.x && stamina > 0 && !isHiding && !isGrabbed)
                {
                    Debug.Log("Run Left");
                    isMoving = true;
                    transform.Translate((Vector3.left * Time.deltaTime) * 10f);
                    StartCoroutine(StaminaLoss());
                }
            }
        }


        IEnumerator StaminaLoss()
        {
            stamina = stamina - 0.1f;
            heartBeat = heartBeat + 1 * Time.deltaTime;
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
                heartBeat = heartBeat - 0.5f * Time.deltaTime;
            }
        }
    }
}
            
 


       
