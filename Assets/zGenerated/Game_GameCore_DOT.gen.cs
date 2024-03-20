using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class DOT : IUpdatableFrom<Game.GameCore.DOT>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.DOT)other;
            dps = otherConcrete.dps;
        }
        public void UpdateFrom(Game.GameCore.DOT other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            dps = reader.ReadSingle();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(dps);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)860487872;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)dps;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  DOT() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.DOT)other;
            if (dps != otherConcrete.dps) SerializationTools.LogCompError(__helper, "dps", printer, otherConcrete.dps, dps);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "dps":
                dps = (float)(double)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("dps");
            writer.WriteValue(dps);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.DOT;
        }
        public override System.Object NewInst() 
        {
        return new DOT();
        }
    }
}
#endif
