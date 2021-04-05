using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{
    public class TriggerBigBoy : MonoBehaviour
    {
        PlayerController playerScript;
        BigBoyController bigBoyScript;

        Transform target;

        public int detectRadius = 10;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            bigBoyScript = FindObjectOfType<BigBoyController>();
        }

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if(distance <= detectRadius && bigBoyScript.bBcanMove && !playerScript.isGrabbed && !playerScript.isHiding)
            {
                bigBoyScript.TriggerPoursuite();
            }

            if(distance >= detectRadius)
            {
                bigBoyScript.playerInVision = false;
                bigBoyScript.RunningOutsideCamera();
            }
        }

        /*public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                bigBoyScript.TriggerPoursuite();
            }
        }*/

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}

