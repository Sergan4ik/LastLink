using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class RTSCommand : IUpdatableFrom<Game.GameCore.RTSCommand>, IUpdatableFrom<ZeroLag.ZeroLagCommand>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.ZeroLagCommand>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public enum Types : ushort
        {
            RTSCommand = 1,
            CancelReadyCommand = 6,
            ConnectCommand = 5,
            InputCommand = 2,
            LogCommand = 3,
            MoveRandomUnitCommand = 10,
            SetReadyCommand = 7,
            SpawnRandomUnitCommand = 9,
            StartGameCommand = 4,
            UpdateLobbyPlayerCommand = 8,
        }
        static Func<RTSCommand> [] polymorphConstructors = new Func<RTSCommand> [] {
            () => null, // 0
            () => new Game.GameCore.RTSCommand(), // 1
            () => new Game.GameCore.InputCommand(), // 2
            () => new Game.GameCore.LogCommand(), // 3
            () => new Game.GameCore.StartGameCommand(), // 4
            () => new Game.GameCore.ConnectCommand(), // 5
            () => new Game.GameCore.CancelReadyCommand(), // 6
            () => new Game.GameCore.SetReadyCommand(), // 7
            () => new Game.GameCore.UpdateLobbyPlayerCommand(), // 8
            () => new Game.GameCore.SpawnRandomUnitCommand(), // 9
            () => new Game.GameCore.MoveRandomUnitCommand(), // 10
        };
        public static RTSCommand CreatePolymorphic(System.UInt16 typeId) {
            return polymorphConstructors[typeId]();
        }
        public override void UpdateFrom(ZeroLag.ZeroLagCommand other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.RTSCommand)other;
        }
        public void UpdateFrom(Game.GameCore.RTSCommand other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((ZeroLag.ZeroLagCommand)other, __helper);
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
            hash ^= (ulong)135429853;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  RTSCommand() 
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
        public virtual ushort GetClassId() 
        {
        return (System.UInt16)Types.RTSCommand;
        }
        public virtual System.Object NewInst() 
        {
        return new RTSCommand();
        }
    }
}
#endif
