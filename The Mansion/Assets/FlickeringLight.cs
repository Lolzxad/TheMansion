using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    public Animator animController;
    public int indexAnim;
    public float delayFlicker;

    void Start()
    {
        
    }

    IEnumerator WaitTillFlicker()
    {
        yield return new WaitForSeconds(delayFlicker);
    }

}
