using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraManagerBB : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        Physics2D.IgnoreLayerCollision(11, 9, true);
        //Physics2D.IgnoreLayerCollision(11, 2, true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            ProCamera2DShake.Instance.ConstantShake("BBisNear");
            Debug.Log("Boom ca bouge");
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
