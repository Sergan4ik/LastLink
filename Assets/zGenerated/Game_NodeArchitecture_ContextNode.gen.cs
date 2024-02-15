using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.NodeArchitecture {

    public partial class ContextNode : IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public enum Types : ushort
        {
            ContextNode = 1,
            Faction = 2,
            GameModel = 3,
            RTSContextNode = 4,
            RTSContextRoot = 5,
            Unit = 6,
            ContextRoot = 7,
        }
        static Func<ContextNode> [] polymorphConstructors = new Func<ContextNode> [] {
            () => null, // 0
            () => new Game.NodeArchitecture.ContextNode(), // 1
            () => new Game.GameModel.Faction(), // 2
            () => new Game.GameModel.GameModel(), // 3
            () => new Game.GameModel.RTSContextNode(), // 4
            () => new Game.GameModel.RTSContextRoot(), // 5
            () => new Game.GameModel.Unit(), // 6
            () => new Game.NodeArchitecture.ContextRoot(), // 7
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
            if (nodeId != other.nodeId) SerializationTools.LogCompError(__helper, "nodeId", printer, other.nodeId, nodeId);
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
