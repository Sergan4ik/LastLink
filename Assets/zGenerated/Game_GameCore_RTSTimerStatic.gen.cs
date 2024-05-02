using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSTimerStatic : IUpdatableFrom<Game.GameCore.RTSTimerStatic>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSTimerStatic)other;
            staticCycleTime = otherConcrete.staticCycleTime;
        }
        public void UpdateFrom(Game.GameCore.RTSTimerStatic other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            staticCycleTime = reader.ReadSingle();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(staticCycleTime);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)125004293;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)staticCycleTime;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  RTSTimerStatic() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSTimerStatic)other;
            if (staticCycleTime != otherConcrete.staticCycleTime) CodeGenImplTools.LogCompError(__helper, "staticCycleTime", printer, otherConcrete.staticCycleTime, staticCycleTime);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "staticCycleTime":
                staticCycleTime = (float)(double)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("staticCycleTime");
            writer.WriteValue(staticCycleTime);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSTimerStatic;
        }
        public override System.Object NewInst() 
        {
        return new RTSTimerStatic();
        }
    }
}
#endif
