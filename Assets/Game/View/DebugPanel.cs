using Game.GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class DebugPanel : RTSView
    {
        public RectTransform panel;
        
        public TextMeshProUGUI currentModelSize;
        private RTSTimerStatic timer;

        private void Awake()
        {
            timer = new RTSTimerStatic()
            {
                staticCycleTime = 1f
            };
        }

        private void Update()
        {
            if (game == null) return;

            if (timer.Tick(Time.deltaTime))
            {
                currentModelSize.text = $"Model: {game.GetModelSize()} bytes";
            }

            if (Keyboard.current.f10Key.wasPressedThisFrame)
            {
                gameSession.SendRTSCommand(new SpawnRandomUnitCommand());
            }
            
        }
    }
}