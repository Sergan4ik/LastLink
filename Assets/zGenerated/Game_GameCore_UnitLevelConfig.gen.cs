using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitLevelConfig : IUpdatableFrom<Game.GameCore.UnitLevelConfig>, IUpdatableFrom<Game.GameCore.ConfigData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.ConfigData>, IJsonSerializable
    {
        public override void UpdateFrom(Game.GameCore.ConfigData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitLevelConfig)other;
            stats.UpdateFrom(otherConcrete.stats, __helper);
        }
        public void UpdateFrom(Game.GameCore.UnitLevelConfig other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.ConfigData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            stats.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            stats.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1738183399;
            hash += hash << 11; hash ^= hash >> 7;
            hash += stats.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.ConfigData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitLevelConfig)other;
            __helper.Push("stats");
            stats.CompareCheck(otherConcrete.stats, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "stats":
                stats.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("stats");
            stats.WriteJson(writer);
        }
        public  UnitLevelConfig() 
        {
            stats = new Game.GameCore.UnitStatsContainer();
        }
    }
}
#endif
