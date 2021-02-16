using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
public class PlayerController : MonoBehaviour
    {
        private float stamina = 100f;
        public Transform BasePosition;
        public Transform WalkRight;
        public Transform WalkLeft;
        public Transform RunRight;
        public Transform RunLeft;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(stamina);

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                if (touchPosition.x > BasePosition.position.x && touchPosition.x <= WalkRight.position.x)
                {
                    Debug.Log("Walk Right");
                    transform.Translate((Vector3.right * Time.deltaTime) * 1f);
                    Camera.main.transform.Translate((Vector3.right * Time.deltaTime) * 1f);
                }

                if (touchPosition.x < BasePosition.position.x && touchPosition.x >= WalkLeft.position.x)
                {
                    Debug.Log("Walk Left");
                    transform.Translate((Vector3.left * Time.deltaTime) * 1f);
                    Camera.main.transform.Translate((Vector3.left * Time.deltaTime) * 1f);
                }

                if (touchPosition.x > WalkRight.position.x && touchPosition.x <= RunRight.position.x)
                {
                    Debug.Log("Run Right");
                    transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    Camera.main.transform.Translate((Vector3.right * Time.deltaTime) * 5f);
                    stamina = stamina - 1 * Time.deltaTime;
                }

                if (touchPosition.x < WalkLeft.position.x && touchPosition.x >= RunLeft.position.x)
                {
                    Debug.Log("Run Left");
                    transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    Camera.main.transform.Translate((Vector3.left * Time.deltaTime) * 5f);
                    stamina = stamina - 1 * Time.deltaTime;
                }
            }
        }
    }
}