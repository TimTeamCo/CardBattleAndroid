using Unity.Netcode;

namespace LobbyRelaySample.ngo
{
    public class PlayerData : INetworkSerializable
    {
        public string name;
        public ulong id;
        public int round;
        public int score;
        
        public PlayerData() { }

        public PlayerData(string name, ulong id, int round = 0, int score = 0)
        {
            this.name = name; 
            this.id = id; 
            this.round = round;
            this.score = score;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref round);
            serializer.SerializeValue(ref score);
        }
    }
}