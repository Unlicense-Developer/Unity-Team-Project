using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CartDelivery
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        CinemachineFreeLook camera1; //회전 하게될 카메라
        [SerializeField]
        CinemachineVirtualCameraBase camera2;
        bool isCamera2Active = false;
        public bool gamePlaying = false;
        public float mouseSensitivity = 50.0f; // 마우스 감도
        public Transform playerCamTr;  // 플레이어 위치값 

        void Start()
        {
            camera1.m_XAxis.m_MinValue = 0;
            camera1.m_XAxis.m_MaxValue = 0;
        }
        public void OnCameraSign()
        {
            if (gamePlaying == true)
            {
                camera1.m_XAxis.m_MinValue = -90;
                camera1.m_XAxis.m_MaxValue = 90;
            }
            // camera1.enabled = true;
        }



        void ChangeCamera()
        {
            if (gamePlaying == true)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isCamera2Active = !isCamera2Active;
                    if (isCamera2Active)
                    {
                        camera1.Priority = 10;
                        camera2.Priority = 15;
                    }
                    else
                    {
                        camera1.Priority = 15;
                        camera2.Priority = 10;
                    }
                }
            }
            else
            {
                return;
            }
        }
        void Update()
        {
            ChangeCamera();
        }
    }
}

