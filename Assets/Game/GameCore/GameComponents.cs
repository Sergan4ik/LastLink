using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameCore
{
    public partial class RTSTransform : RTSRuntimeData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public enum UnitStatType
    {
        Health,
        Mana,
        Armor,
        MoveSpeed,
        RotationSpeed
    }
    
    public partial class Stat<TStat> where TStat : Enum
    {
        public TStat type;
        public float currentValue;
        public float maxValue = -1;
    }
    
    public partial class UnitStat : Stat<UnitStatType> {}

    public partial class UnitStatsContainer : RTSRuntimeData
    {
        public List<UnitStat> stats;
        
        public float GetStatValue(UnitStatType statType)
        {
            return stats.Find(s => s.type == statType).currentValue;
        }
        
        public ref float GetStatRef(UnitStatType statType)
        {
            return ref stats.Find(s => s.type == statType).currentValue;
        }

        public UnitStat GetStat(UnitStatType statType)
        {
            return stats.Find(s => s.type == statType);
        }
        
        public ref float Health => ref GetStatRef(UnitStatType.Health);
        public float MaxHealth => GetStat(UnitStatType.Health).maxValue;
        public ref float Mana => ref GetStatRef(UnitStatType.Mana);
        public ref float MoveSpeed => ref GetStatRef(UnitStatType.MoveSpeed);
        public float MaxMana => GetStat(UnitStatType.Mana).maxValue;
        public ref float RotationSpeed => ref GetStatRef(UnitStatType.RotationSpeed);

        public static UnitStatsContainer CreateDefault()
        {
            return new UnitStatsContainer()
            {
                stats = new List<UnitStat>()
                {
                    new UnitStat() {type = UnitStatType.Health, currentValue = 100, maxValue = 100},
                    new UnitStat() {type = UnitStatType.Mana, currentValue = 100, maxValue = 100},
                    new UnitStat() {type = UnitStatType.Armor, currentValue = 0},
                    new UnitStat() {type = UnitStatType.MoveSpeed, currentValue = 5}
                }
            };
        }
    }
}