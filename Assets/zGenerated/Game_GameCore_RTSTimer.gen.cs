using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSTimer : IUpdatableFrom<Game.GameCore.RTSTimer>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSTimer)other;
            elapsedTime = otherConcrete.elapsedTime;
            state = otherConcrete.state;
        }
        public void UpdateFrom(Game.GameCore.RTSTimer other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            elapsedTime = reader.ReadSingle();
            state = reader.ReadEnum<Game.GameCore.TimerState>();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(elapsedTime);
            writer.Write((Int32)state);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash += (System.UInt64)elapsedTime;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)state;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  RTSTimer() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSTimer)other;
            if (elapsedTime != otherConcrete.elapsedTime) CodeGenImplTools.LogCompError(__helper, "elapsedTime", printer, otherConcrete.elapsedTime, elapsedTime);
            if (state != otherConcrete.state) CodeGenImplTools.LogCompError(__helper, "state", printer, otherConcrete.state, state);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "elapsedTime":
                elapsedTime = (float)(double)reader.Value;
                break;
                case "state":
                state = ((string)reader.Value).ParseEnum<Game.GameCore.TimerState>();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("elapsedTime");
            writer.WriteValue(elapsedTime);
            writer.WritePropertyName("state");
            writer.WriteValue(state.ToString());
        }
        public override System.Object NewInst() 
        {
        throw new NotImplementedException();
        }
    }
}
#endif
