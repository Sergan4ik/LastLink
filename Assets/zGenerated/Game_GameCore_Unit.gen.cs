using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class Unit : IUpdatableFrom<Game.GameCore.Unit>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.Unit)other;
            cfg.UpdateFrom(otherConcrete.cfg, __helper);
            stats.UpdateFrom(otherConcrete.stats, __helper);
            transform.UpdateFrom(otherConcrete.transform, __helper);
            unitActions.UpdateFrom(otherConcrete.unitActions, __helper);
        }
        public void UpdateFrom(Game.GameCore.Unit other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            cfg.Deserialize(reader);
            stats.Deserialize(reader);
            transform.Deserialize(reader);
            unitActions.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            cfg.Serialize(writer);
            stats.Serialize(writer);
            transform.Serialize(writer);
            unitActions.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1347601703;
            hash += hash << 11; hash ^= hash >> 7;
            hash += cfg.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += stats.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += transform.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += unitActions.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.Unit)other;
            __helper.Push("cfg");
            cfg.CompareCheck(otherConcrete.cfg, __helper, printer);
            __helper.Pop();
            __helper.Push("stats");
            stats.CompareCheck(otherConcrete.stats, __helper, printer);
            __helper.Pop();
            __helper.Push("transform");
            transform.CompareCheck(otherConcrete.transform, __helper, printer);
            __helper.Pop();
            __helper.Push("unitActions");
            unitActions.CompareCheck(otherConcrete.unitActions, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "cfg":
                cfg.ReadFromJson(reader);
                break;
                case "stats":
                stats.ReadFromJson(reader);
                break;
                case "transform":
                transform.ReadFromJson(reader);
                break;
                case "unitActions":
                unitActions.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("cfg");
            cfg.WriteJson(writer);
            writer.WritePropertyName("stats");
            stats.WriteJson(writer);
            writer.WritePropertyName("transform");
            transform.WriteJson(writer);
            writer.WritePropertyName("unitActions");
            unitActions.WriteJson(writer);
        }
        public  Unit() 
        {
            cfg = new Game.GameCore.UnitConfig();
            stats = new Game.GameCore.UnitStatsContainer();
            transform = new Game.GameCore.RTSTransform();
            unitActions = new System.Collections.Generic.List<Game.GameCore.UnitAction>();
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
