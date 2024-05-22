using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameKit.Unity.UnityNetork;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZergRush;
using ZergRush.ReactiveCore;

namespace Game.GameCore.GameControllers
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession instance;
        public PredictionRollbackClientMultiplayerController<GameModel> clientController;
        public PredictionRollbackServerEngine<GameModel> serverController;

        
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            GameConfig.Reload();
        }
        
        private void Update()
        {
            serverController?.UpdateBattleSimulation();
            clientController?.UnityUpdate(Time.deltaTime);
        }

        private void OnApplicationQuit()
        {
            clientController?.Dispose();
            serverController?.Dispose();
            
            GameConfig.Instance.Save();
        }
        

        public async Task StartLobby(Func<GameModel> gmGetter)
        {
            await SceneManager.LoadSceneAsync("LevelTest");
            await Task.Yield();
            
            LobbyView.instance.Show(gmGetter);
            GameView.instance.SetupGameModel(gmGetter);
        }
        
        public async Task StartGameLocal(GameModel model)
        {
            await SceneManager.LoadSceneAsync("LevelTest");
            await Task.Yield();
            
            GameView.instance.SetupGameModel(() => model);
        }

        public GameModel GetTestModel()
        {
            GameModel game = new GameModel();
            Faction myFaction = new Faction()
            {
                slot = FactionSlot.Player1
            };
            Faction enemyFaction = new Faction()
            {
                slot = FactionSlot.Player2
            };
            
            game.Init(new[] { myFaction, enemyFaction });
            
            game.factions[0].Init(game,new List<UnitConfig>()
            {
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0]
            });
            
            game.factions[1].Init(game, new List<UnitConfig>()
            {
                GameConfig.Instance.units[1],
                GameConfig.Instance.units[1],
                GameConfig.Instance.units[1],
                GameConfig.Instance.units[1],
                GameConfig.Instance.units[1]
            });
            
            game.units[0].transform.position = new Vector3(0, 0, 0);
            game.units[1].transform.position = new Vector3(3, 0, 0);
            game.units[2].transform.position = new Vector3(0, 0, 3);
            game.units[3].transform.position = new Vector3(3, 0, 3);
            game.units[4].transform.position = new Vector3(6, 0, 0);
            
            game.units[5].transform.position = new Vector3(10, 0, 10);
            game.units[6].transform.position = new Vector3(13, 0, 10);
            game.units[7].transform.position = new Vector3(10, 0, 13);
            game.units[8].transform.position = new Vector3(13, 0, 13);
            game.units[9].transform.position = new Vector3(16, 0, 10);

            return game;
        }

        public async void StartClient(string ip, ushort port, long playerID, string joinCode)
        {
            GameObject transportClientGM = new GameObject("[Client]");
            transportClientGM.AddComponent<UnityNetworkClient>();
            var transport = transportClientGM.GetComponent<UnityNetworkClient>();
            DontDestroyOnLoad(transportClientGM);


            if (joinCode == "######")
            {
                if (NetworkEndpoint.TryParse(ip, port, out var endpoint, NetworkFamily.Ipv4) == false)
                {
                    Debug.LogError($"Can't parse endpoint {ip}:{port}");
                }

                await transport.ConnectToServer(endpoint, playerID);
            }
            else
            {
                await transport.ConnectToServerWithRelay(playerID, joinCode);
            }
            
            clientController = new PredictionRollbackClientMultiplayerController<GameModel>();

            var multiplayerTransportClient = new MultiplayerTransportClient();
            multiplayerTransportClient.sendToChannel += transport.SendToChannel;
            multiplayerTransportClient.listenToChannel += transport.ListenToChannel;
                
            await InitAndWaitController(multiplayerTransportClient);

            await StartLobby(() => clientController.currentModel);
        }

        public async Task InitAndWaitController(MultiplayerTransportClient multiplayerTransportClient)
        {
            clientController.Init(multiplayerTransportClient, GetModelDelegate(), GameModel.FrameTimeMS);

            while (clientController.state != ControllerStatus.Normal)
                await Task.Yield();
        }

        public async void StartHost(ushort port, bool localPlay, bool useRelay)
        {
            GameObject gm = new GameObject("[Server]");
            gm.AddComponent<UnityNetworkServer>();
            var serverTransport = gm.GetComponent<UnityNetworkServer>();
            DontDestroyOnLoad(gm);

            if (useRelay)
                await serverTransport.InitTransportWithRelay(port);
            else
                await serverTransport.InitTransport(port);
            
            serverController = new PredictionRollbackServerEngine<GameModel>();

            var multiplayerTransport = new MultiplayerTransportServer();
            multiplayerTransport.sendToChannel += serverTransport.SendToChannel;
            multiplayerTransport.listenToChannel += serverTransport.ListenToChannel;
            multiplayerTransport.broadcastChannel += serverTransport.BroadcastToChannel;
            multiplayerTransport.onUserConnected += serverTransport.OnUserConnected;
            multiplayerTransport.onUserDisconnected += serverTransport.OnUserDisconnected;

            if (localPlay == true)
            {
                var (newTransport, localConnectFactory, localDisconnect) =
                    MultiplayerTransportTools.LocalPlay(multiplayerTransport);
                GameModel model = GetTestModel();

                var gamemodelDelegate = GetModelDelegate();

                serverController.Init(newTransport, gamemodelDelegate, model, null);
                var localClientTransport = localConnectFactory.Invoke(0);
                clientController = new PredictionRollbackClientMultiplayerController<GameModel>();

                await InitAndWaitController(localClientTransport);

                await StartLobby(() => clientController.currentModel);
            }
            else
            {
                GameModel model = GetTestModel();
                var gamemodelDelegate = GetModelDelegate();

                serverController.Init(multiplayerTransport, gamemodelDelegate, model, null);
            }
        }

        private static MultiplayerServerGameModelDelegate GetModelDelegate()
        {
            var gamemodelDelegate = new MultiplayerServerGameModelDelegate();
            gamemodelDelegate.gameRootCommandType = typeof(RTSCommand);
            gamemodelDelegate.isUserAllowedToConnect += player => true;
            gamemodelDelegate.newPlayerCommandGenerator +=
                (connectionHandler, playerId, playerServerid) => new ConnectCommand()
                {
                    globalPlayerId = playerId,
                };
            gamemodelDelegate.playerLeaveCommandGenerator +=
                (connectionHandler, playerId, playerServerid) => new LogCommand(){ message = $"player disconnected {playerId}" };
            return gamemodelDelegate;
        }
        
        public void SendRTSCommand(RTSCommand cmd)
        {
            clientController?.WriteLocalAndSendCommand(cmd);
        }
    }
}