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

        #region Levels
        public bool isLevel1;
        public bool isLevel2;
        public bool isLevel3;
        public bool isLevel4;
        public bool isLevel5;
        public bool isLevel6;
        public bool isLevel7;

        #endregion

        private void Start()
        {
            //provisoire
            isLevel1 = true;

            player = GameObject.FindGameObjectWithTag("Player");
            canWin = false;

            sideObj1 = false;
            sideObj2 = false;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("TOuche baby");

                if (isLevel1)
                {
                    if (canWin && sideObj1 && isCDV)
                    {
                        Debug.Log("Level complete!");
                        //apparait un menu (same scene) lui félicitant d'avoir terminé le niveau 
                            //il peut passer au menu ou sélection de niveau
                    }

                    if (!canWin)
                    {
                        Debug.Log("Quest item done");
                        sideObj1 = true;
                        canWin = true;
                        //supp le clé tavu
                        //supp la clé dans la map (fx)
                        //frère c'est bon tu peux ouvrir la porte et finir ce niveau
                        //clé qui apparait au coin de l'écran tavu #feedback

                    }
                }
                

            }
        }
    }
}
