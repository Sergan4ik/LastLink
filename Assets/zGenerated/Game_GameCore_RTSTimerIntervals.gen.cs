using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSTimerIntervals : IUpdatableFrom<Game.GameCore.RTSTimerIntervals>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSTimerIntervals)other;
            currentInterval = otherConcrete.currentInterval;
            intervals.UpdateFrom(otherConcrete.intervals, __helper);
            loop = otherConcrete.loop;
            totalTimeElapsed = otherConcrete.totalTimeElapsed;
        }
        public void UpdateFrom(Game.GameCore.RTSTimerIntervals other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            currentInterval = reader.ReadInt32();
            intervals.Deserialize(reader);
            loop = reader.ReadBoolean();
            totalTimeElapsed = reader.ReadSingle();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(currentInterval);
            intervals.Serialize(writer);
            writer.Write(loop);
            writer.Write(totalTimeElapsed);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)14893346;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)currentInterval;
            hash += hash << 11; hash ^= hash >> 7;
            hash += intervals.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += loop ? 1u : 0u;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)totalTimeElapsed;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSTimerIntervals)other;
            if (currentInterval != otherConcrete.currentInterval) SerializationTools.LogCompError(__helper, "currentInterval", printer, otherConcrete.currentInterval, currentInterval);
            __helper.Push("intervals");
            intervals.CompareCheck(otherConcrete.intervals, __helper, printer);
            __helper.Pop();
            if (loop != otherConcrete.loop) SerializationTools.LogCompError(__helper, "loop", printer, otherConcrete.loop, loop);
            if (totalTimeElapsed != otherConcrete.totalTimeElapsed) SerializationTools.LogCompError(__helper, "totalTimeElapsed", printer, otherConcrete.totalTimeElapsed, totalTimeElapsed);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "currentInterval":
                currentInterval = (int)(Int64)reader.Value;
                break;
                case "intervals":
                intervals.ReadFromJson(reader);
                break;
                case "loop":
                loop = (bool)reader.Value;
                break;
                case "totalTimeElapsed":
                totalTimeElapsed = (float)(double)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("currentInterval");
            writer.WriteValue(currentInterval);
            writer.WritePropertyName("intervals");
            intervals.WriteJson(writer);
            writer.WritePropertyName("loop");
            writer.WriteValue(loop);
            writer.WritePropertyName("totalTimeElapsed");
            writer.WriteValue(totalTimeElapsed);
        }
        public  RTSTimerIntervals() 
        {
            intervals = new System.Collections.Generic.List<float>();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSTimerIntervals;
        }
        public override System.Object NewInst() 
        {
        return new RTSTimerIntervals();
        }
    }
}
#endif
