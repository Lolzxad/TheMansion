using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerTuto : MonoBehaviour
{
    public GameObject crawlerTexte;

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            crawlerTexte.SetActive(true);
        }


    }


}
