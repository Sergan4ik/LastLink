using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class LogCommand : IUpdatableFrom<Game.GameCore.LogCommand>, IUpdatableFrom<ZeroLag.ZeroLagCommand>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.ZeroLagCommand>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(ZeroLag.ZeroLagCommand other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.LogCommand)other;
            message = otherConcrete.message;
        }
        public void UpdateFrom(Game.GameCore.LogCommand other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((ZeroLag.ZeroLagCommand)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            message = reader.ReadString();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(message);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1601007159;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (ulong)message.CalculateHash();
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  LogCommand() 
        {
            message = string.Empty;
        }
        public override void CompareCheck(ZeroLag.ZeroLagCommand other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.LogCommand)other;
            if (message != otherConcrete.message) CodeGenImplTools.LogCompError(__helper, "message", printer, otherConcrete.message, message);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "message":
                message = (string) reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("message");
            writer.WriteValue(message);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.LogCommand;
        }
        public override System.Object NewInst() 
        {
        return new LogCommand();
        }
    }
}
#endif
