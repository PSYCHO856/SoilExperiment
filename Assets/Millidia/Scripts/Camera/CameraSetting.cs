using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPGameDemo
{
    public enum InputChoice
    {
        KeyboardAndMouse,
        Controller,
    }

    public class CameraSetting : MonoBehaviour
    {
       
        [Serializable]
        public struct InvertSettings
        {
            public bool invertX;
            public bool invertY;
        }


        public CinemachineFreeLook keyboardAndMouseCamera;
        public InputChoice inputChoice;
        public InvertSettings keyboardAndMouseInvertSettings;
        public InvertSettings controllerInvertSettings;
        public bool allowRuntimeCameraSettingsChanges;

        public CinemachineFreeLook Current
        {
            get { return inputChoice == InputChoice.KeyboardAndMouse? keyboardAndMouseCamera : null; }
        }

        private void Awake()
        {
        }

        void Update()
        {
            UpdateCameraSettings();
        }
        // 相机跟随
        void UpdateCameraSettings() {
            // 键鼠控制
            if(Input.GetMouseButton(1)){
                keyboardAndMouseCamera.m_YAxis.Value-=Input.GetAxis("CameraY")/10;
                keyboardAndMouseCamera.m_XAxis.Value-=Input.GetAxis("CameraX")*10;
            }
        }
    }
}

