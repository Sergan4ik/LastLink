using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.NodeArchitecture {

    public partial class ContextRoot : IUpdatableFrom<Game.NodeArchitecture.ContextRoot>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.NodeArchitecture.ContextRoot)other;
            nodeCounter = otherConcrete.nodeCounter;
        }
        public void UpdateFrom(Game.NodeArchitecture.ContextRoot other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            nodeCounter = reader.ReadInt32();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(nodeCounter);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1157793690;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)nodeCounter;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  ContextRoot() 
        {

        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.NodeArchitecture.ContextRoot)other;
            if (nodeCounter != otherConcrete.nodeCounter) SerializationTools.LogCompError(__helper, "nodeCounter", printer, otherConcrete.nodeCounter, nodeCounter);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "nodeCounter":
                nodeCounter = (int)(Int64)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("nodeCounter");
            writer.WriteValue(nodeCounter);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.ContextRoot;
        }
        public override System.Object NewInst() 
        {
        return new ContextRoot();
        }
    }
}
#endif
