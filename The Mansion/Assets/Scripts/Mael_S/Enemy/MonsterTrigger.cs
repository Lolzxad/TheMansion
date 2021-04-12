using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{

    public class MonsterTrigger : MonoBehaviour
    {
        BigBoyController bbController;

        [SerializeField] GameObject warningMonster;

        private void Start()
        {
            bbController = FindObjectOfType<BigBoyController>();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "PlayerTrigger" && bbController.isPatrolling)
            {
                warningMonster.SetActive(true);
                Debug.Log("WARNING");
            }
        }
    }
}
