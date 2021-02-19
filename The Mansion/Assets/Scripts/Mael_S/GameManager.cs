using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class GameManager : MonoBehaviour
    {
        GameObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                Debug.Log("Level complete!");
            }
        }
    }
}
