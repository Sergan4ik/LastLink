using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Game.GameCore;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;
using UnityEngine;

public class RTSClientTransport : MonoBehaviour
{
    NetworkDriver networkDriver;
    NetworkConnection connection;
    NetworkPipeline pipeline;

    public bool isConnected;
    
    Dictionary<ushort, Action<Stream>> channelListeners = new Dictionary<ushort, Action<Stream>>();
    
    public async Task ConnectToServer(long playerID)
    {
        NetworkSettings settings = new NetworkSettings();
        settings.WithFragmentationStageParameters(1024 * 10);
        
        networkDriver = NetworkDriver.Create(settings);

        pipeline = networkDriver.CreatePipeline(typeof(FragmentationPipelineStage),
            typeof(ReliableSequencedPipelineStage));
            
        var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        NativeArray<byte> payload = new NativeArray<byte>(sizeof(long), Allocator.Persistent);
        
        // DataStreamWriter writer = new DataStreamWriter(payload);
        // writer.WriteLong(playerID);
        // writer.Flush();
        //
        // m_Connection = m_Driver.Connect(endpoint, payload);
        
        connection = networkDriver.Connect(endpoint);  
        
        while(isConnected == false)
            await Task.Yield();
    }

    void OnDestroy()
    {
        networkDriver.Dispose();
    }

    void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        if (!connection.IsCreated)
        {
            return;
        }

        Unity.Collections.DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(networkDriver, out stream, out var receivedPipeline)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                isConnected = true;
                Debug.Log("Connected to server.");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                if (receivedPipeline != default)
                {
                    Debug.Log($"Received message with pipeline {receivedPipeline}");
                }
                ushort channel = stream.ReadUShort();
                if (channelListeners.TryGetValue(channel, out var listener))
                {
                    UnityNetworkTools.ReadIncomingMessage(stream, connection, (o, st) => listener(st));
                }
                else
                {
                    Debug.Log($"Received message on channel {channel} but no listener was found.");
                }
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                connection = default;
            }
        }
    }
    

    public void SendToChannel(ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        networkDriver.SendMessageFromStream(channelid, connection, writemessagetostreamcallback, NetworkPipeline.Null);
    }

    public void ListenToChannel(ushort channelid, Action<Stream> messagereceived)
    {
        channelListeners[channelid] = messagereceived;
    }
}