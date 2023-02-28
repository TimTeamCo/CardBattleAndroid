using Unity.Networking.Transport;
using UnityEngine;

namespace Logic.NetCodeTT.UTP
{
    public class ClientBehaviour : MonoBehaviour
    {
        public NetworkDriver m_Driver;
        public NetworkConnection m_Connection;
        public bool Done;

        private void Start()
        {
            m_Driver = NetworkDriver.Create();
            m_Connection = default(NetworkConnection);

            var endpoint = NetworkEndPoint.LoopbackIpv4;
            endpoint.Port = 9000;
            m_Connection = m_Driver.Connect(endpoint);
        }

        private void Update()
        {
            m_Driver.ScheduleUpdate().Complete();

            if (m_Connection.IsCreated == false)
            {
                if (Done) return;
                Debug.Log("Something went wrong during connect");
                return;
            }

            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("We are now connected to the server");

                    uint value = 1;
                    m_Driver.BeginSend(m_Connection, out var writer);
                    writer.WriteUInt(value);
                    m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    uint value = stream.ReadUInt();
                    Debug.Log("Got the value = " + value + " back from the server");
                    Done = true;
                    m_Connection.Disconnect(m_Driver);
                    m_Connection = default(NetworkConnection);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client got disconnected from server");
                    m_Connection = default(NetworkConnection);
                }
            }
        }

        public void OnDestroy()
        {
            m_Driver.Dispose();
        }
    }
}