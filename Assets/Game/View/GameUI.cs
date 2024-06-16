using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class GameUI : RTSView
    {
        public SelectionUI selectionUI;

        public RectTransform debugPanel;

        public void Update()
        {
            if (Keyboard.current.f12Key.wasPressedThisFrame)
            {
                debugPanel.gameObject.SetActive(!debugPanel.gameObject.activeSelf);
            }
            
        }
    }
}