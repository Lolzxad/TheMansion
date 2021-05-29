using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class CrawlerController : MonoBehaviour
    {
        BigBoyController bbScript;
        PlayerController playerScript;
        AudioManagerVEVO audioManager;

        Animator animator;
        Transform target;
        VictoryManager victoryManager;

        private bool isScreaming;

        public int detectRadius = 10;

        bool canPlayMusic;

        private void Awake()
        {
            playerScript = FindObjectOfType<PlayerController>();
            bbScript = FindObjectOfType<BigBoyController>();
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            victoryManager = FindObjectOfType<VictoryManager>();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            canPlayMusic = true;           
        }

        private void Update()
        {
            float distance = Vector2.Distance(target.position, transform.position);

            if (distance <= detectRadius && playerScript.playerAnimator.GetBool("isRunning") && !isScreaming)
            {
                StartCoroutine(CrawlerIsScreaming());
                Debug.Log("IS SCREAMING");
                animator.SetBool("isScreaming", true);
            }   
        }

        IEnumerator CrawlerIsScreaming()
        {
            isScreaming = true;

            if (victoryManager.isLevel3 && canPlayMusic)
            {
                audioManager.PlayAudio(AudioType.Phonograph);
                canPlayMusic = false;
            }

            if (victoryManager.isLevel5 && canPlayMusic)
            {
                audioManager.PlayAudio(AudioType.Phonograph);
                canPlayMusic = false;
            }

            
            bbScript.isCalled = true;
            playerScript.heartBeat += 10f;
            playerScript.hidingFactor += 10f;
            playerScript.heartbeatSpeed += 1f;
            playerScript.heartOpacity += 0.5f;

            yield return new WaitForSeconds(8);

            canPlayMusic = true;
            isScreaming = false;
            animator.SetBool("isScreaming", false);
        }
      

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}

