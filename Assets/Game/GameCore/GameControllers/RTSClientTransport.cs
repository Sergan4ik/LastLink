using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Game.GameCore;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class RTSClientTransport : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    public bool isConnected;
    
    Dictionary<ushort, Action<Stream>> channelListeners = new Dictionary<ushort, Action<Stream>>();
    
    void Start()
    {
        m_Driver = NetworkDriver.Create();
    }

    public async Task ConnectToServer(long playerID)
    {
        var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        NativeArray<byte> payload = new NativeArray<byte>(sizeof(long), Allocator.Temp);
        
        DataStreamWriter writer = new DataStreamWriter(payload);
        writer.WriteLong(playerID);
        
        m_Connection = m_Driver.Connect(endpoint, payload);
        
        while(isConnected == false)
            await Task.Yield();
    }

    void OnDestroy()
    {
        m_Driver.Dispose();
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        Unity.Collections.DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                isConnected = true;
                Debug.Log("Connected to server.");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                ushort channel = stream.ReadUShort();
                var streamLength = stream.Length - sizeof(ushort);
                byte[] buf = new byte[streamLength];
                for (int i = 0; i < streamLength; ++i)
                {
                    buf[i] = stream.ReadByte();
                }
                
                if (channelListeners.ContainsKey(channel))
                {
                    channelListeners[channel].Invoke(new MemoryStream(buf));
                }
                else
                {
                    Debug.Log("Received data of unknown channel: " + channel);
                }
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
            }
        }
    }
    

    public void SendToChannel(ushort channelid, Action<Stream> writemessagetostreamcallback)
    {
        m_Driver.SendMessageFromStream(channelid, m_Connection, writemessagetostreamcallback);
    }

    public void ListenToChannel(ushort channelid, Action<Stream> messagereceived)
    {
        channelListeners[channelid] = messagereceived;
    }
}