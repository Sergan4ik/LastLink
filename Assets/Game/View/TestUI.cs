using System;
using Game.GameCore.GameControllers;
using UnityEngine.UI;

namespace Game
{
    public class TestUI : ConnectableMonoBehaviour
    {
        public Button startTestBattle;

        public void Start()
        {
            connections += startTestBattle.Subscribe(() => GameSession.instance.StartTestBattle());
        }
    }
}