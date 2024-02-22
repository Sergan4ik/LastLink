using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class Faction : IUpdatableFrom<Faction>, IUpdatableFrom<Game.NodeArchitecture.ContextNode>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.NodeArchitecture.ContextNode>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.NodeArchitecture.ContextNode other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Faction)other;
            factionType = otherConcrete.factionType;
            units.UpdateFrom(otherConcrete.units, __helper);
        }
        public void UpdateFrom(Faction other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.NodeArchitecture.ContextNode)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            factionType = reader.ReadEnum<FactionType>();
            units.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((Int32)factionType);
            units.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1821259775;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)factionType;
            hash += hash << 11; hash ^= hash >> 7;
            hash += units.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  Faction() 
        {
            units = new ZergRush.ReactiveCore.ReactiveCollection<Unit>();
        }
        public override void CompareCheck(Game.NodeArchitecture.ContextNode other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Faction)other;
            if (factionType != otherConcrete.factionType) SerializationTools.LogCompError(__helper, "factionType", printer, otherConcrete.factionType, factionType);
            __helper.Push("units");
            units.CompareCheck(otherConcrete.units, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "factionType":
                factionType = ((string)reader.Value).ParseEnum<FactionType>();
                break;
                case "units":
                units.ReadFromJson(reader);
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
            writer.WritePropertyName("units");
            units.WriteJson(writer);
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
