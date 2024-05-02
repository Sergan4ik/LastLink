using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class AnimationData : IUpdatableFrom<Game.GameCore.AnimationData>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.AnimationData)other;
            animationName = otherConcrete.animationName;
            duration = otherConcrete.duration;
            loop = otherConcrete.loop;
            timer.UpdateFrom(otherConcrete.timer, __helper);
        }
        public void UpdateFrom(Game.GameCore.AnimationData other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            animationName = reader.ReadString();
            duration = reader.ReadSingle();
            loop = reader.ReadBoolean();
            timer.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(animationName);
            writer.Write(duration);
            writer.Write(loop);
            timer.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1903294260;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (ulong)animationName.CalculateHash();
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)duration;
            hash += hash << 11; hash ^= hash >> 7;
            hash += loop ? 1u : 0u;
            hash += hash << 11; hash ^= hash >> 7;
            hash += timer.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  AnimationData() 
        {
            animationName = string.Empty;
            timer = new Game.GameCore.RTSTimerIntervals();
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.AnimationData)other;
            if (animationName != otherConcrete.animationName) CodeGenImplTools.LogCompError(__helper, "animationName", printer, otherConcrete.animationName, animationName);
            if (duration != otherConcrete.duration) CodeGenImplTools.LogCompError(__helper, "duration", printer, otherConcrete.duration, duration);
            if (loop != otherConcrete.loop) CodeGenImplTools.LogCompError(__helper, "loop", printer, otherConcrete.loop, loop);
            __helper.Push("timer");
            timer.CompareCheck(otherConcrete.timer, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "animationName":
                animationName = (string) reader.Value;
                break;
                case "duration":
                duration = (float)(double)reader.Value;
                break;
                case "loop":
                loop = (bool)reader.Value;
                break;
                case "timer":
                timer.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("animationName");
            writer.WriteValue(animationName);
            writer.WritePropertyName("duration");
            writer.WriteValue(duration);
            writer.WritePropertyName("loop");
            writer.WriteValue(loop);
            writer.WritePropertyName("timer");
            timer.WriteJson(writer);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.AnimationData;
        }
        public override System.Object NewInst() 
        {
        return new AnimationData();
        }
    }
}
#endif
