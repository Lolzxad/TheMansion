using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class CrawlerController : MonoBehaviour
    {
        BigBoyController bbScript;
        PlayerController playerScript;

        Animator animator;
        Transform target;

        public int detectRadius = 10;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            bbScript = FindObjectOfType<BigBoyController>();
        }

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            
        }

        private void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= detectRadius && playerScript.isRunning )
            {
                StartCoroutine(CrawlerIsScreaming());
                Debug.Log("IS SCREAMING");                                                 
            }

    
        }

        IEnumerator CrawlerIsScreaming()
        {
            animator.SetBool("isScreaming", true);
            bbScript.isCalled = true;
            playerScript.heartBeat = playerScript.heartBeat + 10f;
            playerScript.hidingFactor = playerScript.hidingFactor + 10f;

            yield return new WaitForSeconds(4);

            animator.SetBool("isScreaming", false);
        }
      

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}

