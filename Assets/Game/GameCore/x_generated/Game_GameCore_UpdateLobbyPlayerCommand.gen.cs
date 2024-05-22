using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UpdateLobbyPlayerCommand : IUpdatableFrom<Game.GameCore.UpdateLobbyPlayerCommand>, IUpdatableFrom<ZeroLag.ZeroLagCommand>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.ZeroLagCommand>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(ZeroLag.ZeroLagCommand other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UpdateLobbyPlayerCommand)other;
            factionSlot = otherConcrete.factionSlot;
            factionType = otherConcrete.factionType;
            globalPlayerId = otherConcrete.globalPlayerId;
        }
        public void UpdateFrom(Game.GameCore.UpdateLobbyPlayerCommand other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((ZeroLag.ZeroLagCommand)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            factionSlot = reader.ReadEnum<Game.GameCore.FactionSlot>();
            factionType = reader.ReadEnum<Game.GameCore.FactionType>();
            globalPlayerId = reader.ReadInt64();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((Int32)factionSlot);
            writer.Write((Int32)factionType);
            writer.Write(globalPlayerId);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1313752215;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionSlot;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionType;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)globalPlayerId;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  UpdateLobbyPlayerCommand() 
        {

        }
        public override void CompareCheck(ZeroLag.ZeroLagCommand other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UpdateLobbyPlayerCommand)other;
            if (factionSlot != otherConcrete.factionSlot) CodeGenImplTools.LogCompError(__helper, "factionSlot", printer, otherConcrete.factionSlot, factionSlot);
            if (factionType != otherConcrete.factionType) CodeGenImplTools.LogCompError(__helper, "factionType", printer, otherConcrete.factionType, factionType);
            if (globalPlayerId != otherConcrete.globalPlayerId) CodeGenImplTools.LogCompError(__helper, "globalPlayerId", printer, otherConcrete.globalPlayerId, globalPlayerId);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "factionSlot":
                factionSlot = ((string)reader.Value).ParseEnum<Game.GameCore.FactionSlot>();
                break;
                case "factionType":
                factionType = ((string)reader.Value).ParseEnum<Game.GameCore.FactionType>();
                break;
                case "globalPlayerId":
                globalPlayerId = (long)(Int64)reader.Value;
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
            writer.WritePropertyName("factionType");
            writer.WriteValue(factionType.ToString());
            writer.WritePropertyName("globalPlayerId");
            writer.WriteValue(globalPlayerId);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.UpdateLobbyPlayerCommand;
        }
        public override System.Object NewInst() 
        {
        return new UpdateLobbyPlayerCommand();
        }
    }
}
#endif
