using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMansion
{
    public class HeartAnimation : MonoBehaviour
    {
        Animator anim;

        PlayerController player;

        private void Start()
        {
            anim = gameObject.GetComponent<Animator>();

            player = FindObjectOfType<PlayerController>();
        }

        public void Update()
        {
            if(player.heartBeat <= 120)
            {
                //default
                anim.speed = 1;
            }

            if(player.heartBeat >= 120)
            {
                anim.speed = 2;
            }

            if(player.heartBeat >= 140)
            {
                anim.speed = 3;
            }
        }
    }
}

