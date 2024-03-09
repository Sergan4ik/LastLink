using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameCore.GameControllers
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession instance;

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

        public async void StartTestBattle()
        {
            GameModel game = new GameModel()
            {
                logger = new UnityLogger()
            };
            Faction myFaction = new Faction()
            {
                slot = FactionSlot.Player1
            };
            Faction enemyFaction = new Faction()
            {
                slot = FactionSlot.Player2
            };
            
            game.Init(new[] { myFaction, enemyFaction });
            
            game.controlData.Add(new ControlData()
            {
                factionSlot = FactionSlot.Player1,
                serverPlayerId = 0
            });
            game.controlData.Add(new ControlData()
            {
                factionSlot = FactionSlot.Player2,
                serverPlayerId = -1
            });
            
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
            
            await SceneManager.LoadSceneAsync("LevelTest");
            await Task.Yield();
            
            GameView.instance.SetupGameModel(game, 0);
        }

        private void OnApplicationQuit()
        {
            GameConfig.Instance.Save();
        }
    }
}