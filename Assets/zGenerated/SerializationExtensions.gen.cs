using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ZergRush.Alive;
using ZergRush;
using Newtonsoft.Json;
#if !INCLUDE_ONLY_CODE_GENERATION

public static partial class SerializationExtensions
{
    public static void UpdateFrom(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> other, ZRUpdateFromHelper __helper) 
    {
        int i = 0;
        int oldCount = self.Count;
        int crossCount = Math.Min(oldCount, other.Count);
        for (; i < crossCount; ++i)
        {
            if (other[i] == null) {
                self[i] = null;
            }
            else { 
                if (self[i] == null) {
                    self[i] = new Game.GameCore.Unit();
                }
                self[i].UpdateFrom(other[i], __helper);
            }
        }
        for (; i < other.Count; ++i)
        {
            Game.GameCore.Unit inst = default;
            if (other[i] == null) {
                inst = null;
            }
            else { 
                inst = new Game.GameCore.Unit();
                inst.UpdateFrom(other[i], __helper);
            }
            self.Add(inst);
        }
        for (; i < oldCount; ++i)
        {
            self.RemoveAt(self.Count - 1);
        }
    }
    public static void Deserialize(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZRBinaryReader reader) 
    {
        var size = reader.ReadInt32();
        if(size > 100000) throw new ZergRushCorruptedOrInvalidDataLayout();
        self.Capacity = size;
        for (int i = 0; i < size; i++)
        {
            if (!reader.ReadBoolean()) { self.Add(null); continue; }
            Game.GameCore.Unit val = default;
            val = new Game.GameCore.Unit();
            val.Deserialize(reader);
            self.Add(val);
        }
    }
    public static void Serialize(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZRBinaryWriter writer) 
    {
        writer.Write(self.Count);
        for (int i = 0; i < self.Count; i++)
        {
            writer.Write(self[i] != null);
            if (self[i] != null)
            {
                self[i].Serialize(writer);
            }
        }
    }
    public static ulong CalculateHash(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)1261931807;
        hash += hash << 11; hash ^= hash >> 7;
        var size = self.Count;
        for (int i = 0; i < size; i++)
        {
            hash += self[i] != null ? self[i].CalculateHash(__helper) : 345093625;
            hash += hash << 11; hash ^= hash >> 7;
        }
        return hash;
    }
    public static void CompareCheck(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.Count != other.Count) SerializationTools.LogCompError(__helper, "Count", printer, other.Count, self.Count);
        var count = Math.Min(self.Count, other.Count);
        for (int i = 0; i < count; i++)
        {
            if (SerializationTools.CompareNull(__helper, i.ToString(), printer, self[i], other[i])) {
                __helper.Push(i.ToString());
                self[i].CompareCheck(other[i], __helper, printer);
                __helper.Pop();
            }
        }
    }
    public static bool ReadFromJson(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZRJsonTextReader reader) 
    {
        if (reader.TokenType != JsonToken.StartArray) throw new JsonSerializationException("Bad Json Format");
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray) { break; }
            if (reader.TokenType == JsonToken.Null) { self.Add(null); continue; }
            Game.GameCore.Unit val = default;
            val = new Game.GameCore.Unit();
            val.ReadFromJson(reader);
            self.Add(val);
        }
        return true;
    }
    public static void WriteJson(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Unit> self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartArray();
        for (int i = 0; i < self.Count; i++)
        {
            if (self[i] == null)
            {
                writer.WriteNull();
            }
            else
            {
                self[i].WriteJson(writer);
            }
        }
        writer.WriteEndArray();
    }
    public static void UpdateFrom(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, System.Collections.Generic.List<Game.GameCore.UnitAction> other, ZRUpdateFromHelper __helper) 
    {
        int i = 0;
        int oldCount = self.Count;
        int crossCount = Math.Min(oldCount, other.Count);
        for (; i < crossCount; ++i)
        {
            if (other[i] == null) {
                self[i] = null;
            }
            else { 
                var self_i_ClassId = other[i].GetClassId();
                if (self[i] == null || self[i].GetClassId() != self_i_ClassId) {
                    self[i] = (Game.GameCore.UnitAction)other[i].NewInst();
                }
                self[i].UpdateFrom(other[i], __helper);
            }
        }
        for (; i < other.Count; ++i)
        {
            Game.GameCore.UnitAction inst = default;
            if (other[i] == null) {
                inst = null;
            }
            else { 
                inst = (Game.GameCore.UnitAction)other[i].NewInst();
                inst.UpdateFrom(other[i], __helper);
            }
            self.Add(inst);
        }
        for (; i < oldCount; ++i)
        {
            self.RemoveAt(self.Count - 1);
        }
    }
    public static void Deserialize(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, ZRBinaryReader reader) 
    {
        var size = reader.ReadInt32();
        if(size > 100000) throw new ZergRushCorruptedOrInvalidDataLayout();
        self.Capacity = size;
        for (int i = 0; i < size; i++)
        {
            if (!reader.ReadBoolean()) { self.Add(null); continue; }
            Game.GameCore.UnitAction val = default;
            val = (Game.GameCore.UnitAction)Game.NodeArchitecture.ContextNode.CreatePolymorphic(reader.ReadUInt16());
            val.Deserialize(reader);
            self.Add(val);
        }
    }
    public static void Serialize(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, ZRBinaryWriter writer) 
    {
        writer.Write(self.Count);
        for (int i = 0; i < self.Count; i++)
        {
            writer.Write(self[i] != null);
            if (self[i] != null)
            {
                writer.Write(self[i].GetClassId());
                self[i].Serialize(writer);
            }
        }
    }
    public static ulong CalculateHash(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)910491146;
        hash += hash << 11; hash ^= hash >> 7;
        var size = self.Count;
        for (int i = 0; i < size; i++)
        {
            hash += self[i] != null ? self[i].CalculateHash(__helper) : 345093625;
            hash += hash << 11; hash ^= hash >> 7;
        }
        return hash;
    }
    public static void CompareCheck(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, System.Collections.Generic.List<Game.GameCore.UnitAction> other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.Count != other.Count) SerializationTools.LogCompError(__helper, "Count", printer, other.Count, self.Count);
        var count = Math.Min(self.Count, other.Count);
        for (int i = 0; i < count; i++)
        {
            if (SerializationTools.CompareNull(__helper, i.ToString(), printer, self[i], other[i])) {
                if (SerializationTools.CompareClassId(__helper, i.ToString(), printer, self[i], other[i])) {
                    __helper.Push(i.ToString());
                    self[i].CompareCheck(other[i], __helper, printer);
                    __helper.Pop();
                }
            }
        }
    }
    public static bool ReadFromJson(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, ZRJsonTextReader reader) 
    {
        if (reader.TokenType != JsonToken.StartArray) throw new JsonSerializationException("Bad Json Format");
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray) { break; }
            if (reader.TokenType == JsonToken.Null) { self.Add(null); continue; }
            Game.GameCore.UnitAction val = default;
            val = (Game.GameCore.UnitAction)Game.NodeArchitecture.ContextNode.CreatePolymorphic(reader.ReadJsonClassId());
            val.ReadFromJson(reader);
            self.Add(val);
        }
        return true;
    }
    public static void WriteJson(this System.Collections.Generic.List<Game.GameCore.UnitAction> self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartArray();
        for (int i = 0; i < self.Count; i++)
        {
            if (self[i] == null)
            {
                writer.WriteNull();
            }
            else
            {
                self[i].WriteJson(writer);
            }
        }
        writer.WriteEndArray();
    }
    public static void UpdateFrom(ref this UnityEngine.Vector3 self, UnityEngine.Vector3 other, ZRUpdateFromHelper __helper) 
    {
        self.x = other.x;
        self.y = other.y;
        self.z = other.z;
    }
    public static void UpdateFrom(ref this UnityEngine.Quaternion self, UnityEngine.Quaternion other, ZRUpdateFromHelper __helper) 
    {
        self.w = other.w;
        self.x = other.x;
        self.y = other.y;
        self.z = other.z;
    }
    public static UnityEngine.Vector3 ReadUnityEngine_Vector3(this ZRBinaryReader reader) 
    {
        var self = new UnityEngine.Vector3();
        self.x = reader.ReadSingle();
        self.y = reader.ReadSingle();
        self.z = reader.ReadSingle();
        return self;
    }
    public static UnityEngine.Quaternion ReadUnityEngine_Quaternion(this ZRBinaryReader reader) 
    {
        var self = new UnityEngine.Quaternion();
        self.w = reader.ReadSingle();
        self.x = reader.ReadSingle();
        self.y = reader.ReadSingle();
        self.z = reader.ReadSingle();
        return self;
    }
    public static void Serialize(this UnityEngine.Vector3 self, ZRBinaryWriter writer) 
    {
        writer.Write(self.x);
        writer.Write(self.y);
        writer.Write(self.z);
    }
    public static void Serialize(this UnityEngine.Quaternion self, ZRBinaryWriter writer) 
    {
        writer.Write(self.w);
        writer.Write(self.x);
        writer.Write(self.y);
        writer.Write(self.z);
    }
    public static ulong CalculateHash(this UnityEngine.Vector3 self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)701202043;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.x;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.y;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.z;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static ulong CalculateHash(this UnityEngine.Quaternion self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)98633650;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.w;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.x;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.y;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.z;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static void CompareCheck(this UnityEngine.Vector3 self, UnityEngine.Vector3 other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.x != other.x) SerializationTools.LogCompError(__helper, "x", printer, other.x, self.x);
        if (self.y != other.y) SerializationTools.LogCompError(__helper, "y", printer, other.y, self.y);
        if (self.z != other.z) SerializationTools.LogCompError(__helper, "z", printer, other.z, self.z);
    }
    public static void CompareCheck(this UnityEngine.Quaternion self, UnityEngine.Quaternion other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.w != other.w) SerializationTools.LogCompError(__helper, "w", printer, other.w, self.w);
        if (self.x != other.x) SerializationTools.LogCompError(__helper, "x", printer, other.x, self.x);
        if (self.y != other.y) SerializationTools.LogCompError(__helper, "y", printer, other.y, self.y);
        if (self.z != other.z) SerializationTools.LogCompError(__helper, "z", printer, other.z, self.z);
    }
    public static UnityEngine.Vector3 ReadFromJsonUnityEngine_Vector3(this ZRJsonTextReader reader) 
    {
        var self = new UnityEngine.Vector3();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    case "x":
                    self.x = (float)(double)reader.Value;
                    break;
                    case "y":
                    self.y = (float)(double)reader.Value;
                    break;
                    case "z":
                    self.z = (float)(double)reader.Value;
                    break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return self;
    }
    public static void WriteJson(this UnityEngine.Vector3 self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(self.x);
        writer.WritePropertyName("y");
        writer.WriteValue(self.y);
        writer.WritePropertyName("z");
        writer.WriteValue(self.z);
        writer.WriteEndObject();
    }
    public static UnityEngine.Quaternion ReadFromJsonUnityEngine_Quaternion(this ZRJsonTextReader reader) 
    {
        var self = new UnityEngine.Quaternion();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    case "w":
                    self.w = (float)(double)reader.Value;
                    break;
                    case "x":
                    self.x = (float)(double)reader.Value;
                    break;
                    case "y":
                    self.y = (float)(double)reader.Value;
                    break;
                    case "z":
                    self.z = (float)(double)reader.Value;
                    break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return self;
    }
    public static void WriteJson(this UnityEngine.Quaternion self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WritePropertyName("w");
        writer.WriteValue(self.w);
        writer.WritePropertyName("x");
        writer.WriteValue(self.x);
        writer.WritePropertyName("y");
        writer.WriteValue(self.y);
        writer.WritePropertyName("z");
        writer.WriteValue(self.z);
        writer.WriteEndObject();
    }
    public static void UpdateFrom(this System.Collections.Generic.List<float> self, System.Collections.Generic.List<float> other, ZRUpdateFromHelper __helper) 
    {
        int i = 0;
        int oldCount = self.Count;
        int crossCount = Math.Min(oldCount, other.Count);
        for (; i < crossCount; ++i)
        {
            self[i] = other[i];
        }
        for (; i < other.Count; ++i)
        {
            var inst = other[i];
            self.Add(inst);
        }
        for (; i < oldCount; ++i)
        {
            self.RemoveAt(self.Count - 1);
        }
    }
    public static void Deserialize(this System.Collections.Generic.List<float> self, ZRBinaryReader reader) 
    {
        var size = reader.ReadInt32();
        if(size > 100000) throw new ZergRushCorruptedOrInvalidDataLayout();
        self.Capacity = size;
        for (int i = 0; i < size; i++)
        {
            float val = default;
            val = reader.ReadSingle();
            self.Add(val);
        }
    }
    public static void Serialize(this System.Collections.Generic.List<float> self, ZRBinaryWriter writer) 
    {
        writer.Write(self.Count);
        for (int i = 0; i < self.Count; i++)
        {
            {
                writer.Write(self[i]);
            }
        }
    }
    public static ulong CalculateHash(this System.Collections.Generic.List<float> self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)910491146;
        hash += hash << 11; hash ^= hash >> 7;
        var size = self.Count;
        for (int i = 0; i < size; i++)
        {
            hash += (System.UInt64)self[i];
            hash += hash << 11; hash ^= hash >> 7;
        }
        return hash;
    }
    public static void CompareCheck(this System.Collections.Generic.List<float> self, System.Collections.Generic.List<float> other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.Count != other.Count) SerializationTools.LogCompError(__helper, "Count", printer, other.Count, self.Count);
        var count = Math.Min(self.Count, other.Count);
        for (int i = 0; i < count; i++)
        {
            if (self[i] != other[i]) SerializationTools.LogCompError(__helper, i.ToString(), printer, other[i], self[i]);
        }
    }
    public static bool ReadFromJson(this System.Collections.Generic.List<float> self, ZRJsonTextReader reader) 
    {
        if (reader.TokenType != JsonToken.StartArray) throw new JsonSerializationException("Bad Json Format");
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray) { break; }
            float val = default;
            val = (float)(double)reader.Value;
            self.Add(val);
        }
        return true;
    }
    public static void WriteJson(this System.Collections.Generic.List<float> self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartArray();
        for (int i = 0; i < self.Count; i++)
        {
            writer.WriteValue(self[i]);
        }
        writer.WriteEndArray();
    }
    public static void UpdateFrom(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> other, ZRUpdateFromHelper __helper) 
    {
        int i = 0;
        int oldCount = self.Count;
        int crossCount = Math.Min(oldCount, other.Count);
        for (; i < crossCount; ++i)
        {
            if (other[i] == null) {
                self[i] = null;
            }
            else { 
                if (self[i] == null) {
                    self[i] = new Game.GameCore.Faction();
                }
                self[i].UpdateFrom(other[i], __helper);
            }
        }
        for (; i < other.Count; ++i)
        {
            Game.GameCore.Faction inst = default;
            if (other[i] == null) {
                inst = null;
            }
            else { 
                inst = new Game.GameCore.Faction();
                inst.UpdateFrom(other[i], __helper);
            }
            self.Add(inst);
        }
        for (; i < oldCount; ++i)
        {
            self.RemoveAt(self.Count - 1);
        }
    }
    public static void UpdateFrom(this ZergRush.ReactiveCore.EventStream self, ZergRush.ReactiveCore.EventStream other, ZRUpdateFromHelper __helper) 
    {

    }
    public static void Deserialize(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZRBinaryReader reader) 
    {
        var size = reader.ReadInt32();
        if(size > 100000) throw new ZergRushCorruptedOrInvalidDataLayout();
        self.Capacity = size;
        for (int i = 0; i < size; i++)
        {
            if (!reader.ReadBoolean()) { self.Add(null); continue; }
            Game.GameCore.Faction val = default;
            val = new Game.GameCore.Faction();
            val.Deserialize(reader);
            self.Add(val);
        }
    }
    public static void Deserialize(this ZergRush.ReactiveCore.EventStream self, ZRBinaryReader reader) 
    {

    }
    public static void Serialize(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZRBinaryWriter writer) 
    {
        writer.Write(self.Count);
        for (int i = 0; i < self.Count; i++)
        {
            writer.Write(self[i] != null);
            if (self[i] != null)
            {
                self[i].Serialize(writer);
            }
        }
    }
    public static void Serialize(this ZergRush.ReactiveCore.EventStream self, ZRBinaryWriter writer) 
    {

    }
    public static ulong CalculateHash(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)1261931807;
        hash += hash << 11; hash ^= hash >> 7;
        var size = self.Count;
        for (int i = 0; i < size; i++)
        {
            hash += self[i] != null ? self[i].CalculateHash(__helper) : 345093625;
            hash += hash << 11; hash ^= hash >> 7;
        }
        return hash;
    }
    public static ulong CalculateHash(this ZergRush.ReactiveCore.EventStream self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)396934352;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static void CompareCheck(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.Count != other.Count) SerializationTools.LogCompError(__helper, "Count", printer, other.Count, self.Count);
        var count = Math.Min(self.Count, other.Count);
        for (int i = 0; i < count; i++)
        {
            if (SerializationTools.CompareNull(__helper, i.ToString(), printer, self[i], other[i])) {
                __helper.Push(i.ToString());
                self[i].CompareCheck(other[i], __helper, printer);
                __helper.Pop();
            }
        }
    }
    public static void CompareCheck(this ZergRush.ReactiveCore.EventStream self, ZergRush.ReactiveCore.EventStream other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {

    }
    public static bool ReadFromJson(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZRJsonTextReader reader) 
    {
        if (reader.TokenType != JsonToken.StartArray) throw new JsonSerializationException("Bad Json Format");
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray) { break; }
            if (reader.TokenType == JsonToken.Null) { self.Add(null); continue; }
            Game.GameCore.Faction val = default;
            val = new Game.GameCore.Faction();
            val.ReadFromJson(reader);
            self.Add(val);
        }
        return true;
    }
    public static void WriteJson(this ZergRush.ReactiveCore.ReactiveCollection<Game.GameCore.Faction> self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartArray();
        for (int i = 0; i < self.Count; i++)
        {
            if (self[i] == null)
            {
                writer.WriteNull();
            }
            else
            {
                self[i].WriteJson(writer);
            }
        }
        writer.WriteEndArray();
    }
    public static bool ReadFromJson(this ZergRush.ReactiveCore.EventStream self, ZRJsonTextReader reader) 
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    default: return false; break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return true;
    }
    public static void WriteJson(this ZergRush.ReactiveCore.EventStream self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WriteEndObject();
    }
    public static void UpdateFrom(ref this UnityEngine.Vector2 self, UnityEngine.Vector2 other, ZRUpdateFromHelper __helper) 
    {
        self.x = other.x;
        self.y = other.y;
    }
    public static void UpdateFrom(ref this UnityEngine.Matrix4x4 self, UnityEngine.Matrix4x4 other, ZRUpdateFromHelper __helper) 
    {
        self.m00 = other.m00;
        self.m01 = other.m01;
        self.m02 = other.m02;
        self.m03 = other.m03;
        self.m10 = other.m10;
        self.m11 = other.m11;
        self.m12 = other.m12;
        self.m13 = other.m13;
        self.m20 = other.m20;
        self.m21 = other.m21;
        self.m22 = other.m22;
        self.m23 = other.m23;
        self.m30 = other.m30;
        self.m31 = other.m31;
        self.m32 = other.m32;
        self.m33 = other.m33;
    }
    public static UnityEngine.Vector2 ReadUnityEngine_Vector2(this ZRBinaryReader reader) 
    {
        var self = new UnityEngine.Vector2();
        self.x = reader.ReadSingle();
        self.y = reader.ReadSingle();
        return self;
    }
    public static UnityEngine.Matrix4x4 ReadUnityEngine_Matrix4x4(this ZRBinaryReader reader) 
    {
        var self = new UnityEngine.Matrix4x4();
        self.m00 = reader.ReadSingle();
        self.m01 = reader.ReadSingle();
        self.m02 = reader.ReadSingle();
        self.m03 = reader.ReadSingle();
        self.m10 = reader.ReadSingle();
        self.m11 = reader.ReadSingle();
        self.m12 = reader.ReadSingle();
        self.m13 = reader.ReadSingle();
        self.m20 = reader.ReadSingle();
        self.m21 = reader.ReadSingle();
        self.m22 = reader.ReadSingle();
        self.m23 = reader.ReadSingle();
        self.m30 = reader.ReadSingle();
        self.m31 = reader.ReadSingle();
        self.m32 = reader.ReadSingle();
        self.m33 = reader.ReadSingle();
        return self;
    }
    public static void Serialize(this UnityEngine.Vector2 self, ZRBinaryWriter writer) 
    {
        writer.Write(self.x);
        writer.Write(self.y);
    }
    public static void Serialize(this UnityEngine.Matrix4x4 self, ZRBinaryWriter writer) 
    {
        writer.Write(self.m00);
        writer.Write(self.m01);
        writer.Write(self.m02);
        writer.Write(self.m03);
        writer.Write(self.m10);
        writer.Write(self.m11);
        writer.Write(self.m12);
        writer.Write(self.m13);
        writer.Write(self.m20);
        writer.Write(self.m21);
        writer.Write(self.m22);
        writer.Write(self.m23);
        writer.Write(self.m30);
        writer.Write(self.m31);
        writer.Write(self.m32);
        writer.Write(self.m33);
    }
    public static ulong CalculateHash(this UnityEngine.Vector2 self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)701198946;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.x;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.y;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static ulong CalculateHash(this UnityEngine.Matrix4x4 self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)146128276;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m00;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m01;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m02;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m03;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m10;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m11;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m12;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m13;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m20;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m21;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m22;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m23;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m30;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m31;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m32;
        hash += hash << 11; hash ^= hash >> 7;
        hash += (System.UInt64)self.m33;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static void CompareCheck(this UnityEngine.Vector2 self, UnityEngine.Vector2 other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.x != other.x) SerializationTools.LogCompError(__helper, "x", printer, other.x, self.x);
        if (self.y != other.y) SerializationTools.LogCompError(__helper, "y", printer, other.y, self.y);
    }
    public static void CompareCheck(this UnityEngine.Matrix4x4 self, UnityEngine.Matrix4x4 other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {
        if (self.m00 != other.m00) SerializationTools.LogCompError(__helper, "m00", printer, other.m00, self.m00);
        if (self.m01 != other.m01) SerializationTools.LogCompError(__helper, "m01", printer, other.m01, self.m01);
        if (self.m02 != other.m02) SerializationTools.LogCompError(__helper, "m02", printer, other.m02, self.m02);
        if (self.m03 != other.m03) SerializationTools.LogCompError(__helper, "m03", printer, other.m03, self.m03);
        if (self.m10 != other.m10) SerializationTools.LogCompError(__helper, "m10", printer, other.m10, self.m10);
        if (self.m11 != other.m11) SerializationTools.LogCompError(__helper, "m11", printer, other.m11, self.m11);
        if (self.m12 != other.m12) SerializationTools.LogCompError(__helper, "m12", printer, other.m12, self.m12);
        if (self.m13 != other.m13) SerializationTools.LogCompError(__helper, "m13", printer, other.m13, self.m13);
        if (self.m20 != other.m20) SerializationTools.LogCompError(__helper, "m20", printer, other.m20, self.m20);
        if (self.m21 != other.m21) SerializationTools.LogCompError(__helper, "m21", printer, other.m21, self.m21);
        if (self.m22 != other.m22) SerializationTools.LogCompError(__helper, "m22", printer, other.m22, self.m22);
        if (self.m23 != other.m23) SerializationTools.LogCompError(__helper, "m23", printer, other.m23, self.m23);
        if (self.m30 != other.m30) SerializationTools.LogCompError(__helper, "m30", printer, other.m30, self.m30);
        if (self.m31 != other.m31) SerializationTools.LogCompError(__helper, "m31", printer, other.m31, self.m31);
        if (self.m32 != other.m32) SerializationTools.LogCompError(__helper, "m32", printer, other.m32, self.m32);
        if (self.m33 != other.m33) SerializationTools.LogCompError(__helper, "m33", printer, other.m33, self.m33);
    }
    public static UnityEngine.Vector2 ReadFromJsonUnityEngine_Vector2(this ZRJsonTextReader reader) 
    {
        var self = new UnityEngine.Vector2();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    case "x":
                    self.x = (float)(double)reader.Value;
                    break;
                    case "y":
                    self.y = (float)(double)reader.Value;
                    break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return self;
    }
    public static void WriteJson(this UnityEngine.Vector2 self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(self.x);
        writer.WritePropertyName("y");
        writer.WriteValue(self.y);
        writer.WriteEndObject();
    }
    public static UnityEngine.Matrix4x4 ReadFromJsonUnityEngine_Matrix4x4(this ZRJsonTextReader reader) 
    {
        var self = new UnityEngine.Matrix4x4();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    case "m00":
                    self.m00 = (float)(double)reader.Value;
                    break;
                    case "m01":
                    self.m01 = (float)(double)reader.Value;
                    break;
                    case "m02":
                    self.m02 = (float)(double)reader.Value;
                    break;
                    case "m03":
                    self.m03 = (float)(double)reader.Value;
                    break;
                    case "m10":
                    self.m10 = (float)(double)reader.Value;
                    break;
                    case "m11":
                    self.m11 = (float)(double)reader.Value;
                    break;
                    case "m12":
                    self.m12 = (float)(double)reader.Value;
                    break;
                    case "m13":
                    self.m13 = (float)(double)reader.Value;
                    break;
                    case "m20":
                    self.m20 = (float)(double)reader.Value;
                    break;
                    case "m21":
                    self.m21 = (float)(double)reader.Value;
                    break;
                    case "m22":
                    self.m22 = (float)(double)reader.Value;
                    break;
                    case "m23":
                    self.m23 = (float)(double)reader.Value;
                    break;
                    case "m30":
                    self.m30 = (float)(double)reader.Value;
                    break;
                    case "m31":
                    self.m31 = (float)(double)reader.Value;
                    break;
                    case "m32":
                    self.m32 = (float)(double)reader.Value;
                    break;
                    case "m33":
                    self.m33 = (float)(double)reader.Value;
                    break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return self;
    }
    public static void WriteJson(this UnityEngine.Matrix4x4 self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WritePropertyName("m00");
        writer.WriteValue(self.m00);
        writer.WritePropertyName("m01");
        writer.WriteValue(self.m01);
        writer.WritePropertyName("m02");
        writer.WriteValue(self.m02);
        writer.WritePropertyName("m03");
        writer.WriteValue(self.m03);
        writer.WritePropertyName("m10");
        writer.WriteValue(self.m10);
        writer.WritePropertyName("m11");
        writer.WriteValue(self.m11);
        writer.WritePropertyName("m12");
        writer.WriteValue(self.m12);
        writer.WritePropertyName("m13");
        writer.WriteValue(self.m13);
        writer.WritePropertyName("m20");
        writer.WriteValue(self.m20);
        writer.WritePropertyName("m21");
        writer.WriteValue(self.m21);
        writer.WritePropertyName("m22");
        writer.WriteValue(self.m22);
        writer.WritePropertyName("m23");
        writer.WriteValue(self.m23);
        writer.WritePropertyName("m30");
        writer.WriteValue(self.m30);
        writer.WritePropertyName("m31");
        writer.WriteValue(self.m31);
        writer.WritePropertyName("m32");
        writer.WriteValue(self.m32);
        writer.WritePropertyName("m33");
        writer.WriteValue(self.m33);
        writer.WriteEndObject();
    }
    public static void UpdateFrom(this UnityEngine.AI.NavMeshPath self, UnityEngine.AI.NavMeshPath other, ZRUpdateFromHelper __helper) 
    {

    }
    public static void Deserialize(this UnityEngine.AI.NavMeshPath self, ZRBinaryReader reader) 
    {

    }
    public static void Serialize(this UnityEngine.AI.NavMeshPath self, ZRBinaryWriter writer) 
    {

    }
    public static ulong CalculateHash(this UnityEngine.AI.NavMeshPath self, ZRHashHelper __helper) 
    {
        System.UInt64 hash = 345093625;
        hash ^= (ulong)1983906321;
        hash += hash << 11; hash ^= hash >> 7;
        return hash;
    }
    public static void CompareCheck(this UnityEngine.AI.NavMeshPath self, UnityEngine.AI.NavMeshPath other, ZRCompareCheckHelper __helper, Action<string> printer) 
    {

    }
    public static bool ReadFromJson(this UnityEngine.AI.NavMeshPath self, ZRJsonTextReader reader) 
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var __name = (string) reader.Value;
                reader.Read();
                switch(__name)
                {
                    default: return false; break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) { break; }
        }
        return true;
    }
    public static void WriteJson(this UnityEngine.AI.NavMeshPath self, ZRJsonTextWriter writer) 
    {
        writer.WriteStartObject();
        writer.WriteEndObject();
    }
}
#endif
