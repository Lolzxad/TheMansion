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

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

