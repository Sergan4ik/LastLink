using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class CancelReadyCommand : IUpdatableFrom<Game.GameCore.CancelReadyCommand>, IUpdatableFrom<ZeroLag.ZeroLagCommand>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.ZeroLagCommand>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(ZeroLag.ZeroLagCommand other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.CancelReadyCommand)other;
            globalPlayerId = otherConcrete.globalPlayerId;
        }
        public void UpdateFrom(Game.GameCore.CancelReadyCommand other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((ZeroLag.ZeroLagCommand)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            globalPlayerId = reader.ReadInt64();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(globalPlayerId);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)279928293;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)globalPlayerId;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  CancelReadyCommand() 
        {

        }
        public override void CompareCheck(ZeroLag.ZeroLagCommand other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.CancelReadyCommand)other;
            if (globalPlayerId != otherConcrete.globalPlayerId) CodeGenImplTools.LogCompError(__helper, "globalPlayerId", printer, otherConcrete.globalPlayerId, globalPlayerId);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
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
            writer.WritePropertyName("globalPlayerId");
            writer.WriteValue(globalPlayerId);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.CancelReadyCommand;
        }
        public override System.Object NewInst() 
        {
        return new CancelReadyCommand();
        }
    }
}
#endif
