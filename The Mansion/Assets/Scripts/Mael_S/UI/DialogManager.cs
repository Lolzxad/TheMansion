﻿using System.Collections;
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

        public GameObject continueButton;

        private void Start()
        {
            StartCoroutine(Type());
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

            if(index < sentences.Length - 1)
            {
                index++;
                textDisplayDoorLocked.text = "";
                StartCoroutine(Type());
            }
            else
            {
                textDisplayDoorLocked.text = "";
            }
        }
    }
}
