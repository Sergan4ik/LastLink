using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class GameModel : IUpdatableFrom<Game.GameCore.GameModel>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.GameModel>, IJsonSerializable
    {
        public virtual void UpdateFrom(Game.GameCore.GameModel other, ZRUpdateFromHelper __helper) 
        {
            controlData.UpdateFrom(other.controlData, __helper);
            factions.UpdateFrom(other.factions, __helper);
            gameState.value = other.gameState.value;
            random.UpdateFrom(other.random, __helper);
            stopWatch.UpdateFrom(other.stopWatch, __helper);
            units.UpdateFrom(other.units, __helper);
        }
        public virtual void Deserialize(ZRBinaryReader reader) 
        {
            controlData.Deserialize(reader);
            factions.Deserialize(reader);
            gameState.value = reader.ReadEnum<Game.GameCore.GameState>();
            random.Deserialize(reader);
            stopWatch.Deserialize(reader);
            units.Deserialize(reader);
        }
        public virtual void Serialize(ZRBinaryWriter writer) 
        {
            controlData.Serialize(writer);
            factions.Serialize(writer);
            writer.Write((Int32)gameState.value);
            random.Serialize(writer);
            stopWatch.Serialize(writer);
            units.Serialize(writer);
        }
        public virtual ulong CalculateHash(ZRHashHelper __helper) 
        {
            System.UInt64 hash = 345093625;
            hash ^= (ulong)624014821;
            hash += hash << 11; hash ^= hash >> 7;
            hash += controlData.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += factions.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)gameState.value;
            hash += hash << 11; hash ^= hash >> 7;
            hash += random.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += stopWatch.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += units.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  GameModel() 
        {
            controlData = new System.Collections.Generic.List<Game.GameCore.ControlData>();
            factions = new ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction>();
            gameState = new ZergRush.ReactiveCore.Cell<Game.GameCore.GameState>();
            random = new ZergRush.ZergRandom();
            stopWatch = new Game.GameCore.RTSStopWatch();
            units = new ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit>();
        }
        public virtual void CompareCheck(Game.GameCore.GameModel other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            __helper.Push("controlData");
            controlData.CompareCheck(other.controlData, __helper, printer);
            __helper.Pop();
            __helper.Push("factions");
            factions.CompareCheck(other.factions, __helper, printer);
            __helper.Pop();
            if (gameState.value != other.gameState.value) SerializationTools.LogCompError(__helper, "gameState", printer, other.gameState.value, gameState.value);
            __helper.Push("random");
            random.CompareCheck(other.random, __helper, printer);
            __helper.Pop();
            __helper.Push("stopWatch");
            stopWatch.CompareCheck(other.stopWatch, __helper, printer);
            __helper.Pop();
            __helper.Push("units");
            units.CompareCheck(other.units, __helper, printer);
            __helper.Pop();
        }
        public virtual bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            switch(__name)
            {
                case "controlData":
                controlData.ReadFromJson(reader);
                break;
                case "factions":
                factions.ReadFromJson(reader);
                break;
                case "gameState":
                gameState.value = ((string)reader.Value).ParseEnum<Game.GameCore.GameState>();
                break;
                case "random":
                random.ReadFromJson(reader);
                break;
                case "stopWatch":
                stopWatch.ReadFromJson(reader);
                break;
                case "units":
                units.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public virtual void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            writer.WritePropertyName("controlData");
            controlData.WriteJson(writer);
            writer.WritePropertyName("factions");
            factions.WriteJson(writer);
            writer.WritePropertyName("gameState");
            writer.WriteValue(gameState.value.ToString());
            writer.WritePropertyName("random");
            random.WriteJson(writer);
            writer.WritePropertyName("stopWatch");
            stopWatch.WriteJson(writer);
            writer.WritePropertyName("units");
            units.WriteJson(writer);
        }
    }
}
#endif
