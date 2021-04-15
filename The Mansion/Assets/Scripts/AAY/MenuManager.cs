using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMansion
{
    public class MenuManager : MonoBehaviour
    {
        public void ChangeLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadLevel1()
        {
            SceneManager.LoadScene("Niveau_1_TUTO");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

