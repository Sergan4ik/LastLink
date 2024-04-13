using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class TargetData : IUpdatableFrom<Game.GameCore.TargetData>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.TargetData)other;
            sourceIds.UpdateFrom(otherConcrete.sourceIds, __helper);
            targetId = otherConcrete.targetId;
            worldPosition.UpdateFrom(otherConcrete.worldPosition, __helper);
        }
        public void UpdateFrom(Game.GameCore.TargetData other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            sourceIds.Deserialize(reader);
            targetId = reader.ReadInt32();
            worldPosition = reader.ReadUnityEngine_Vector3();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            sourceIds.Serialize(writer);
            writer.Write(targetId);
            worldPosition.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1420404676;
            hash += hash << 11; hash ^= hash >> 7;
            hash += sourceIds.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)targetId;
            hash += hash << 11; hash ^= hash >> 7;
            hash += worldPosition.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.TargetData)other;
            __helper.Push("sourceIds");
            sourceIds.CompareCheck(otherConcrete.sourceIds, __helper, printer);
            __helper.Pop();
            if (targetId != otherConcrete.targetId) SerializationTools.LogCompError(__helper, "targetId", printer, otherConcrete.targetId, targetId);
            __helper.Push("worldPosition");
            worldPosition.CompareCheck(otherConcrete.worldPosition, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "sourceIds":
                sourceIds.ReadFromJson(reader);
                break;
                case "targetId":
                targetId = (int)(Int64)reader.Value;
                break;
                case "worldPosition":
                worldPosition = (UnityEngine.Vector3)reader.ReadFromJsonUnityEngine_Vector3();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("sourceIds");
            sourceIds.WriteJson(writer);
            writer.WritePropertyName("targetId");
            writer.WriteValue(targetId);
            writer.WritePropertyName("worldPosition");
            worldPosition.WriteJson(writer);
        }
        public  TargetData() 
        {
            sourceIds = new System.Collections.Generic.List<int>();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.TargetData;
        }
        public override System.Object NewInst() 
        {
        return new TargetData();
        }
    }
}
#endif
