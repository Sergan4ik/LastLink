using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameModel {

    public partial class RTSTransform : IUpdatableFrom<Game.GameModel.RTSTransform>, IUpdatableFrom<Game.GameModel.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameModel.RTSRuntimeData>, IJsonSerializable
    {
        public override void UpdateFrom(Game.GameModel.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameModel.RTSTransform)other;
            var __position = position.value;
            __position.UpdateFrom(otherConcrete.position.value, __helper);
            position.value = __position;
            var __rotation = rotation.value;
            __rotation.UpdateFrom(otherConcrete.rotation.value, __helper);
            rotation.value = __rotation;
        }
        public void UpdateFrom(Game.GameModel.RTSTransform other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameModel.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            position.value = reader.ReadUnityEngine_Vector3();
            rotation.value = reader.ReadUnityEngine_Quaternion();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            position.value.Serialize(writer);
            rotation.value.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)615361206;
            hash += hash << 11; hash ^= hash >> 7;
            hash += position.value.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += rotation.value.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameModel.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameModel.RTSTransform)other;
            __helper.Push("position");
            position.value.CompareCheck(otherConcrete.position.value, __helper, printer);
            __helper.Pop();
            __helper.Push("rotation");
            rotation.value.CompareCheck(otherConcrete.rotation.value, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "position":
                position.value = (UnityEngine.Vector3)reader.ReadFromJsonUnityEngine_Vector3();
                break;
                case "rotation":
                rotation.value = (UnityEngine.Quaternion)reader.ReadFromJsonUnityEngine_Quaternion();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("position");
            position.value.WriteJson(writer);
            writer.WritePropertyName("rotation");
            rotation.value.WriteJson(writer);
        }
        public  RTSTransform() 
        {
            position = new ZergRush.ReactiveCore.Cell<UnityEngine.Vector3>();
            rotation = new ZergRush.ReactiveCore.Cell<UnityEngine.Quaternion>();
        }
    }
}
#endif
