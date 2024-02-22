using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSTransform : IUpdatableFrom<Game.GameCore.RTSTransform>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSTransform)other;
            position.UpdateFrom(otherConcrete.position, __helper);
            rotation.UpdateFrom(otherConcrete.rotation, __helper);
        }
        public void UpdateFrom(Game.GameCore.RTSTransform other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            position = reader.ReadUnityEngine_Vector3();
            rotation = reader.ReadUnityEngine_Quaternion();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            position.Serialize(writer);
            rotation.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)615361206;
            hash += hash << 11; hash ^= hash >> 7;
            hash += position.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += rotation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSTransform)other;
            __helper.Push("position");
            position.CompareCheck(otherConcrete.position, __helper, printer);
            __helper.Pop();
            __helper.Push("rotation");
            rotation.CompareCheck(otherConcrete.rotation, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "position":
                position = (UnityEngine.Vector3)reader.ReadFromJsonUnityEngine_Vector3();
                break;
                case "rotation":
                rotation = (UnityEngine.Quaternion)reader.ReadFromJsonUnityEngine_Quaternion();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("position");
            position.WriteJson(writer);
            writer.WritePropertyName("rotation");
            rotation.WriteJson(writer);
        }
        public  RTSTransform() 
        {

        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSTransform;
        }
        public override System.Object NewInst() 
        {
        return new RTSTransform();
        }
    }
}
#endif
