/// Something that wants to subscribe to messages from arbitrary, unknown senders.
public interface IReceiveMessages 
{
    void OnReceiveMessage(MessageType type, object msg);
}
