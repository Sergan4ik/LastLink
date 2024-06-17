using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class MoveRandomUnitCommand : IUpdatableFrom<Game.GameCore.MoveRandomUnitCommand>, IUpdatableFrom<ZeroLag.ZeroLagCommand>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.ZeroLagCommand>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(ZeroLag.ZeroLagCommand other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.MoveRandomUnitCommand)other;
            seed = otherConcrete.seed;
        }
        public void UpdateFrom(Game.GameCore.MoveRandomUnitCommand other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((ZeroLag.ZeroLagCommand)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            seed = reader.ReadInt32();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            writer.Write(seed);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1205145125;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)seed;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  MoveRandomUnitCommand() 
        {

        }
        public override void CompareCheck(ZeroLag.ZeroLagCommand other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.MoveRandomUnitCommand)other;
            if (seed != otherConcrete.seed) CodeGenImplTools.LogCompError(__helper, "seed", printer, otherConcrete.seed, seed);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "seed":
                seed = (int)(Int64)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("seed");
            writer.WriteValue(seed);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.MoveRandomUnitCommand;
        }
        public override System.Object NewInst() 
        {
        return new MoveRandomUnitCommand();
        }
    }
}
#endif
