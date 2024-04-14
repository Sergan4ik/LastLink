using System;
using System.Collections.Generic;
using System.Text;
using ZergRush.Alive;
using ZergRush;
using System.IO;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION
namespace Game.GameCore {

    public partial class UnitMove : IUpdatableFrom<Game.GameCore.UnitMove>, IUpdatableFrom<Game.GameCore.RTSRuntimeData>, IBinaryDeserializable, IBinarySerializable, IHashable, ICompareChechable<Game.GameCore.RTSRuntimeData>, IJsonSerializable, IPolymorphable, ICloneInst
    {
        public override void UpdateFrom(Game.GameCore.RTSRuntimeData other, ZRUpdateFromHelper __helper) 
        {
            base.UpdateFrom(other,__helper);
            var otherConcrete = (Game.GameCore.UnitMove)other;
            var cachedWaypointsCount = otherConcrete.cachedWaypoints.Length;
            var cachedWaypointsTemp = cachedWaypoints;
            Array.Resize(ref cachedWaypointsTemp, cachedWaypointsCount);
            cachedWaypoints = cachedWaypointsTemp;
            cachedWaypoints.UpdateFrom(otherConcrete.cachedWaypoints, __helper);
            currentWaypoint = otherConcrete.currentWaypoint;
            globalDestination.UpdateFrom(otherConcrete.globalDestination, __helper);
            maxDistanceToTarget = otherConcrete.maxDistanceToTarget;
            moveSpeed = otherConcrete.moveSpeed;
        }
        public void UpdateFrom(Game.GameCore.UnitMove other, ZRUpdateFromHelper __helper) 
        {
            this.UpdateFrom((Game.GameCore.RTSRuntimeData)other, __helper);
        }
        public override void Deserialize(ZRBinaryReader reader) 
        {
            base.Deserialize(reader);
            cachedWaypoints = reader.ReadUnityEngine_Vector3_Array();
            currentWaypoint = reader.ReadInt32();
            globalDestination = reader.ReadUnityEngine_Vector3();
            maxDistanceToTarget = reader.ReadSingle();
            moveSpeed = reader.ReadSingle();
        }
        public override void Serialize(ZRBinaryWriter writer) 
        {
            base.Serialize(writer);
            cachedWaypoints.Serialize(writer);
            writer.Write(currentWaypoint);
            globalDestination.Serialize(writer);
            writer.Write(maxDistanceToTarget);
            writer.Write(moveSpeed);
        }
        public override ulong CalculateHash(ZRHashHelper __helper) 
        {
            var baseVal = base.CalculateHash(__helper);
            System.UInt64 hash = baseVal;
            hash ^= (ulong)1925159920;
            hash += hash << 11; hash ^= hash >> 7;
            hash += cachedWaypoints.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)currentWaypoint;
            hash += hash << 11; hash ^= hash >> 7;
            hash += globalDestination.CalculateHash(__helper);
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)maxDistanceToTarget;
            hash += hash << 11; hash ^= hash >> 7;
            hash += (System.UInt64)moveSpeed;
            hash += hash << 11; hash ^= hash >> 7;
            return hash;
        }
        public  UnitMove() 
        {
            cachedWaypoints = Array.Empty<UnityEngine.Vector3>();
        }
        public override void CompareCheck(Game.GameCore.RTSRuntimeData other, ZRCompareCheckHelper __helper, Action<string> printer) 
        {
            base.CompareCheck(other,__helper,printer);
            var otherConcrete = (Game.GameCore.UnitMove)other;
            __helper.Push("cachedWaypoints");
            cachedWaypoints.CompareCheck(otherConcrete.cachedWaypoints, __helper, printer);
            __helper.Pop();
            if (currentWaypoint != otherConcrete.currentWaypoint) SerializationTools.LogCompError(__helper, "currentWaypoint", printer, otherConcrete.currentWaypoint, currentWaypoint);
            __helper.Push("globalDestination");
            globalDestination.CompareCheck(otherConcrete.globalDestination, __helper, printer);
            __helper.Pop();
            if (maxDistanceToTarget != otherConcrete.maxDistanceToTarget) SerializationTools.LogCompError(__helper, "maxDistanceToTarget", printer, otherConcrete.maxDistanceToTarget, maxDistanceToTarget);
            if (moveSpeed != otherConcrete.moveSpeed) SerializationTools.LogCompError(__helper, "moveSpeed", printer, otherConcrete.moveSpeed, moveSpeed);
        }
        public override bool ReadFromJsonField(ZRJsonTextReader reader, string __name) 
        {
            if (base.ReadFromJsonField(reader, __name)) return true;
            switch(__name)
            {
                case "cachedWaypoints":
                cachedWaypoints = cachedWaypoints.ReadFromJson(reader);
                break;
                case "currentWaypoint":
                currentWaypoint = (int)(Int64)reader.Value;
                break;
                case "globalDestination":
                globalDestination = (UnityEngine.Vector3)reader.ReadFromJsonUnityEngine_Vector3();
                break;
                case "maxDistanceToTarget":
                maxDistanceToTarget = (float)(double)reader.Value;
                break;
                case "moveSpeed":
                moveSpeed = (float)(double)reader.Value;
                break;
                default: return false; break;
            }
            return true;
        }
        public override void WriteJsonFields(ZRJsonTextWriter writer) 
        {
            base.WriteJsonFields(writer);
            writer.WritePropertyName("cachedWaypoints");
            cachedWaypoints.WriteJson(writer);
            writer.WritePropertyName("currentWaypoint");
            writer.WriteValue(currentWaypoint);
            writer.WritePropertyName("globalDestination");
            globalDestination.WriteJson(writer);
            writer.WritePropertyName("maxDistanceToTarget");
            writer.WriteValue(maxDistanceToTarget);
            writer.WritePropertyName("moveSpeed");
            writer.WriteValue(moveSpeed);
        }
        public override ushort GetClassId() 
        {
        return (System.UInt16)Types.UnitMove;
        }
        public override System.Object NewInst() 
        {
        return new UnitMove();
        }
    }
}
#endif
