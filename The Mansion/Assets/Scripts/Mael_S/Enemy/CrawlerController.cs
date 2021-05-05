using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class CrawlerController : MonoBehaviour
    {
        BigBoyController bbScript;
        PlayerController playerScript;

        Animation anim;
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
            anim = gameObject.GetComponent<Animation>();

            
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
            anim.Play("CrawlerScream");
            bbScript.isCalled = true;

            yield return new WaitForSeconds(4);

            anim.Stop("CrawlerScream");
        }
      

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}

