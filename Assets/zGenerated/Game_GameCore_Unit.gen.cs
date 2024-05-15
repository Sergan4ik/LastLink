using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class Unit : IUpdatableFrom<Game.GameCore.Unit>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IsMultiRef, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.Unit)other;
            var behaviourClassId = otherConcrete.behaviour.GetClassId();
            if (behaviour == null || behaviour.GetClassId() != behaviourClassId) {
                behaviour = (Game.GameCore.UnitBehaviour)otherConcrete.behaviour.NewInst();
            }
            behaviour.UpdateFrom(otherConcrete.behaviour, __helper);
            cfg.UpdateFrom(otherConcrete.cfg, __helper);
            currentAnimation.UpdateFrom(otherConcrete.currentAnimation, __helper);
            factionSlot = otherConcrete.factionSlot;
            id = otherConcrete.id;
            stats.UpdateFrom(otherConcrete.stats, __helper);
            transform.UpdateFrom(otherConcrete.transform, __helper);
            unitActions.UpdateFrom(otherConcrete.unitActions, __helper);
        }
        public void UpdateFrom(Game.GameCore.Unit other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            var behaviourClassId = reader.ReadUInt16();
            if (behaviour == null || behaviour.GetClassId() != behaviourClassId) {
                behaviour = (Game.GameCore.UnitBehaviour)Game.GameCore.RTSRuntimeData.CreatePolymorphic(behaviourClassId);
            }
            behaviour.Deserialize(reader);
            cfg.Deserialize(reader);
            currentAnimation.Deserialize(reader);
            factionSlot = reader.ReadEnum<Game.GameCore.FactionSlot>();
            id = reader.ReadInt32();
            stats.Deserialize(reader);
            transform.Deserialize(reader);
            unitActions.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(behaviour.GetClassId());
            behaviour.Serialize(writer);
            cfg.Serialize(writer);
            currentAnimation.Serialize(writer);
            writer.Write((Int32)factionSlot);
            writer.Write(id);
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
            hash += behaviour.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += cfg.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += currentAnimation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionSlot;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)id;
            hash += hash << 11; hash ^= hash >> 7;
            hash += stats.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += transform.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += unitActions.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.Unit)other;
            if (CodeGenImplTools.CompareClassId(__helper, "behaviour", printer, behaviour, otherConcrete.behaviour)) {
                __helper.Push("behaviour");
                behaviour.CompareCheck(otherConcrete.behaviour, __helper, printer);
                __helper.Pop();
            }
            __helper.Push("cfg");
            cfg.CompareCheck(otherConcrete.cfg, __helper, printer);
            __helper.Pop();
            __helper.Push("currentAnimation");
            currentAnimation.CompareCheck(otherConcrete.currentAnimation, __helper, printer);
            __helper.Pop();
            if (factionSlot != otherConcrete.factionSlot) CodeGenImplTools.LogCompError(__helper, "factionSlot", printer, otherConcrete.factionSlot, factionSlot);
            if (id != otherConcrete.id) CodeGenImplTools.LogCompError(__helper, "id", printer, otherConcrete.id, id);
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
                case "behaviour":
                var behaviourClassId = reader.ReadJsonClassId();
                if (behaviour == null || behaviour.GetClassId() != behaviourClassId) {
                    behaviour = (Game.GameCore.UnitBehaviour)Game.GameCore.RTSRuntimeData.CreatePolymorphic(behaviourClassId);
                }
                behaviour.ReadFromJson(reader);
                break;
                case "cfg":
                cfg.ReadFromJson(reader);
                break;
                case "currentAnimation":
                currentAnimation.ReadFromJson(reader);
                break;
                case "factionSlot":
                factionSlot = ((string)reader.Value).ParseEnum<Game.GameCore.FactionSlot>();
                break;
                case "id":
                id = (int)(Int64)reader.Value;
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
            writer.WritePropertyName("behaviour");
            behaviour.WriteJson(writer);
            writer.WritePropertyName("cfg");
            cfg.WriteJson(writer);
            writer.WritePropertyName("currentAnimation");
            currentAnimation.WriteJson(writer);
            writer.WritePropertyName("factionSlot");
            writer.WriteValue(factionSlot.ToString());
            writer.WritePropertyName("id");
            writer.WriteValue(id);
            writer.WritePropertyName("stats");
            stats.WriteJson(writer);
            writer.WritePropertyName("transform");
            transform.WriteJson(writer);
            writer.WritePropertyName("unitActions");
            unitActions.WriteJson(writer);
        }
        public  Unit() 
        {
            behaviour = (Game.GameCore.UnitBehaviour)new Game.GameCore.UnitBehaviour();
            cfg = new Game.GameCore.UnitConfig();
            currentAnimation = new Game.GameCore.AnimationData();
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
