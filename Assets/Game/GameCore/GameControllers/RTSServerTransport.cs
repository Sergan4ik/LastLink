using System;
using System.Collections.Generic;
using System.IO;
using Game.GameCore;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Error;
using Unity.Networking.Transport.Utilities;
using UnityEngine;

public class RTSServerTransport : MonoBehaviour
{
    NetworkDriver networkDriver;
    NativeList<NetworkConnection> connections;
    NetworkPipeline pipeline;
    
    Action<object,DisconnectType> userDisconnectedCallback;
    Action<object, long> userConnectedCallback;
    Dictionary<ushort, Action<object, Stream>> channelListeners = new Dictionary<ushort, Action<object, Stream>>();

    public void InitTransport()
    {
        NetworkSettings settings = new NetworkSettings();
        settings.WithFragmentationStageParameters(1024 * 10);
        networkDriver = NetworkDriver.Create(settings);

        pipeline = networkDriver.CreatePipeline(typeof(FragmentationPipelineStage),
            typeof(ReliableSequencedPipelineStage));
        
        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        var endpoint = NetworkEndpoint.AnyIpv4.WithPort(7777);
        if (networkDriver.Bind(endpoint) != 0)
        {
            Debug.LogError("Failed to bind to port 7777.");
            return;
        }

        Debug.Log($"Server initialized <color=green>successfully</color> on port 7777.");

        networkDriver.Listen();
    }

    void OnDestroy()
    {
        if (networkDriver.IsCreated)
        {
            networkDriver.Dispose();
            connections.Dispose();
        }
    }

    void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        // Clean up connections.
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        // Accept new connections.
        NetworkConnection c;
        while ((c = networkDriver.Accept())!= default)
        {
            // DataStreamReader reader = new DataStreamReader(payload);
            long playerID = 1;
            
            connections.Add(c);
            userConnectedCallback.Invoke(c, playerID);
            Debug.Log($"Accepted a connection player with id {playerID}.");
        }
        
        for (int i = 0; i < connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = networkDriver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    ushort channel = stream.ReadUShort();
                    
                    if (channelListeners.TryGetValue(channel, out var listener))
                    {
                        UnityNetworkTools.ReadIncomingMessage(stream, connections[i], listener);
                    }
                    else
                    {
                        Debug.Log($"Received message on channel {channel} but no listener was found.");
                    }
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    var connectionReason = (DisconnectReason)stream.ReadByte();
                    userDisconnectedCallback.Invoke(connections[i], DisconnectType.Manual);
                    connections[i] = default;
                    Debug.Log($"Client disconnected from the server with {connectionReason}.");
                }
            }
        }
    }

    public void SendToChannel(object connectionhandler, ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        NetworkConnection con = (NetworkConnection)connectionhandler;
        networkDriver.SendMessageFromStream(channelid, con, writemessagetostreamcallback,
            channelid == NetworkChannels.BattleReceiveModelChannel ? pipeline : NetworkPipeline.Null);
    }


    public void ListenToChannel(ushort channelid, Action<object, Stream> messagereceived)
    {
        channelListeners[channelid] = messagereceived;
    }

    public void BroadcastToChannel(ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        networkDriver.BroadcastMessageFromStream(channelid, connections, writemessagetostreamcallback,
            channelid == NetworkChannels.BattleReceiveModelChannel ? pipeline : NetworkPipeline.Null);
    }

    public void OnUserConnected(Action<object, long> userconnectedcallback)
    {
        userConnectedCallback = userconnectedcallback;
    }

    public void OnUserDisconnected(Action<object, DisconnectType> userdisconnectedcallback)
    {
        userDisconnectedCallback = userdisconnectedcallback;
    }
}