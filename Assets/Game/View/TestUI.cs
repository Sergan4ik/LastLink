using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.GameCore;
using Game.GameCore.GameControllers;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
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


        // 0 - login, 1 - logging in, 2 - logout
        private StateButton loginState;
        
        public void Start()
        {
            avatarDropdown.ClearOptions();
            avatarDropdown.AddOptions(Tools.GetPlayerAvatars()
                .Select(s => new TMP_Dropdown.OptionData(s.name, s, Color.white)).ToList());


            SetupGameConnection();
            SetupAuthentication();
            
            username.text = PlayerPrefs.GetString("username");
            password.text = PlayerPrefs.GetString("password");
        }

        private void SetupAuthentication()
        {
            loginState = loginButton.SetupButtonStates((b, label, i) =>
            {
                b.interactable = true;
                if (i == 0)
                {
                    label.text = "Login";
                }
                else if (i == 1)
                {
                    b.interactable = false;
                    label.text = "Logging in...";
                }
                else if (i == 2)
                {
                    label.text = "Logout";
                }
            }, loginButton.GetComponentInChildren<TextMeshProUGUI>());
            
            connections += loginButton.Subscribe(async () =>
            {
                if (loginState.state == 2)
                {
                    await loginState.HoldStateUntilTaskDone(GameSession.instance.Logout(), 1, 0);
                    return;
                }
                if (loginState.state == 0)
                {
                    var res = await loginState.HoldStateUntilTaskDone(GameSession.instance.Login(username.text, password.text), 1,
                        session => session == null ? 0 : 2);
                    if (res != null)
                    {
                        PlayerPrefs.SetString("username", username.text);
                        PlayerPrefs.SetString("password", password.text);
                    }
                }

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