using System;
using System.Linq;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using Game.GameCore;
using Game.GameCore.GameControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZergRush;
using ZergRush.ReactiveCore;
using ZeroLag.MultiplayerTools.Modules.Authentication;
using ZeroLag.MultiplayerTools.Modules.Database;

namespace Game
{
    public class TestUI : ConnectableMonoBehaviour
    {
        public Button startTestBattle;
        public Button startHost;
        public Button startClient;

        public Button loginButton;
        public Button registerButton;

        public TMP_InputField username;
        public TMP_InputField password;
        public TMP_Dropdown avatarDropdown;
        
        public TMP_InputField ipToConnect;
        public TMP_InputField portToConnect;
        public TMP_InputField joinCode;
        
        public TMP_InputField portToHost;

        public Toggle localPlayToggle;
        public Toggle useRelayToggle;
        
        public ushort portToHostValue => ushort.TryParse(portToHost.text, out var result) ? result : (ushort)7777;
        public ushort portToConnectValue => ushort.TryParse(portToConnect.text, out var result) ? result : (ushort)7777;
        public string ipToConnectValue => ipToConnect.text.IsNullOrEmpty() ? "127.0.0.1" : ipToConnect.text;


        public void Start()
        {
            avatarDropdown.ClearOptions();
            avatarDropdown.AddOptions(Tools.GetPlayerAvatars()
                .Select(s => new TMP_Dropdown.OptionData(s.name, s, Color.white)).ToList());


            SetupGameConnection();
            SetupAuthentication();
        }

        private void SetupAuthentication()
        {
            connections += loginButton.Subscribe(async () =>
            {
                await GameSession.instance.Login(username.text, password.text);
                Debug.Log($"Logged in: {GameSession.instance.userSession.value.userId}");
            });

            connections += registerButton.Subscribe(async () =>
            {
                var result = await GameSession.instance.authenticator.RegisterAndLogin(username.text, password.text);
                long playerId = await GameSession.instance.playerDatabase.client.GetNextPlayerId(AWS_CONFIG.PLAYER_DATA_TABLE_NAME);
                await result.SetKeyValue("playerId", playerId.ToString());
                
                Debug.Log($"Registered player: {result.userId}, id {playerId}");
                await GameSession.instance.playerDatabase.AddPlayer(playerId, new RTSPlayerData()
                {
                    playerId = playerId,
                    username = result.userId,
                    customData = new RTSPlayerCustomData()
                    {
                        level = 1,
                        playerAvatar = avatarDropdown.options[avatarDropdown.value].image.name
                    }
                });
            });
        }

        private void SetupGameConnection()
        {
            connections += startTestBattle.Subscribe(() =>
                GameSession.instance.StartGameLocal(GameSession.instance.GetTestModel()));
            connections += startHost.Subscribe(() =>
            {
                GameSession.instance.StartHost(portToHostValue, useRelayToggle.isOn, localPlayToggle.isOn);
            });
            connections += startClient.Subscribe(async () =>
            {
                GameSession.instance.StartClient(ipToConnectValue, portToConnectValue, joinCode.text);
            });

            connections += GameSession.instance.canHostOrJoin.Bind(t =>
            {
                startHost.interactable = t;
                startClient.interactable = t;
            });
        }
    }
}