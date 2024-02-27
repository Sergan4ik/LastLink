using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSRuntimeData : IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public enum Types : ushort
        {
            RTSRuntimeData = 1,
            RTSTimerIntervals = 4,
            RTSTimerStatic = 5,
            RTSTransform = 2,
            SelectionRectClipSpace = 3,
            UnitStatsContainer = 6,
        }
        static Func<RTSRuntimeData> [] polymorphConstructors = new Func<RTSRuntimeData> [] {
            () => null, // 0
            () => new Game.GameCore.RTSRuntimeData(), // 1
            () => new Game.GameCore.RTSTransform(), // 2
            () => new Game.GameCore.SelectionRectClipSpace(), // 3
            () => new Game.GameCore.RTSTimerIntervals(), // 4
            () => new Game.GameCore.RTSTimerStatic(), // 5
            () => new Game.GameCore.UnitStatsContainer(), // 6
        };
        public static RTSRuntimeData CreatePolymorphic(System.UInt16 typeId) {
            return polymorphConstructors[typeId]();
        }
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
        public virtual ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSRuntimeData;
        }
        public virtual System.Object NewInst() 
        {
        return new RTSRuntimeData();
        }
    }
}
#endif
