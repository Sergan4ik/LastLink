using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSContextNode : IUpdatableFrom<RTSContextNode>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (RTSContextNode)other;
        }
        public void UpdateFrom(RTSContextNode other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);

        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);

        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1691573271;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  RTSContextNode() 
        {

        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);

        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSContextNode;
        }
        public override System.Object NewInst() 
        {
        return new RTSContextNode();
        }
    }
}
#endif
