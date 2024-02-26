using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Canvas))]
    public class RTSWorldCanvas : RTSView 
    {
        public Canvas canvas;
        public Camera activeCamera => gameView.cameraController.currentCamera;
        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            canvas.worldCamera = activeCamera;
            canvas.transform.rotation = Quaternion.LookRotation(activeCamera.transform.forward, activeCamera.transform.up);
        }
    }
}