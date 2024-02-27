using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class GameModel : IUpdatableFrom<Game.GameCore.GameModel>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.GameModel)other;
            controlData.UpdateFrom(otherConcrete.controlData, __helper);
            factions.UpdateFrom(otherConcrete.factions, __helper);
            gameState.value = otherConcrete.gameState.value;
            random.UpdateFrom(otherConcrete.random, __helper);
        }
        public void UpdateFrom(Game.GameCore.GameModel other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            controlData.Deserialize(reader);
            factions.Deserialize(reader);
            gameState.value = reader.ReadEnum<Game.GameCore.GameState>();
            random.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            controlData.Serialize(writer);
            factions.Serialize(writer);
            writer.Write((Int32)gameState.value);
            random.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
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
            return hash;
        }
        public  GameModel() 
        {
            controlData = new System.Collections.Generic.List<Game.GameCore.ControlData>();
            factions = new ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction>();
            gameState = new ZergRush.ReactiveCore.Cell<Game.GameCore.GameState>();
            random = new ZergRush.ZergRandom();
        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.GameModel)other;
            __helper.Push("controlData");
            controlData.CompareCheck(otherConcrete.controlData, __helper, printer);
            __helper.Pop();
            __helper.Push("factions");
            factions.CompareCheck(otherConcrete.factions, __helper, printer);
            __helper.Pop();
            if (gameState.value != otherConcrete.gameState.value) SerializationTools.LogCompError(__helper, "gameState", printer, otherConcrete.gameState.value, gameState.value);
            __helper.Push("random");
            random.CompareCheck(otherConcrete.random, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
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
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("controlData");
            controlData.WriteJson(writer);
            writer.WritePropertyName("factions");
            factions.WriteJson(writer);
            writer.WritePropertyName("gameState");
            writer.WriteValue(gameState.value.ToString());
            writer.WritePropertyName("random");
            random.WriteJson(writer);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.GameModel;
        }
        public override System.Object NewInst() 
        {
        return new GameModel();
        }
    }
}
#endif
