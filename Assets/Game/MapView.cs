using System;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game
{
    public class MapView : ConnectableMonoBehaviour
    {
        public NavMeshAgent testAgent;
        public RTSCameraController cameraController;

        public PlayerInput input;
        private void Start()
        {
            input = new PlayerInput();
            input.Enable();
            input.RTS.Enable();
        }

        public void Update()
        {
            if (input.RTS.SecondaryAction.WasPerformedThisFrame())
            {
                testAgent.destination = cameraController.worldMousePosition;
            }
        }
    }
}