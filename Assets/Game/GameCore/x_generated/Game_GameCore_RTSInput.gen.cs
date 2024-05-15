using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSInput : IUpdatableFrom<Game.GameCore.RTSInput>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSInput)other;
            inputType = otherConcrete.inputType;
            inputTypeVariation = otherConcrete.inputTypeVariation;
            targetData.UpdateFrom(otherConcrete.targetData, __helper);
        }
        public void UpdateFrom(Game.GameCore.RTSInput other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            inputType = reader.ReadEnum<Game.GameCore.RTSInputType>();
            inputTypeVariation = reader.ReadInt32();
            targetData.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((Int32)inputType);
            writer.Write(inputTypeVariation);
            targetData.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)978946045;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)inputType;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)inputTypeVariation;
            hash += hash << 11; hash ^= hash >> 7;
            hash += targetData.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.RTSInput)other;
            if (inputType != otherConcrete.inputType) CodeGenImplTools.LogCompError(__helper, "inputType", printer, otherConcrete.inputType, inputType);
            if (inputTypeVariation != otherConcrete.inputTypeVariation) CodeGenImplTools.LogCompError(__helper, "inputTypeVariation", printer, otherConcrete.inputTypeVariation, inputTypeVariation);
            __helper.Push("targetData");
            targetData.CompareCheck(otherConcrete.targetData, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "inputType":
                inputType = ((string)reader.Value).ParseEnum<Game.GameCore.RTSInputType>();
                break;
                case "inputTypeVariation":
                inputTypeVariation = (int)(Int64)reader.Value;
                break;
                case "targetData":
                targetData.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("inputType");
            writer.WriteValue(inputType.ToString());
            writer.WritePropertyName("inputTypeVariation");
            writer.WriteValue(inputTypeVariation);
            writer.WritePropertyName("targetData");
            targetData.WriteJson(writer);
        }
        public  RTSInput() 
        {
            targetData = new Game.GameCore.TargetData();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSInput;
        }
        public override System.Object NewInst() 
        {
        return new RTSInput();
        }
    }
}
#endif
