using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class TriggerTutoText : MonoBehaviour
    {
        [SerializeField] bool isDoor;

        

        TutoManager tuto;

        private void Start()
        {
            tuto = FindObjectOfType<TutoManager>();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                if (isDoor)
                {
                    Debug.Log("Trigger porte fermee");
                    tuto.DoorLocked();
                }
                else
                {
                    Debug.Log("Trigger hide ready");
                    //tuto.player.GetComponent<PlayerController>().enabled = false;
                    tuto.ReadyToHide();
                }
            }
            
        }
    }
}

