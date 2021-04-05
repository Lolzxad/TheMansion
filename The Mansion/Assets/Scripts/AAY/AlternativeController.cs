using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace TheMansion
{
    public class AlternativeController : Button
    {
        PlayerController playerScript;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
        }

        public void Update()
        {
            if (IsPressed())
            {
                WhilePressed();
            }
        }

        public void WhilePressed()
        {
            if (this.gameObject.name == "Walk Right")
            {
                playerScript.transform.Translate((Vector3.right * Time.deltaTime) * 5f);
            }

            if (this.gameObject.name == "Walk Left")
            {
                playerScript.transform.Translate((Vector3.left * Time.deltaTime) * 5f);
            }

            if (this.gameObject.name == "Run Right")
            {
                playerScript.transform.Translate((Vector3.right * Time.deltaTime) * 10f);
                playerScript.StartCoroutine(playerScript.StaminaLoss());
                
            }

            if (this.gameObject.name == "Run Left")
            {
                playerScript.transform.Translate((Vector3.left * Time.deltaTime) * 10f);
                playerScript.StartCoroutine(playerScript.StaminaLoss());
            }
        }
    }
}

