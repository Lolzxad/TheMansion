using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{
    public class CrawlerTuto : MonoBehaviour
    {
        public GameObject crawlerTexte;
        public GameObject injonctionNTexte;

        [SerializeField] bool isForCrawler;



        public void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.tag == "Player")
            {
                if (isForCrawler)
                {
                    crawlerTexte.SetActive(true);
                }
                else
                {
                    injonctionNTexte.SetActive(true);
                }

            }


        }


    }
}

