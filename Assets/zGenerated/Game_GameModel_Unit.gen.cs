using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameModel {

    public partial class Unit : IUpdatableFrom<Game.GameModel.Unit>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameModel.Unit)other;
            engine.UpdateFrom(otherConcrete.engine, __helper);
            moveSpeed = otherConcrete.moveSpeed;
            transform.UpdateFrom(otherConcrete.transform, __helper);
        }
        public void UpdateFrom(Game.GameModel.Unit other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            engine.Deserialize(reader);
            moveSpeed = reader.ReadSingle();
            transform.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            engine.Serialize(writer);
            writer.Write(moveSpeed);
            transform.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1347601703;
            hash += hash << 11; hash ^= hash >> 7;
            hash += engine.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)moveSpeed;
            hash += hash << 11; hash ^= hash >> 7;
            hash += transform.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameModel.Unit)other;
            __helper.Push("engine");
            engine.CompareCheck(otherConcrete.engine, __helper, printer);
            __helper.Pop();
            if (moveSpeed != otherConcrete.moveSpeed) SerializationTools.LogCompError(__helper, "moveSpeed", printer, otherConcrete.moveSpeed, moveSpeed);
            __helper.Push("transform");
            transform.CompareCheck(otherConcrete.transform, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "engine":
                engine.ReadFromJson(reader);
                break;
                case "moveSpeed":
                moveSpeed = (float)(double)reader.Value;
                break;
                case "transform":
                transform.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("engine");
            engine.WriteJson(writer);
            writer.WritePropertyName("moveSpeed");
            writer.WriteValue(moveSpeed);
            writer.WritePropertyName("transform");
            transform.WriteJson(writer);
        }
        public  Unit() 
        {
            engine = new CoroutineEngine();
            transform = new Game.GameModel.RTSTransform();
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.Unit;
        }
        public override System.Object NewInst() 
        {
        return new Unit();
        }
    }
}
#endif
