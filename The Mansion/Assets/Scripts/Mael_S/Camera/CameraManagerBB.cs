using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraManagerBB : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 9, false);
        Physics2D.IgnoreLayerCollision(11, 2, false);
    }

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
