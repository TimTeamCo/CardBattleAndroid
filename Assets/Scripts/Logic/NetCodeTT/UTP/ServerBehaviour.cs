using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

using Unity.Networking.Transport;

namespace Logic.NetCodeTT.UTP
{
    public class ServerBehaviour : MonoBehaviour
    {
        public NetworkDriver m_Driver;
        private NativeList<NetworkConnection> m_Connections;

        private void Start()
        {
            m_Driver = NetworkDriver.Create();
            var endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;
            if (m_Driver.Bind(endpoint) != 0)
                Debug.Log("Failed to bind to port 9000");
            else
                m_Driver.Listen();

            m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
        }

        private void Update()
        {
            m_Driver.ScheduleUpdate().Complete();

            // Clean up connections
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (m_Connections[i].IsCreated) continue;

                m_Connections.RemoveAtSwapBack(i);
                --i;
            }

            // Accept new connections
            NetworkConnection c;
            while ((c = m_Driver.Accept()) != default(NetworkConnection))
            {
                m_Connections.Add(c);
                Debug.Log("Accepted a connection");
            }

            DataStreamReader stream;
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (m_Connections[i].IsCreated == false)
                    continue;

                NetworkEvent.Type cmd;
                while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        uint number = stream.ReadUInt();
                        Debug.Log("Got " + number + " from the Client adding + 2 to it.");
                        number += 2;

                        m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);
                        writer.WriteUInt(number);
                        m_Driver.EndSend(writer);
                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        Debug.Log("Client disconnected from server");
                        m_Connections[i] = default(NetworkConnection);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (m_Driver.IsCreated == false) return;

            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }
}
