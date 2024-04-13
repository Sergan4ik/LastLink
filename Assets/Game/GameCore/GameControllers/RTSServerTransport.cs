using System;
using System.Collections.Generic;
using System.IO;
using Game.GameCore;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Error;
using UnityEngine;

public class RTSServerTransport : MonoBehaviour
{
    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    
    Action<object,DisconnectType> userDisconnectedCallback;
    Action<object, long> userConnectedCallback;
    Dictionary<ushort, Action<object, Stream>> channelListeners = new Dictionary<ushort, Action<object, Stream>>();

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        var endpoint = NetworkEndpoint.AnyIpv4.WithPort(7777);
        if (m_Driver.Bind(endpoint) != 0)
        {
            Debug.LogError("Failed to bind to port 7777.");
            return;
        }

        Debug.Log($"Server initialized <color=green>successfully</color> on port 7777.");

        m_Driver.Listen();
    }

    void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections.
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        // Accept new connections.
        NetworkConnection c;
        while ((c = m_Driver.Accept(out var payload)) != default)
        {
            DataStreamReader reader = new DataStreamReader(payload);
            long playerID = reader.ReadLong();
            
            m_Connections.Add(c);
            userConnectedCallback.Invoke(c, playerID);
            Debug.Log($"Accepted a connection player with id {playerID}.");
        }
        
        for (int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    ushort channel = stream.ReadUShort();
                    UnityNetworkTools.ReadIncomingMessage(stream, m_Connections[i], channelListeners[channel]);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    var connectionReason = (DisconnectReason)stream.ReadByte();
                    userDisconnectedCallback.Invoke(m_Connections[i], DisconnectType.Manual);
                    m_Connections[i] = default;
                    Debug.Log($"Client disconnected from the server with {connectionReason}.");
                }
            }
        }
    }

    public void SendToChannel(object connectionhandler, ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        NetworkConnection con = (NetworkConnection)connectionhandler;
        m_Driver.SendMessageFromStream(channelid, con, writemessagetostreamcallback);
    }


    public void ListenToChannel(ushort channelid, Action<object, Stream> messagereceived)
    {
        channelListeners[channelid] = messagereceived;
    }

    public void BroadcastToChannel(ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        m_Driver.BroadcastMessageFromStream(channelid, m_Connections, writemessagetostreamcallback);
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