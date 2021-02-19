using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
public class PlayerController : MonoBehaviour
    {
        private int stamina = 500;

        private bool isMoving;
        private bool canInteract;
        private bool isHiding;

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
                            if (touchedObject.tag == "Hard Hiding Spot" && canInteract)
                            {
                                isHiding = true;
                                Debug.Log("Is Hiding");
                                transform.position = touchedObject.transform.position;
                                StartCoroutine(Hiding());
                            }
                        }                                         
                    }
                }

                if (touchPosition.x > BasePosition.position.x && touchPosition.x <= WalkRight.position.x && !isHiding)
                {
                    Debug.Log("Walk Right");
                    isMoving = true;
                    transform.Translate((Vector3.right * Time.deltaTime) * 1f);
                }

                if (touchPosition.x < BasePosition.position.x && touchPosition.x >= WalkLeft.position.x && !isHiding)
                {
                    Debug.Log("Walk Left");
                    isMoving = true;
                    transform.Translate((Vector3.left * Time.deltaTime) * 1f);
                }

                if (touchPosition.x > WalkRight.position.x && touchPosition.x <= RunRight.position.x && stamina > 0 && !isHiding) 
                {
                    Debug.Log("Run Right");
                    isMoving = true;
                    transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    StartCoroutine(StaminaLoss());
                }

                if (touchPosition.x < WalkLeft.position.x && touchPosition.x >= RunLeft.position.x && stamina > 0 && !isHiding)
                {
                    Debug.Log("Run Left");
                    isMoving = true;
                    transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    StartCoroutine(StaminaLoss());                   
                }
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