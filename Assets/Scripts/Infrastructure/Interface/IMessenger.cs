/// Something to which IReceiveMessages can send/subscribe for arbitrary messages.
public interface IMessenger : IReceiveMessages, IProvidable<IMessenger>
{
    void Subscribe(IReceiveMessages receiver);
    void Unsubscribe(IReceiveMessages receiver);
}
