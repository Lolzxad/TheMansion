using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraManagerBB : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            ProCamera2DShake.Instance.ConstantShake("BBisNear");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {

            ProCamera2DShake.Instance.StopConstantShaking();
            Debug.Log("Boom c bon tkt tout vas bien");
        }
    }
}
