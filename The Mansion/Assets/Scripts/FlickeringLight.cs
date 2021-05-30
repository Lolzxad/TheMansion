using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    public Animator animController;
    private int indexAnim;
    private int delayFlicker;

    void Start()
    {
        StartCoroutine(WaitTillFlicker());
    }

    IEnumerator WaitTillFlicker()
    {
        delayFlicker = Random.Range(0, 1);
        yield return new WaitForSeconds(delayFlicker);
        indexAnim = Random.Range(0, 2);
        animController.SetInteger("LightType", indexAnim);
        Debug.Log(indexAnim);
    }

}
