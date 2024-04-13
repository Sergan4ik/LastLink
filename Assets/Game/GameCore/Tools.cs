using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Error;
using UnityEngine;

namespace Game.GameCore
{
    public static class UnityNetworkTools
    {
        public static void SendMessageFromStream(this NetworkDriver driver, ushort channelid, NetworkConnection con, Action<Stream> streamWriter, NetworkPipeline pipeline)
        {
            var stream = new MemoryStream();
            streamWriter(stream);
            Debug.Log($"Sending message from stream on channel {channelid} to connection {con}, size = {stream.Length}.");

            DataStreamWriter writer;
            if (pipeline != default)
                driver.BeginSend(pipeline, con, out writer);
            else
                driver.BeginSend(con, out writer);
            
            writer.WriteUShort(channelid);

            for (var index = 0; index < stream.Length; index++)
            {
                var bt = stream.GetBuffer()[index];
                writer.WriteByte(bt);
            }
            
            int errorCode = driver.EndSend(writer);
            if (errorCode < 0)
            {
                Debug.LogError($"Failed to send message to connection {con}, error code: {(StatusCode)errorCode}");
            }
        }
        
        public static void BroadcastMessageFromStream(this NetworkDriver driver, ushort channelid, NativeList<NetworkConnection> connections, Action<Stream> streamWriter, NetworkPipeline pipeline)
        {
            var stream = new MemoryStream();
            streamWriter(stream);
            Debug.Log($"Broadcasting message from stream on channel {channelid} to {connections.Length} connections, size = {stream.Length}.");

            for (int i = 0; i < connections.Length; i++)
            {
                var networkConnection = connections[i];
                DataStreamWriter writer;
                
                if (pipeline != default)
                    driver.BeginSend(pipeline, networkConnection, out writer);
                else
                    driver.BeginSend(networkConnection, out writer);
                
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
            Debug.Log($"Reading incoming message from connection {con}, size = {streamLength}.");
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