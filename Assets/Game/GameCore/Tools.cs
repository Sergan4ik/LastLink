using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace Game.GameCore
{
    public static class UnityNetworkTools
    {
        public static void SendMessageFromStream(this NetworkDriver driver, ushort channelid, NetworkConnection con, Action<Stream> streamWriter)
        {
            var stream = new MemoryStream();
            streamWriter(stream);
            
            driver.BeginSend(con, out var writer);
            writer.WriteUShort(channelid);

            for (var index = 0; index < stream.Length; index++)
            {
                var bt = stream.GetBuffer()[index];
                writer.WriteByte(bt);
            }
            
            driver.EndSend(writer);
        }
        
        public static void BroadcastMessageFromStream(this NetworkDriver driver, ushort channelid, IEnumerable<NetworkConnection> connections, Action<Stream> streamWriter)
        {
            var stream = new MemoryStream();
            streamWriter(stream);

            foreach (var networkConnection in connections)
            {
                driver.BeginSend(networkConnection, out var writer);
                writer.WriteUShort(channelid);

                var buf = stream.GetBuffer();
                for (var index = 0; index < stream.Length; index++)
                {
                    writer.WriteByte(buf[index]);
                }

                driver.EndSend(writer);
            }
        }
        
        public static void ReadIncomingMessage(DataStreamReader stream, NetworkConnection con, Action<object, Stream> readCallback)
        {
            var streamLength = stream.Length - sizeof(ushort);
            byte[] buf = new byte[streamLength];
            for (int j = 0; j < streamLength; ++j)
            {
                buf[j] = stream.ReadByte();
            }

            readCallback.Invoke(con, new MemoryStream(buf));
        }


    }

    public static class Tools
    {
        public static float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        
        public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
        {
            return Cross2D(b - a, c - a) / 2f;
        }

        public static Matrix4x4 GetUnitToViewportMatrix(Camera camera)
        {
            return camera.projectionMatrix * camera.transform.worldToLocalMatrix;
        }
        
        public static UnitView GetUnitView(this string unitId)
        {
            return Resources.Load<UnitView>($"UnitViews/{unitId}");
        }
        public static Sprite GetUnitIcon(this string unitId)
        {
            return Resources.Load<Sprite>($"UnitIcons/{unitId}");
        }
    }
}