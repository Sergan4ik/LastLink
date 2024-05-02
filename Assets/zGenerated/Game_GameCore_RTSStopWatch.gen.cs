using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSStopWatch : IUpdatableFrom<Game.GameCore.RTSStopWatch>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSStopWatch)other;
            circles.UpdateFrom(otherConcrete.circles, __helper);
            elapsedTime = otherConcrete.elapsedTime;
            isPaused = otherConcrete.isPaused;
        }
        public void UpdateFrom(Game.GameCore.RTSStopWatch other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            circles.Deserialize(reader);
            elapsedTime = reader.ReadSingle();
            isPaused = reader.ReadBoolean();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            circles.Serialize(writer);
            writer.Write(elapsedTime);
            writer.Write(isPaused);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)919478866;
            hash += hash << 11; hash ^= hash >> 7;
            hash += circles.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)elapsedTime;
            hash += hash << 11; hash ^= hash >> 7;
            hash += isPaused ? 1u : 0u;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSStopWatch)other;
            __helper.Push("circles");
            circles.CompareCheck(otherConcrete.circles, __helper, printer);
            __helper.Pop();
            if (elapsedTime != otherConcrete.elapsedTime) CodeGenImplTools.LogCompError(__helper, "elapsedTime", printer, otherConcrete.elapsedTime, elapsedTime);
            if (isPaused != otherConcrete.isPaused) CodeGenImplTools.LogCompError(__helper, "isPaused", printer, otherConcrete.isPaused, isPaused);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "circles":
                circles.ReadFromJson(reader);
                break;
                case "elapsedTime":
                elapsedTime = (float)(double)reader.Value;
                break;
                case "isPaused":
                isPaused = (bool)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("circles");
            circles.WriteJson(writer);
            writer.WritePropertyName("elapsedTime");
            writer.WriteValue(elapsedTime);
            writer.WritePropertyName("isPaused");
            writer.WriteValue(isPaused);
        }
        public  RTSStopWatch() 
        {
            circles = new System.Collections.Generic.List<float>();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSStopWatch;
        }
        public override System.Object NewInst() 
        {
        return new RTSStopWatch();
        }
    }
}
#endif
