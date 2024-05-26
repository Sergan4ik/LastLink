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

            avatarDropdown.ClearOptions();
            avatarDropdown.AddOptions(Tools.GetPlayerAvatars()
                .Select(s => new TMP_Dropdown.OptionData(s.name, s, Color.white)).ToList());


            SetupGameConnection();
            SetupAuthentication();
            
            if (autoStartHost)
            {
                GameSession.instance.StartHost(portToHostValue, localPlayToggle.isOn, useRelayToggle.isOn, GameSession.instance.accessToken.value);
            }
            else if (autoStartClient)
            {
                GameSession.instance.StartClient(ipToConnectValue, portToConnectValue, int.Parse(username.text),
                    joinCode.text);
            }
        }

        private void SetupAuthentication()
        {
            connections += loginButton.Subscribe(async () =>
            {
                GameSession.instance.accessToken.value = await GameSession.instance.awsAuth.Authenticate(
                    new BaseCredentials()
                    {
                        userId = username.text,
                        password = password.text
                    });

                Debug.Log($"Logged in: {GameSession.instance.accessToken.value.token}");
            });

            connections += registerButton.Subscribe(async () =>
            {
                await GameSession.instance.awsAuth.Register(new BaseSignUpCredentials()
                {
                    userId = username.text,
                    password = password.text,
                    playerId = await GameSession.instance.awsAuth.GetNextPlayerId(
                        GameSession.instance.playerDatabase.client, AWS_CONFIG.PLAYER_DATA_TABLE_NAME)
                });
            });

            connections += GameSession.instance.awsAuth.onRegisteredPlayer.Subscribe(async p =>
            {
                Debug.Log($"Registered player: {p.userId}, id {p.playerId}");
                await GameSession.instance.playerDatabase.AddPlayer(p.playerId, new RTSPlayerData()
                {
                    playerId = p.playerId,
                    username = p.userId,
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
                GameSession.instance.StartHost(portToHostValue, useRelayToggle.isOn, localPlayToggle.isOn,
                    GameSession.instance.accessToken.value);
            });
            connections += startClient.Subscribe(async () =>
            {
                GameSession.instance.StartClient(ipToConnectValue, portToConnectValue,
                    GameSession.instance.accessToken.value, joinCode.text);
            });
            connections += GameSession.instance.accessToken.Bind(t =>
            {
                startHost.interactable = t != null;
                startClient.interactable = t != null;
            });

        }
    }
}