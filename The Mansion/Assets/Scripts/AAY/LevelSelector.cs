using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void ChangeLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
