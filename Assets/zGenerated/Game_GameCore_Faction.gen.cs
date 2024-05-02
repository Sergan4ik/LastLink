using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class Faction : IUpdatableFrom<Game.GameCore.Faction>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.Faction)other;
            factionType = otherConcrete.factionType;
            slot = otherConcrete.slot;
        }
        public void UpdateFrom(Game.GameCore.Faction other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            factionType = reader.ReadEnum<Game.GameCore.FactionType>();
            slot = reader.ReadEnum<Game.GameCore.FactionSlot>();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((Int32)factionType);
            writer.Write((Int32)slot);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1821259775;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionType;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)slot;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  Faction() 
        {

        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.Faction)other;
            if (factionType != otherConcrete.factionType) CodeGenImplTools.LogCompError(__helper, "factionType", printer, otherConcrete.factionType, factionType);
            if (slot != otherConcrete.slot) CodeGenImplTools.LogCompError(__helper, "slot", printer, otherConcrete.slot, slot);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "factionType":
                factionType = ((string)reader.Value).ParseEnum<Game.GameCore.FactionType>();
                break;
                case "slot":
                slot = ((string)reader.Value).ParseEnum<Game.GameCore.FactionSlot>();
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("factionType");
            writer.WriteValue(factionType.ToString());
            writer.WritePropertyName("slot");
            writer.WriteValue(slot.ToString());
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.Faction;
        }
        public override System.Object NewInst() 
        {
        return new Faction();
        }
    }
}
#endif
