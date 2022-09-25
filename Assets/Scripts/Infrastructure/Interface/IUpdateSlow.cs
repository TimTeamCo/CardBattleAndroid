public interface IUpdateSlow : IProvidable<IUpdateSlow>
{
    void OnUpdate(float dt);
    void Subscribe(UpdateMethod onUpdate, float period);
    void Unsubscribe(UpdateMethod onUpdate);
}

public delegate void UpdateMethod(float dt);
