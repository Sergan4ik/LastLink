using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitMove : IUpdatableFrom<Game.GameCore.UnitMove>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitMove)other;
            moveSpeed = otherConcrete.moveSpeed;
        }
        public void UpdateFrom(Game.GameCore.UnitMove other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            moveSpeed = reader.ReadSingle();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(moveSpeed);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1925159920;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)moveSpeed;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  UnitMove() 
        {

        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitMove)other;
            if (moveSpeed != otherConcrete.moveSpeed) SerializationTools.LogCompError(__helper, "moveSpeed", printer, otherConcrete.moveSpeed, moveSpeed);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "moveSpeed":
                moveSpeed = (float)(double)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("moveSpeed");
            writer.WriteValue(moveSpeed);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.UnitMove;
        }
        public override System.Object NewInst() 
        {
        return new UnitMove();
        }
    }
}
#endif
