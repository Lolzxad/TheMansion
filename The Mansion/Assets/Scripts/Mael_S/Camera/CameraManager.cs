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


        [SerializeField] bool setNewCamera;
        [SerializeField] bool justOneCameraPls;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        public void Update()
        {
            if (playerController.isHiding)
            {
              
                   ProCamera2D.Instance.AdjustCameraTargetInfluence(target, 1f, 1f, 1f);
           
                
            }

            

            if (playerController.isGrabbed)
            {
                if (setNewCamera)
                {
                    ProCamera2D.Instance.AddCameraTarget(target, 1f, 1f, 1f);
                    setNewCamera = false;
                }

            }

            if (!playerController.isGrabbed)
            {
                setNewCamera = true;
            }
        }


    }
}

