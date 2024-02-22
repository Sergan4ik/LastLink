using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSRuntimeData : IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable
    {
        public virtual void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {

        }
        public virtual void Deserialize(ZRBinaryReader reader) 
        {

        }
        public virtual void Serialize(ZRBinaryWriter writer) 
        {

        }
        public virtual ulong CalculateHash(ZRHashHelper __helper) 
        {
            System.UInt64 hash = 345093625;
            hash ^= (ulong)2106435769;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  RTSRuntimeData() 
        {

        }
        public virtual void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {

        }
        public virtual bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            switch(__name)
            {
                default: return false; break;
            }
            return true;
        }
        public virtual void WriteJsonFields(ZRJsonTextWriter writer) 
        {

        }
    }
}
#endif
