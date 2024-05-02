using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSRuntimeData : IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public enum Types : ushort
        {
            RTSRuntimeData = 1,
            AnimationData = 2,
            ControlData = 3,
            DefaultAttack = 4,
            DOT = 5,
            Faction = 6,
            RTSInput = 7,
            RTSStopWatch = 8,
            RTSTimerIntervals = 9,
            RTSTimerStatic = 10,
            RTSTransform = 11,
            SelectionRectClipSpace = 12,
            TargetData = 13,
            Unit = 14,
            UnitAction = 15,
            UnitEffect = 16,
            UnitMove = 17,
            UnitStatsContainer = 18,
        }
        static Func<RTSRuntimeData> [] polymorphConstructors = new Func<RTSRuntimeData> [] {
            () => null, // 0
            () => new Game.GameCore.RTSRuntimeData(), // 1
            () => new Game.GameCore.AnimationData(), // 2
            () => new Game.GameCore.ControlData(), // 3
            () => new Game.GameCore.DefaultAttack(), // 4
            () => new Game.GameCore.DOT(), // 5
            () => new Game.GameCore.Faction(), // 6
            () => new Game.GameCore.RTSInput(), // 7
            () => new Game.GameCore.RTSStopWatch(), // 8
            () => new Game.GameCore.RTSTimerIntervals(), // 9
            () => new Game.GameCore.RTSTimerStatic(), // 10
            () => new Game.GameCore.RTSTransform(), // 11
            () => new Game.GameCore.SelectionRectClipSpace(), // 12
            () => new Game.GameCore.TargetData(), // 13
            () => new Game.GameCore.Unit(), // 14
            () => new Game.GameCore.UnitAction(), // 15
            () => new Game.GameCore.UnitEffect(), // 16
            () => new Game.GameCore.UnitMove(), // 17
            () => new Game.GameCore.UnitStatsContainer(), // 18
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
