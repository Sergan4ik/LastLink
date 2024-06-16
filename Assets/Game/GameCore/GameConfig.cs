using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZergRush;
using ZergRush.Alive;
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
                    UnitConfig.CreateBaseUnit(),
                    UnitConfig.CreateBaseUnit("DefaultEnemy")
                }
            };
        }

        public static void ResetConfigs()
        {
            foreach (var path in savePaths)
            {
                File.Delete(path);
            }
            
            CreateDefault().Save();
        }
        
        public void Save()
        {
            string json = this.WriteToJsonString();
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
        public List<AnimationData> customAnimations;
        public AnimationData walkAnimation;
        public AnimationData idleAnimation;
        public AnimationData autoAttackAnimation;
        public AnimationData deathAnimation;
        
        public UnitLevelConfig GetLevelConfig(int level)
        {
            return levelConfig[level];
        }

        public static UnitConfig CreateBaseUnit(string name = "DefaultUnit")
        {
            return new UnitConfig()
            {
                name = name,
                levelConfig = new List<UnitLevelConfig>()
                {
                    new UnitLevelConfig()
                    {
                        stats = new UnitStatsContainer()
                        {
                            stats = new List<UnitStat>()
                            {
                                new UnitStat() { type = UnitStatType.Health, currentValue = 100, maxValue = 100 },
                                new UnitStat() { type = UnitStatType.Mana, currentValue = 100, maxValue = 100 },
                                new UnitStat() { type = UnitStatType.Armor, currentValue = 0, maxValue = 0 },
                                new UnitStat() { type = UnitStatType.MoveSpeed, currentValue = 5, maxValue = 5 },
                                new UnitStat() { type = UnitStatType.RotationSpeed, currentValue = 720, maxValue = 720 }
                            }
                        }
                    }
                },
                customAnimations = new List<AnimationData>()
                {
                    new AnimationData()
                    {
                        animationName = "Attack",
                        duration = 0.5f,
                        loop = false,
                    }
                },
                walkAnimation = new AnimationData()
                {
                    animationName = "Run_Forward",
                    duration = 0.667f,
                    loop = true,
                },
                idleAnimation = new AnimationData()
                {
                    animationName = "Idle01",
                    duration = name == "DefaultUnit" ? 2.233f : 2.267f,
                    loop = true,
                },
                autoAttackAnimation = new AnimationData()
                {
                    animationName = "Combat_Cast_Attack",
                    duration = 1,
                    loop = false
                },
                deathAnimation = new AnimationData()
                {
                    animationName = "Death",
                    duration = (name == "DefaultUnit" ? 57f : 58f )/60f,
                    loop = false
                }
            };
        }
    }

    public partial class UnitLevelConfig : ConfigData
    {
        public UnitStatsContainer stats;
    }
}