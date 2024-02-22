using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class SelectionRectClipSpace : IUpdatableFrom<Game.GameCore.SelectionRectClipSpace>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.SelectionRectClipSpace)other;
            cameraSize.UpdateFrom(otherConcrete.cameraSize, __helper);
            leftBottom.UpdateFrom(otherConcrete.leftBottom, __helper);
            rightTop.UpdateFrom(otherConcrete.rightTop, __helper);
            unitToViewportMatrix.UpdateFrom(otherConcrete.unitToViewportMatrix, __helper);
        }
        public void UpdateFrom(Game.GameCore.SelectionRectClipSpace other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            cameraSize = reader.ReadUnityEngine_Vector2();
            leftBottom = reader.ReadUnityEngine_Vector2();
            rightTop = reader.ReadUnityEngine_Vector2();
            unitToViewportMatrix = reader.ReadUnityEngine_Matrix4x4();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            cameraSize.Serialize(writer);
            leftBottom.Serialize(writer);
            rightTop.Serialize(writer);
            unitToViewportMatrix.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)231248435;
            hash += hash << 11; hash ^= hash >> 7;
            hash += cameraSize.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += leftBottom.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += rightTop.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += unitToViewportMatrix.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  SelectionRectClipSpace() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.SelectionRectClipSpace)other;
            __helper.Push("cameraSize");
            cameraSize.CompareCheck(otherConcrete.cameraSize, __helper, printer);
            __helper.Pop();
            __helper.Push("leftBottom");
            leftBottom.CompareCheck(otherConcrete.leftBottom, __helper, printer);
            __helper.Pop();
            __helper.Push("rightTop");
            rightTop.CompareCheck(otherConcrete.rightTop, __helper, printer);
            __helper.Pop();
            __helper.Push("unitToViewportMatrix");
            unitToViewportMatrix.CompareCheck(otherConcrete.unitToViewportMatrix, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "cameraSize":
                cameraSize = (UnityEngine.Vector2)reader.ReadFromJsonUnityEngine_Vector2();
                break;
                case "leftBottom":
                leftBottom = (UnityEngine.Vector2)reader.ReadFromJsonUnityEngine_Vector2();
                break;
                case "rightTop":
                rightTop = (UnityEngine.Vector2)reader.ReadFromJsonUnityEngine_Vector2();
                break;
                case "unitToViewportMatrix":
                unitToViewportMatrix = (UnityEngine.Matrix4x4)reader.ReadFromJsonUnityEngine_Matrix4x4();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("cameraSize");
            cameraSize.WriteJson(writer);
            writer.WritePropertyName("leftBottom");
            leftBottom.WriteJson(writer);
            writer.WritePropertyName("rightTop");
            rightTop.WriteJson(writer);
            writer.WritePropertyName("unitToViewportMatrix");
            unitToViewportMatrix.WriteJson(writer);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.SelectionRectClipSpace;
        }
        public override System.Object NewInst() 
        {
        return new SelectionRectClipSpace();
        }
    }
}
#endif
