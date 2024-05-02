using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class GameConfig : IUpdatableFrom<Game.GameCore.GameConfig>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.GameConfig>, IJsonSerializable
    {
        public virtual void UpdateFrom(Game.GameCore.GameConfig other, ZRUpdateFromHelper __helper) 
        {
            units.UpdateFrom(other.units, __helper);
        }
        public virtual void Deserialize(ZRBinaryReader reader) 
        {
            units.Deserialize(reader);
        }
        public virtual void Serialize(ZRBinaryWriter writer) 
        {
            units.Serialize(writer);
        }
        public virtual ulong CalculateHash(ZRHashHelper __helper) 
        {
            System.UInt64 hash = 345093625;
            hash ^= (ulong)1214082341;
            hash += hash << 11; hash ^= hash >> 7;
            hash += units.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  GameConfig() 
        {
            units = new System.Collections.Generic.List<Game.GameCore.UnitConfig>();
        }
        public virtual void CompareCheck(Game.GameCore.GameConfig other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            __helper.Push("units");
            units.CompareCheck(other.units, __helper, printer);
            __helper.Pop();
        }
        public virtual bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            switch(__name)
            {
                case "units":
                units.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public virtual void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            writer.WritePropertyName("units");
            units.WriteJson(writer);
        }
    }
}
#endif
