using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitAction : IUpdatableFrom<Game.GameCore.UnitAction>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitAction)other;
            duration = otherConcrete.duration;
            preparingTime = otherConcrete.preparingTime;
            state = otherConcrete.state;
            stateTimer.UpdateFrom(otherConcrete.stateTimer, __helper);
        }
        public void UpdateFrom(Game.GameCore.UnitAction other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            duration = reader.ReadSingle();
            preparingTime = reader.ReadSingle();
            state = reader.ReadEnum<Game.GameCore.ActionState>();
            stateTimer.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(duration);
            writer.Write(preparingTime);
            writer.Write((Int32)state);
            stateTimer.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1110380895;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)duration;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)preparingTime;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)state;
            hash += hash << 11; hash ^= hash >> 7;
            hash += stateTimer.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitAction)other;
            if (duration != otherConcrete.duration) SerializationTools.LogCompError(__helper, "duration", printer, otherConcrete.duration, duration);
            if (preparingTime != otherConcrete.preparingTime) SerializationTools.LogCompError(__helper, "preparingTime", printer, otherConcrete.preparingTime, preparingTime);
            if (state != otherConcrete.state) SerializationTools.LogCompError(__helper, "state", printer, otherConcrete.state, state);
            __helper.Push("stateTimer");
            stateTimer.CompareCheck(otherConcrete.stateTimer, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "duration":
                duration = (float)(double)reader.Value;
                break;
                case "preparingTime":
                preparingTime = (float)(double)reader.Value;
                break;
                case "state":
                state = ((string)reader.Value).ParseEnum<Game.GameCore.ActionState>();
                break;
                case "stateTimer":
                stateTimer.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("duration");
            writer.WriteValue(duration);
            writer.WritePropertyName("preparingTime");
            writer.WriteValue(preparingTime);
            writer.WritePropertyName("state");
            writer.WriteValue(state.ToString());
            writer.WritePropertyName("stateTimer");
            stateTimer.WriteJson(writer);
        }
        public  UnitAction() 
        {
            stateTimer = new Game.GameCore.RTSTimerIntervals();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.UnitAction;
        }
        public override System.Object NewInst() 
        {
        return new UnitAction();
        }
    }
}
#endif
