using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitConfig : IUpdatableFrom<Game.GameCore.UnitConfig>, IUpdatableFrom<Game.GameCore.ConfigData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareCheckable<Game.GameCore.ConfigData>, IJsonSerializable
    {
        public override void UpdateFrom(Game.GameCore.ConfigData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitConfig)other;
            autoAttackAnimation.UpdateFrom(otherConcrete.autoAttackAnimation, __helper);
            customAnimations.UpdateFrom(otherConcrete.customAnimations, __helper);
            deathAnimation.UpdateFrom(otherConcrete.deathAnimation, __helper);
            idleAnimation.UpdateFrom(otherConcrete.idleAnimation, __helper);
            levelConfig.UpdateFrom(otherConcrete.levelConfig, __helper);
            name = otherConcrete.name;
            walkAnimation.UpdateFrom(otherConcrete.walkAnimation, __helper);
        }
        public void UpdateFrom(Game.GameCore.UnitConfig other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.ConfigData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            autoAttackAnimation.Deserialize(reader);
            customAnimations.Deserialize(reader);
            deathAnimation.Deserialize(reader);
            idleAnimation.Deserialize(reader);
            levelConfig.Deserialize(reader);
            name = reader.ReadString();
            walkAnimation.Deserialize(reader);
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            autoAttackAnimation.Serialize(writer);
            customAnimations.Serialize(writer);
            deathAnimation.Serialize(writer);
            idleAnimation.Serialize(writer);
            levelConfig.Serialize(writer);
            writer.Write(name);
            walkAnimation.Serialize(writer);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)2124769659;
            hash += hash << 11; hash ^= hash >> 7;
            hash += autoAttackAnimation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += customAnimations.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += deathAnimation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += idleAnimation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += levelConfig.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (ulong)name.CalculateHash();
            hash += hash << 11; hash ^= hash >> 7;
            hash += walkAnimation.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public override void CompareCheck(Game.GameCore.ConfigData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitConfig)other;
            __helper.Push("autoAttackAnimation");
            autoAttackAnimation.CompareCheck(otherConcrete.autoAttackAnimation, __helper, printer);
            __helper.Pop();
            __helper.Push("customAnimations");
            customAnimations.CompareCheck(otherConcrete.customAnimations, __helper, printer);
            __helper.Pop();
            __helper.Push("deathAnimation");
            deathAnimation.CompareCheck(otherConcrete.deathAnimation, __helper, printer);
            __helper.Pop();
            __helper.Push("idleAnimation");
            idleAnimation.CompareCheck(otherConcrete.idleAnimation, __helper, printer);
            __helper.Pop();
            __helper.Push("levelConfig");
            levelConfig.CompareCheck(otherConcrete.levelConfig, __helper, printer);
            __helper.Pop();
            if (name != otherConcrete.name) CodeGenImplTools.LogCompError(__helper, "name", printer, otherConcrete.name, name);
            __helper.Push("walkAnimation");
            walkAnimation.CompareCheck(otherConcrete.walkAnimation, __helper, printer);
            __helper.Pop();
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "autoAttackAnimation":
                autoAttackAnimation.ReadFromJson(reader);
                break;
                case "customAnimations":
                customAnimations.ReadFromJson(reader);
                break;
                case "deathAnimation":
                deathAnimation.ReadFromJson(reader);
                break;
                case "idleAnimation":
                idleAnimation.ReadFromJson(reader);
                break;
                case "levelConfig":
                levelConfig.ReadFromJson(reader);
                break;
                case "name":
                name = (string) reader.Value;
                break;
                case "walkAnimation":
                walkAnimation.ReadFromJson(reader);
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("autoAttackAnimation");
            autoAttackAnimation.WriteJson(writer);
            writer.WritePropertyName("customAnimations");
            customAnimations.WriteJson(writer);
            writer.WritePropertyName("deathAnimation");
            deathAnimation.WriteJson(writer);
            writer.WritePropertyName("idleAnimation");
            idleAnimation.WriteJson(writer);
            writer.WritePropertyName("levelConfig");
            levelConfig.WriteJson(writer);
            writer.WritePropertyName("name");
            writer.WriteValue(name);
            writer.WritePropertyName("walkAnimation");
            walkAnimation.WriteJson(writer);
        }
        public  UnitConfig() 
        {
            autoAttackAnimation = new Game.GameCore.AnimationData();
            customAnimations = new System.Collections.Generic.List<Game.GameCore.AnimationData>();
            deathAnimation = new Game.GameCore.AnimationData();
            idleAnimation = new Game.GameCore.AnimationData();
            levelConfig = new System.Collections.Generic.List<Game.GameCore.UnitLevelConfig>();
            name = string.Empty;
            walkAnimation = new Game.GameCore.AnimationData();
        }
    }
}
#endif
