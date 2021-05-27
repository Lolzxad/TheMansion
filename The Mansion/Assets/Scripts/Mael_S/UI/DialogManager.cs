using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace TheMansion
{
    public class DialogManager : MonoBehaviour
    {
        public TextMeshProUGUI textDisplayDoorLocked;
        public string[] sentences;
        private int index;
        public float typingSpeed;
        [SerializeField] GameObject fond;

        public GameObject continueButton;

        [Header("Bools frer")]
        public bool isTimeToHide;
        public bool isTimeToCalmHeart;

        PlayerController playerScript;
        GameObject tuto;
        AudioManagerVEVO audioManager;
        TutoManager tutoScript;


        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManagerVEVO>();
            tutoScript = FindObjectOfType<TutoManager>();
        }

        private void Start()
        {
            StartCoroutine(Type());
            playerScript = FindObjectOfType<PlayerController>();
            playerScript.canMove = false;

            if (isTimeToHide)
            {
                playerScript.GetComponent<PlayerController>().enabled = false;
            }

            if (isTimeToCalmHeart)
            {
                playerScript.GetComponent<PlayerController>().enabled = false;
            }
        }

        private void Update()
        {
            if(textDisplayDoorLocked.text == sentences[index])
            {
                continueButton.SetActive(true);
            }
        }

        IEnumerator Type()
        {

            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplayDoorLocked.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }

        public void NextSentence()
        {
            continueButton.SetActive(false);

            audioManager.PlayAudio(AudioType.Click_Button_SFX);


            if (index < sentences.Length - 1)
            {
                index++;
                textDisplayDoorLocked.text = "";
                StartCoroutine(Type());
            }
            else
            {
                textDisplayDoorLocked.text = "";
               
                playerScript.canMove = true;
                fond.SetActive(false);

                if (isTimeToHide)
                {
                    playerScript.GetComponent<PlayerController>().enabled = true;
                }

                if (isTimeToCalmHeart)
                {
                    tutoScript.heartInput.SetActive(true);
                }
            }
        }
    }
}

