using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] GameObject fullLevels;
    [SerializeField] GameObject lockedLevels;

    public bool canPlayMusic;
    public bool canPlaySFX;

    private void Update()
    {
        PlayerPrefs.SetInt("CanPlayMusic", (canPlayMusic ? 1 : 0));
        PlayerPrefs.SetInt("CanPlaySFX", (canPlaySFX ? 1 : 0));
    }
}
