using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMansion
{


    public class VictoryManager : MonoBehaviour
    {
        GameObject player;
        
        public bool canWin;

        public static bool sideObj1;
        public static bool sideObj2;
        public bool isDoor_O;
        public bool isDoor_Y;
        public bool isKey_O;
        public bool isKey_Y;

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

        AudioManagerVEVO audioManager;
        MenuManager menu;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            menu = FindObjectOfType<MenuManager>();
        }

        private void Start()
        {
            bigWin = false;
            Time.timeScale = 1f;

            if (isLevel1)
            {
                bigWin = true;
            }

            if (!menu.cannotPlayMusic)
            {
                audioManager.PlayAudio(AudioType.Ambience_1_ST);
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
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        bigWin = true;
                        
                        obj.gameObject.SetActive(false);
                        
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
                        audioManager.PlayAudio(AudioType.Door_Opened_SFX);
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);
                        menu.level2Unlocked = true;
                        PlayerPrefs.SetInt("Level2Unlocked", (menu.level2Unlocked ? 1 : 0));
                    }
                }

                if (isLevel2)

                {

                    if (!canWin && !isCDV)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        bigWin = true;
                        

                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);
                        StartCoroutine(TextKeyLooted());
                        obj.gameObject.SetActive(false);
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }



                    if (bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        audioManager.PlayAudio(AudioType.Door_Opened_SFX);
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);
                        menu.level3Unlocked = true;
                        PlayerPrefs.SetInt("Level3Unlocked", (menu.level3Unlocked ? 1 : 0));
                    }
                }

                if (isLevel3)

                {

                    if (!canWin && !isCDV)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        bigWin = true;
                        obj.gameObject.SetActive(false);

                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);
                        //StartCoroutine(TextKeyLooted());
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }



                    if (bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        audioManager.PlayAudio(AudioType.Door_Opened_SFX);
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);
                        menu.level4Unlocked = true;
                        PlayerPrefs.SetInt("Level4Unlocked", (menu.level4Unlocked ? 1 : 0));
                    }
                }

                if (isLevel4)

                {

                    if (!canWin && !isCDV)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        bigWin = true;
                        obj.gameObject.SetActive(false);

                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);
                        //StartCoroutine(TextKeyLooted());
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }



                    if (bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        audioManager.PlayAudio(AudioType.Door_Opened_SFX);
                        Time.timeScale = 0;
                        //anim (on va utiliser une ptite coroutine
                        menuWin.SetActive(true);
                        menu.level5Unlocked = true;
                        PlayerPrefs.SetInt("Level5Unlocked", (menu.level5Unlocked ? 1 : 0));
                    }
                }

                if (isLevel5)

                {

                    if (!canWin && !isCDV && !isDoor_O && !isDoor_Y && !isKey_O && !isKey_Y)
                    {
                        Debug.Log("T'as trouvé la clé, bien ouej");
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        bigWin = true;
                        obj.gameObject.SetActive(false);

                        //supp la clé dans la map (fx)
                        keyUI.SetActive(true);
                        //StartCoroutine(TextKeyLooted());
                    }

                    if (isKey_O)
                    {
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        sideObj1 = true;
                        gameObject.SetActive(false);
                    }


                    if (isKey_Y)
                    {
                        audioManager.PlayAudio(AudioType.Recup_Cle_SFX);
                        sideObj2 = true;
                        gameObject.SetActive(false);
                    }

                    if (!canWin && !isCDV && sideObj1 && isDoor_O)
                    {
                        gameObject.SetActive(false);
                    }

                    if (!canWin && !isCDV && sideObj2 && isDoor_Y)
                    {
                        gameObject.SetActive(false);
                    }



                    if (!bigWin && isCDV)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }

                    if(!bigWin && isDoor_O)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }

                    if (!bigWin && isDoor_Y)
                    {
                        Debug.Log("La porte est fermée frero");

                        StartCoroutine(TextDoorLocked());
                    }



                    if (bigWin && isCDV)
                    {
                        Debug.Log("C'est bon t'as win");
                        audioManager.PlayAudio(AudioType.Door_Opened_SFX);
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
