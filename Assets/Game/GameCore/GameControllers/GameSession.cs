using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using GameKit.Unity.UnityNetork;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZergRush.ReactiveCore;
using ZeroLag.MultiplayerTools.Modules.Authentication;
using ZeroLag.MultiplayerTools.Modules.Database;

namespace Game.GameCore.GameControllers
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession instance;
        public PredictionRollbackClientMultiplayerController<GameModel> clientController;
        public PredictionRollbackServerEngine<GameModel> serverController;

        [HideInInspector]
        public UnityNetworkClient clientTransport;
        [HideInInspector]
        public UnityNetworkServer serverTransport;
        
        public IAuthenticator authenticator;
        public Cell<IUserSession> userSession;
        public ICell<bool> loggedIn => userSession.Map(s => s != null);
        
        public ICell<bool> canHostOrJoin => loggedIn;
        
        public DynamoDBPlayerDatabase<RTSPlayerData, RTSPlayerCustomData> playerDatabase;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitAWS();
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

            model.ConnectPlayer(new ConnectCommand()
            {
                globalPlayerId = 0,
                serverPlayerId = 0
            });
            GameView.instance.SetupGameModel(() => model);
            model.GameStart();
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

        public async void StartClient(string ip, ushort port, string joinCode)
        {
            if (canHostOrJoin.value == false)
                return;
            
            StartClient(ip, port, await userSession.value.GetPlayerId(), joinCode);
        }
        
        public async void StartClient(string ip, ushort port, long playerID, string joinCode)
        {
            if (clientTransport != null)
            {
                Destroy(clientTransport.gameObject);
            }
            
            GameObject transportClientGM = new GameObject("[Client]");
            transportClientGM.AddComponent<UnityNetworkClient>();
            clientTransport = transportClientGM.GetComponent<UnityNetworkClient>();
            DontDestroyOnLoad(transportClientGM);

            if (joinCode == "######")
            {
                if (NetworkEndpoint.TryParse(ip, port, out var endpoint, NetworkFamily.Ipv4) == false)
                {
                    Debug.LogError($"Can't parse endpoint {ip}:{port}");
                }

                await clientTransport.ConnectToServer(endpoint, playerID);
            }
            else
            {
                await clientTransport.ConnectToServerWithRelay(playerID, joinCode);
            }
            
            clientController = new PredictionRollbackClientMultiplayerController<GameModel>();

            var multiplayerTransportClient = new MultiplayerTransportClient();
            multiplayerTransportClient.sendToChannel += clientTransport.SendToChannel;
            multiplayerTransportClient.listenToChannel += clientTransport.ListenToChannel;
                
            await InitAndWaitController(multiplayerTransportClient);

            await StartLobby(() => clientController.currentModel);
        }

        public async Task InitAndWaitController(MultiplayerTransportClient multiplayerTransportClient)
        {
            clientController.Init(multiplayerTransportClient, GetModelDelegate(), GameModel.FrameTimeMS);

            while (clientController.state != ControllerStatus.Normal)
                await Task.Yield();
        }

        public async void StartHost(ushort port, bool useRelay, bool localPlay)
        {
            if (canHostOrJoin.value == false)
                return;
            
            StartHost(port, useRelay, localPlay, await userSession.value.GetPlayerId());
        }
        
        public async void StartHost(ushort port, bool useRelay, bool localPlay, long playerId)
        {
            if (serverTransport != null)
            {
                Destroy(serverTransport.gameObject);
            }
            
            GameObject gm = new GameObject("[Server]");
            gm.AddComponent<UnityNetworkServer>();
            serverTransport = gm.GetComponent<UnityNetworkServer>();
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
                var localClientTransport = localConnectFactory.Invoke(playerId);
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

        public void InitAWS()
        {
            playerDatabase = new DynamoDBPlayerDatabase<RTSPlayerData, RTSPlayerCustomData>( 
                "AKIA3FLD46EXFWUY7LML",
                "WfKLs8dapD+a4riQ0bL4k3KQxeHolUk/LnXwnbYi",
                RegionEndpoint.EUNorth1,
                "LastLink");
            
            authenticator = new AWSCognitoAuthentication(
                "AKIA3FLD46EXFWUY7LML",
                "WfKLs8dapD+a4riQ0bL4k3KQxeHolUk/LnXwnbYi",
                RegionEndpoint.EUNorth1,
                "2v2a3bign3dt6khd2npuboprq5");

        }

        public async Task<IUserSession> Login(string username, string password)
        {
            userSession.value = await authenticator.Login(username, password);
            return userSession.value;
        }
    }
}