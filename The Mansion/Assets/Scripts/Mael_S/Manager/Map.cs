using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{
    public class Map : MonoBehaviour
    {
        public GameObject map;
        public GameObject mapButton;


        public Transform playerPos;
        GameObject player;
        PlayerController playerController;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            player = GameObject.FindGameObjectWithTag("Player");
            
        }




        public void ActivateMap()
        {
            Time.timeScale = 0;
            map.SetActive(true);
            playerPos = player.transform;
            mapButton.SetActive(false);

            
        }

        public void ExitMap()
        {
            mapButton.SetActive(true);
            map.SetActive(false);
            Time.timeScale = 1;
        }
    }
}

