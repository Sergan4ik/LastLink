using System;
using Game.GameCore.GameControllers;
using UnityEngine.UI;

namespace Game
{
    public class TestUI : ConnectableMonoBehaviour
    {
        public Button startTestBattle;
        public Button startHost;
        public Button startClient;

        public bool autoStartHost = false;
        public bool autoStartClient = false;

        public void Start()
        {
            connections += startTestBattle.Subscribe(() => GameSession.instance.StartGame(GameSession.instance.GetTestModel()));
            connections += startHost.Subscribe(() => GameSession.instance.StartHost());
            connections += startClient.Subscribe(() => GameSession.instance.StartClient());
            
            if (autoStartHost)
            {
                GameSession.instance.StartHost();
            }
            else if (autoStartClient)
            {
                GameSession.instance.StartClient();
            }
        }
    }
}