using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitConfig : IUpdatableFrom<Game.GameCore.UnitConfig>, IUpdatableFrom<Game.GameCore.ConfigData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.ConfigData>, IJsonSerializable
    {
        public override void UpdateFrom(Game.GameCore.ConfigData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitConfig)other;
            levelConfig.UpdateFrom(otherConcrete.levelConfig, __helper);
            name = otherConcrete.name;
        }
        public void UpdateFrom(Game.GameCore.UnitConfig other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.ConfigData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            levelConfig.Deserialize(reader);
            name = reader.ReadString();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            levelConfig.Serialize(writer);
            writer.Write(name);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)2124769659;
            hash += hash << 11; hash ^= hash >> 7;
            hash += levelConfig.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (ulong)name.CalculateHash();
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.ConfigData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitConfig)other;
            __helper.Push("levelConfig");
            levelConfig.CompareCheck(otherConcrete.levelConfig, __helper, printer);
            __helper.Pop();
            if (name != otherConcrete.name) SerializationTools.LogCompError(__helper, "name", printer, otherConcrete.name, name);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "levelConfig":
                levelConfig.ReadFromJson(reader);
                break;
                case "name":
                name = (string) reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("levelConfig");
            levelConfig.WriteJson(writer);
            writer.WritePropertyName("name");
            writer.WriteValue(name);
        }
        public  UnitConfig() 
        {
            levelConfig = new System.Collections.Generic.List<Game.GameCore.UnitLevelConfig>();
            name = string.Empty;
        }
    }
}
#endif
