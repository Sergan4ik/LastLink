using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZergRush;
using ZergRush.CodeGen;

namespace Game.GameCore
{
    [GenTask(GenTaskFlags.SimpleDataPack)]
    public partial class GameConfig
    {
        public static GameConfig Instance
        {
            get
            {
                LoadInternal();
                return _instance;
            }
        }

        private static void LoadInternal()
        {
            if (_instance == null)
            {
                var textAsset = Resources.Load<TextAsset>("config");
                _instance = new GameConfig();
                if (textAsset != null)
                {
                    using var reader = new ZRJsonTextReader(textAsset.text);
                    _instance.ReadFromJson(reader);
                }
                else
                {
                    _instance = CreateDefault();
                }
            }
        }

        public static void Reload()
        {
            _instance = null;
            LoadInternal();
        }

        public static string[] savePaths = new[]
        {
            "Assets/Resources/config.json",
        };

        private static GameConfig _instance;
        
        public List<UnitConfig> units;
        
        public static GameConfig CreateDefault()
        {
            return new GameConfig()
            {
                units = new List<UnitConfig>()
                {
                    UnitConfig.CreateDefault()
                }
            };
        }
        
        public void Save()
        {
            string json = this.SaveToJsonString();
            foreach (var path in savePaths)
            {
                File.WriteAllText(path, json);
            }
        }
    }
    
    [GenTask(GenTaskFlags.SimpleDataPack)]
    public partial class ConfigData {}

    public partial class UnitConfig : ConfigData
    {
        public string name;
        public List<UnitLevelConfig> levelConfig;
        
        public UnitLevelConfig GetLevelConfig(int level)
        {
            return levelConfig[level];
        }
        
        public static UnitConfig CreateDefault()
        {
            return new UnitConfig()
            {
                name = "DefaultUnit",
                levelConfig = new List<UnitLevelConfig>()
                {
                    new UnitLevelConfig()
                    {
                        stats = new UnitStatsContainer()
                        {
                            stats = new List<UnitStat>()
                            {
                                new UnitStat() {type = UnitStatType.Health, currentValue = 100, maxValue = 100},
                                new UnitStat() {type = UnitStatType.Mana, currentValue = 100, maxValue = 100},
                                new UnitStat() {type = UnitStatType.Armor, currentValue = 0, maxValue = 0},
                                new UnitStat() {type = UnitStatType.MoveSpeed, currentValue = 5, maxValue = 5},
                            }
                        }
                    }
                }
            };
        }
    }

    public partial class UnitLevelConfig : ConfigData
    {
        public UnitStatsContainer stats;
    }
}