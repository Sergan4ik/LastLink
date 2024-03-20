using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class DefaultAttack : IUpdatableFrom<Game.GameCore.DefaultAttack>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.DefaultAttack)other;
            attackTimer.UpdateFrom(otherConcrete.attackTimer, __helper);
        }
        public void UpdateFrom(Game.GameCore.DefaultAttack other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            attackTimer.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            attackTimer.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1307582501;
            hash += hash << 11; hash ^= hash >> 7;
            hash += attackTimer.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  DefaultAttack() 
        {
            attackTimer = new Game.GameCore.RTSTimerIntervals();
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.DefaultAttack)other;
            __helper.Push("attackTimer");
            attackTimer.CompareCheck(otherConcrete.attackTimer, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "attackTimer":
                attackTimer.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("attackTimer");
            attackTimer.WriteJson(writer);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.DefaultAttack;
        }
        public override System.Object NewInst() 
        {
        return new DefaultAttack();
        }
    }
}
#endif
