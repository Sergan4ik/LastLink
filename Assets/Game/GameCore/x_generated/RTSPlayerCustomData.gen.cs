using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION

public partial class RTSPlayerCustomData : IUpdatableFrom<RTSPlayerCustomData>, IUpdatableFrom<ZeroLag.MultiplayerTools.Modules.Database.CustomPlayerData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<ZeroLag.MultiplayerTools.Modules.Database.CustomPlayerData>, IJsonSerializable
{
    public override void UpdateFrom(ZeroLag.MultiplayerTools.Modules.Database.CustomPlayerData other, ZRUpdateFromHelper __helper) 
    {
        base.UpdateFrom(other,__helper);
        var otherConcrete = (RTSPlayerCustomData)other;
        level = otherConcrete.level;
        playerAvatar = otherConcrete.playerAvatar;
    }
    public void UpdateFrom(RTSPlayerCustomData other, ZRUpdateFromHelper __helper) 
    {
        this.UpdateFrom((ZeroLag.MultiplayerTools.Modules.Database.CustomPlayerData)other, __helper);
    }
    public override void Deserialize(ZRBinaryReader reader) 
    {
        base.Deserialize(reader);
        level = reader.ReadInt32();
        playerAvatar = reader.ReadString();
    }
    public override void Serialize(ZRBinaryWriter writer) 
    {
        base.Serialize(writer);
        writer.Write(level);
        writer.Write(playerAvatar);
    }
    public override ulong CalculateHash(ZRHashHelper __helper) 
    {
        var baseVal = base.CalculateHash(__helper);
        System.UInt64 hash = baseVal;
        hash ^= (ulong)1425863385;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)level;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (ulong)playerAvatar.CalculateHash();
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public  RTSPlayerCustomData() 
    {
        playerAvatar = string.Empty;
    }
    public override void CompareCheck(ZeroLag.MultiplayerTools.Modules.Database.CustomPlayerData other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        base.CompareCheck(other,__helper,printer);
        var otherConcrete = (RTSPlayerCustomData)other;
        if (level != otherConcrete.level) CodeGenImplTools.LogCompError(__helper, "level", printer, otherConcrete.level, level);
        if (playerAvatar != otherConcrete.playerAvatar) CodeGenImplTools.LogCompError(__helper, "playerAvatar", printer, otherConcrete.playerAvatar, playerAvatar);
    }
    public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
    {
        if (base.ReadFromJsonField(reader, __name)) return true;
        switch(__name)
        {
            case "level":
            level = (int)(Int64)reader.Value;
            break;
            case "playerAvatar":
            playerAvatar = (string) reader.Value;
            break;
            default: return false; break;
        }
        return true;
    }
    public override void WriteJsonFields(ZRJsonTextWriter writer) 
    {
        base.WriteJsonFields(writer);
        writer.WritePropertyName("level");
        writer.WriteValue(level);
        writer.WritePropertyName("playerAvatar");
        writer.WriteValue(playerAvatar);
    }
}
#endif
