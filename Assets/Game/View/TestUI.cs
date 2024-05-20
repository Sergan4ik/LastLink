using System;
using Game.GameCore.GameControllers;
using TMPro;
using UnityEngine.UI;
using ZergRush;

namespace Game
{
    public class TestUI : ConnectableMonoBehaviour
    {
        public Button startTestBattle;
        public Button startHost;
        public Button startClient;

        public TMP_InputField username;
        
        public TMP_InputField ipToConnect;
        public TMP_InputField portToConnect;
        public TMP_InputField joinCode;
        
        public TMP_InputField portToHost;

        public Toggle localPlayToggle;
        
        public bool autoStartHost = false;
        public bool autoStartClient = false;
        
        public ushort portToHostValue => ushort.TryParse(portToHost.text, out var result) ? result : (ushort)7777;
        public ushort portToConnectValue => ushort.TryParse(portToConnect.text, out var result) ? result : (ushort)7777;
        public string ipToConnectValue => ipToConnect.text.IsNullOrEmpty() ? "127.0.0.1" : ipToConnect.text;
        

        public void Start()
        {
            #if !UNITY_EDITOR
            autoStartHost = false;
            autoStartClient = false;
            #endif
            
            connections += startTestBattle.Subscribe(() => GameSession.instance.StartGame(() => GameSession.instance.GetTestModel()));
            connections += startHost.Subscribe(() =>
            {
                GameSession.instance.StartHost(portToHostValue, localPlayToggle.isOn);
            });
            connections += startClient.Subscribe(() =>
            {
                GameSession.instance.StartClient(ipToConnectValue, portToConnectValue, int.Parse(username.text), joinCode.text);
            });
            
            if (autoStartHost)
            {
                GameSession.instance.StartHost(portToHostValue, localPlayToggle.isOn);
            }
            else if (autoStartClient)
            {
                GameSession.instance.StartClient(ipToConnectValue, portToConnectValue, int.Parse(username.text), joinCode.text);
            }
        }
    }
}