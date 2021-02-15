using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                if (touchPosition.x > 0)
                {
                    Debug.Log("Right");
                    transform.Translate((Vector3.right * Time.deltaTime) * 1f);
                }

                if (touchPosition.x < 0)
                {
                    Debug.Log("Left");
                    transform.Translate((Vector3.left * Time.deltaTime) * 1f);
                }

                if (touchPosition.x >= 10 || touchPosition.x <= -10)
                {
                    Debug.Log("END");
                }
            }
        }
    }
}