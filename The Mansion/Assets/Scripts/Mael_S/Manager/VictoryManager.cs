using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class VictoryManager : MonoBehaviour
    {
        GameObject player;
        
        public bool canWin;

        public bool sideObj1;
        public bool sideObj2;

        public bool isCDV;
        public static bool bigWin;

        #region Levels
        public bool isLevel1;
        public bool isLevel2;
        public bool isLevel3;
        public bool isLevel4;
        public bool isLevel5;
        public bool isLevel6;
        public bool isLevel7;

        #endregion

        public GameObject obj;
        public GameObject menuWin;
        public GameObject keyTMP;
        public GameObject keyUI;
        public GameObject doorLockedTMP;

        private void Start()
        {
            if (isLevel1)
            {
                bigWin = true;
            }
            

            player = GameObject.FindGameObjectWithTag("Player");
            canWin = false;

        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("TOuche baby");

                if (isLevel1)
                {

                    if(!canWin && !isCDV)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");

                        bigWin = true;
                        obj.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        obj.gameObject.GetComponent<Collider2D>().enabled = false;
                        
                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);                   
                        StartCoroutine(TextKeyLooted());
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }
                  


                    if(bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);                      
                    }
                }

                if (isLevel2)
                {

                    if (!canWin && !isCDV)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");

                        bigWin = true;
                        obj.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        obj.gameObject.GetComponent<Collider2D>().enabled = false;

                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);
                        StartCoroutine(TextKeyLooted());
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }



                    if (bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);
                    }
                }
            }
        }

        IEnumerator TextDoorLocked()
        {
            doorLockedTMP.SetActive(true);

            yield return new WaitForSeconds(5f);

            doorLockedTMP.SetActive(false);
        }

        IEnumerator TextKeyLooted()
        {
            keyTMP.SetActive(true);

            yield return new WaitForSeconds(5f);

            keyTMP.SetActive(false);
            Destroy(obj);
        }
    }
}
