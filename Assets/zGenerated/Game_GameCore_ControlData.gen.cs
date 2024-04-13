using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class ControlData : IUpdatableFrom<Game.GameCore.ControlData>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.ControlData)other;
            factionSlot = otherConcrete.factionSlot;
            serverPlayerId = otherConcrete.serverPlayerId;
        }
        public void UpdateFrom(Game.GameCore.ControlData other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            factionSlot = reader.ReadEnum<Game.GameCore.FactionSlot>();
            serverPlayerId = reader.ReadInt16();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((Int32)factionSlot);
            writer.Write(serverPlayerId);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1434085133;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionSlot;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)serverPlayerId;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  ControlData() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.ControlData)other;
            if (factionSlot != otherConcrete.factionSlot) SerializationTools.LogCompError(__helper, "factionSlot", printer, otherConcrete.factionSlot, factionSlot);
            if (serverPlayerId != otherConcrete.serverPlayerId) SerializationTools.LogCompError(__helper, "serverPlayerId", printer, otherConcrete.serverPlayerId, serverPlayerId);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "factionSlot":
                factionSlot = ((string)reader.Value).ParseEnum<Game.GameCore.FactionSlot>();
                break;
                case "serverPlayerId":
                serverPlayerId = (short)(Int64)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("factionSlot");
            writer.WriteValue(factionSlot.ToString());
            writer.WritePropertyName("serverPlayerId");
            writer.WriteValue(serverPlayerId);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.ControlData;
        }
        public override System.Object NewInst() 
        {
        return new ControlData();
        }
    }
}
#endif
