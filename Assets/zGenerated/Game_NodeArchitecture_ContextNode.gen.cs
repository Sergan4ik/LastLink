using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.NodeArchitecture {

    public partial class ContextNode : IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public enum Types : ushort
        {
            ContextNode = 1,
            RTSContextNode = 2,
            RTSContextRoot = 3,
            ContextRoot = 4,
        }
        static Func<ContextNode> [] polymorphConstructors = new Func<ContextNode> [] {
            () => null, // 0
            () => new Game.NodeArchitecture.ContextNode(), // 1
            () => new Game.GameCore.RTSContextNode(), // 2
            () => new Game.GameCore.RTSContextRoot(), // 3
            () => new Game.NodeArchitecture.ContextRoot(), // 4
        };
        public static ContextNode CreatePolymorphic(System.UInt16 typeId) {
            return polymorphConstructors[typeId]();
        }
        public virtual void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            childs.UpdateFrom(other.childs, __helper);
            nodeId = other.nodeId;
        }
        public virtual void Deserialize(ZRBinaryReader reader) 
        {
            childs.Deserialize(reader);
            nodeId = reader.ReadInt32();
        }
        public virtual void Serialize(ZRBinaryWriter writer) 
        {
            childs.Serialize(writer);
            writer.Write(nodeId);
        }
        public virtual ulong CalculateHash(ZRHashHelper __helper) 
        {
            System.UInt64 hash = 345093625;
            hash ^= (ulong)260215107;
            hash += hash << 11; hash ^= hash >> 7;
            hash += childs.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)nodeId;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  ContextNode() 
        {
            childs = new System.Collections.Generic.List<int>();
        }
        public virtual void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            __helper.Push("childs");
            childs.CompareCheck(other.childs, __helper, printer);
            __helper.Pop();
            if (nodeId != other.nodeId) CodeGenImplTools.LogCompError(__helper, "nodeId", printer, other.nodeId, nodeId);
        }
        public virtual bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            switch(__name)
            {
                case "childs":
                childs.ReadFromJson(reader);
                break;
                case "nodeId":
                nodeId = (int)(Int64)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public virtual void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            writer.WritePropertyName("childs");
            childs.WriteJson(writer);
            writer.WritePropertyName("nodeId");
            writer.WriteValue(nodeId);
        }
        public virtual ushort GetClassId() 
        {
        return (System.UInt16)Types.ContextNode;
        }
        public virtual System.Object NewInst() 
        {
        return new ContextNode();
        }
    }
}
#endif
