using System;
using System.Threading.Tasks;
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
        public TextMeshProUGUI connectedClients;
        
        private RTSTimerStatic timer;

        private void Awake()
        {
            timer = new RTSTimerStatic()
            {
                staticCycleTime = 1f
            };

            ZeroLagSettings.messageLogs = false;
            ZeroLagSettings.debugMode = false;
            ZeroLagSettings.performHealthCheckIntervalMS = 3000;
        }

        private async void Update()
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
            
            if (Keyboard.current.f11Key.wasPressedThisFrame)
            {
                gameSession.SendRTSCommand(new DeleteLastUnitCommand());
            }
            
            if (Keyboard.current.f2Key.wasPressedThisFrame)
            {
                ZeroLagSettings.messageLogs = !ZeroLagSettings.messageLogs;
            }
         
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                MakeDebugClient();
            }
            
            
            connectedClients.text = $"Connected clients: {game.controlData.Count}";
        }

        private async void MakeDebugClient()
        {
            string code = "";
            if (gameSession.clientTransport != null) 
                code = await gameSession.clientTransport.GetJoinCode();
            else if (gameSession.serverTransport != null)
                code = await gameSession.serverTransport.GetJoinCode();
            
            await gameSession.AddDebugClient("127.0.0.1", 7777, Int64.MaxValue - game.controlData.Count, code);
        }
    }
}