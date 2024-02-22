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
}
#endif
