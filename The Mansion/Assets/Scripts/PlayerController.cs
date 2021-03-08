using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
public class PlayerController : MonoBehaviour
    {
        public int stamina = 1000;
        public float heartBeat = 1000f;

        private bool isMoving;
        private bool canInteract;
        public bool isHiding;
        public bool isGrabbed;
        
        public Transform BasePosition;
        public Transform WalkRight;
        public Transform WalkLeft;
        public Transform RunRight;
        public Transform RunLeft;

        TouchPhase touchPhase = TouchPhase.Ended;

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(stamina);

            if (Input.touchCount > 0 /*|| Input.GetMouseButton(0)*/)
            {
                
                Touch touch = Input.GetTouch(0);

                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);
                //Vector3 clickPosition2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Input.GetTouch(0).phase == touchPhase /*|| Input.GetMouseButtonDown(0)*/)
                {   
                    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosition2D, Camera.main.transform.forward);
                    //RaycastHit2D clickInformation = Physics2D.Raycast(clickPosition2D, Camera.main.transform.forward);

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
                                Debug.Log("Is Hiding");
                                transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }                                         
                    }

                    /*if (clickInformation.collider != null)
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
                                gameObject.GetComponent<Collider2D>().enabled = true;
                            }
                        }
                        else
                        {
                            if (touchedObject.tag == "Hard Hiding Spot" && canInteract && !isGrabbed)
                            {
                                isHiding = true;
                                Debug.Log("Is Hiding");
                                transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }
                    }*/
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

                /*if (clickPosition2D.x > BasePosition.position.x && clickPosition2D.x <= WalkRight.position.x && !isHiding && !isGrabbed)
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
                }*/
            }
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


        IEnumerator StaminaLoss()
        {
            stamina--;
            heartBeat++;
            yield return new WaitForSeconds(1f);
        }

        IEnumerator Hiding()
        {
            //Hiding stuff
            gameObject.GetComponent<Collider2D>().enabled = false;
            yield return null;
        }
    }
}