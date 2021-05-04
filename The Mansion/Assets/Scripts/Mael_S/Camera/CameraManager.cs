using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


namespace TheMansion
{
    public class CameraManager : MonoBehaviour
    {
        PlayerController playerController;

        public Transform target;


        //[SerializeField] bool setNewCamera;
        [SerializeField] bool justOneCameraPls;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        public void Update()
        {
            if (playerController.isHiding)
            {

                ProCamera2D.Instance.CenterOnTargets();
           
               // ProCamera2DForwardFocus.ExtensionName.
            }

            

            if (playerController.isGrabbed)
            {
                
               ProCamera2D.Instance.CenterOnTargets();

            }

          /*  if (!playerController.isGrabbed)
            {
                setNewCamera = true;
            }*/
        }


    }
}

