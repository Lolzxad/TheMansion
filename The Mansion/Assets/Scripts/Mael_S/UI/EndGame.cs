using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheMansion
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] Sprite letterRecup;
        [SerializeField] Sprite notebookRecup;

        [SerializeField] GameObject closeNarraButton;
        [SerializeField] GameObject closeNarraButton2;
        [SerializeField] GameObject closeNarraButton3;
        [SerializeField] GameObject narra_1;
        [SerializeField] GameObject narra_2;
        [SerializeField] GameObject narra_3;

        [SerializeField] GameObject narra_1_VEVO;
        [SerializeField] GameObject narra_2_VEVO;
        [SerializeField] GameObject narra_3_VEVO;

        MenuManager menu;
        VictoryManager victoryManager;


        [SerializeField] bool narraRecup_1;
        [SerializeField] bool narraRecup_2;
        [SerializeField] bool narraRecup_3;


        private void Start()
        {
            Time.timeScale = 0f;

            menu = FindObjectOfType<MenuManager>();
            victoryManager = FindObjectOfType<VictoryManager>();

            if(victoryManager.isLevel2 && menu.story1Get)
            {
                narra_1.GetComponent<Image>().sprite = notebookRecup;
                narraRecup_1 = true;
            }  
            
            if(victoryManager.isLevel3)
            {

                if (menu.story2Get)
                {
                    narra_1.GetComponent<Image>().sprite = letterRecup;
                    narraRecup_1 = true;
                }

                if (menu.story3Get)
                {
                    narra_2.GetComponent<Image>().sprite = notebookRecup;
                    narraRecup_2 = true;
                }
                
            }

            if (victoryManager.isLevel4)
            {
                if (menu.story4Get)
                {
                    narra_1.GetComponent<Image>().sprite = letterRecup;
                    narraRecup_1 = true;
                }

                if (menu.story5Get)
                {
                    narra_2.GetComponent<Image>().sprite = notebookRecup;
                    narraRecup_2 = true;
                }
            }


            if (victoryManager.isLevel5)
            {
                if (menu.story6Get)
                {
                    narra_1.GetComponent<Image>().sprite = letterRecup;
                    narraRecup_1 = true;
                }

                if (menu.story7Get)
                {
                    narra_2.GetComponent<Image>().sprite = notebookRecup;
                    narraRecup_2 = true;
                }

                if (menu.story8Get)
                {
                    narra_3.GetComponent<Image>().sprite = notebookRecup;
                    narraRecup_3 = true;
                }
            }
        }

        public void ShowNarra1()
        {
            if (narraRecup_1)
            {
                narra_1.SetActive(true);
                closeNarraButton.SetActive(true);
                narra_1_VEVO.SetActive(true);
            }
        }

        public void ShowNarra2()
        {
            if (narraRecup_2)
            {
                narra_2.SetActive(true);
                closeNarraButton2.SetActive(true);
                narra_2_VEVO.SetActive(true);
            }
        }

        public void ShowNarra3()
        {
            if (narraRecup_3)
            {
                narra_3.SetActive(true);
                closeNarraButton3.SetActive(true);
                narra_3_VEVO.SetActive(true);
            }
        }

        public void CloseNarra1()
        {
            
            closeNarraButton.SetActive(false);
            narra_1_VEVO.SetActive(false);
        }

        public void CloseNarra2()
        {
            
            closeNarraButton2.SetActive(false);
            narra_2_VEVO.SetActive(false);
        }

        public void CloseNarra_3()
        {
            
            closeNarraButton3.SetActive(false);
            narra_3_VEVO.SetActive(false);
        }

    }
}


