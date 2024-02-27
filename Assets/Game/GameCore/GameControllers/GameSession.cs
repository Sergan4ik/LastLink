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
            Faction myFaction = new Faction();
            game.Init(new[] { myFaction });
            game.factions[0].Init(new List<UnitConfig>()
            {
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0],
                GameConfig.Instance.units[0]
            });
            
            game.factions[0].units[0].transform.position = new Vector3(0, 0, 0);
            game.factions[0].units[1].transform.position = new Vector3(3, 0, 0);
            game.factions[0].units[2].transform.position = new Vector3(0, 0, 3);
            game.factions[0].units[3].transform.position = new Vector3(3, 0, 3);
            game.factions[0].units[4].transform.position = new Vector3(6f, 0, 0f);

            await SceneManager.LoadSceneAsync("LevelTest");
            await Task.Yield();
            
            GameView.instance.SetupGameModel(game);
        }

        private void OnApplicationQuit()
        {
            GameConfig.Instance.Save();
        }
    }
}