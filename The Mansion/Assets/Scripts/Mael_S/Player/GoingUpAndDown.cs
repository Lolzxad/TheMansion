using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{

    public class GoingUpAndDown : MonoBehaviour
    {
        GameObject player;
        public Transform centerDownLadder;
        public Transform centerUpLadder;

        float speed = 2f;

        bool isMovingToLadder;
        bool isMovingUp;

        Rigidbody2D rb;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            rb = player.GetComponent<Rigidbody2D>();
        }

        public void GoingUp()
        {


            //amène le joueur au centre du ladders
            isMovingToLadder = true;
            //le monte à l'étage
        }

        private void Update()
        {
            if (isMovingToLadder)
            {
                float step = speed * Time.deltaTime;
                player.transform.position = Vector3.MoveTowards(player.transform.position, centerDownLadder.position, step);
            }

            if (isMovingUp)
            {
                float step = speed * Time.deltaTime;

                player.GetComponent<Rigidbody2D>().gravityScale = 0;

                player.transform.position = Vector3.MoveTowards(player.transform.position, centerUpLadder.position, step);


            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "PlayerTrigger" && isMovingToLadder)
            {
                isMovingUp = true;
                isMovingToLadder = false;
            }
        }
    }
}
