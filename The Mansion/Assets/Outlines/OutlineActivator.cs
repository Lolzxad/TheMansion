using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineActivator : MonoBehaviour
{
    MaterialPropertyBlock propBlock;
    SpriteRenderer rend;
    float opacity = 0;

    float transitionDuration = 0.2f;

    void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EnableOutline();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DisableOutline();
        }
    }

    public void EnableOutline()
    {
        StopAllCoroutines();
        StartCoroutine("IncreaseOutline");
    }

    public void DisableOutline()
    {
        StopAllCoroutines();
        StartCoroutine("DecreaseOutline");
    }


    IEnumerator IncreaseOutline()
    {
        while(opacity < 1f)
        {
            opacity += Time.deltaTime / transitionDuration;
            opacity = Mathf.Clamp01(opacity);
            SetOpacity(opacity);
            yield return null;
        }
    }

    IEnumerator DecreaseOutline()
    {
        while (opacity > 0f)
        {
            opacity -= Time.deltaTime / transitionDuration;
            opacity = Mathf.Clamp01(opacity);
            SetOpacity(opacity);
            yield return null;
        }
    }

    private void SetOpacity(float value)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_Opacity", value);
        rend.SetPropertyBlock(propBlock);
    }
}
