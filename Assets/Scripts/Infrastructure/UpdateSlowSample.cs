public class UpdateSlowSample : IUpdateSlow
{
    public void OnReProvided(IUpdateSlow previousProvider)
    {
    }

    public void OnUpdate(float dt)
    {
    }

    public void Subscribe(UpdateMethod onUpdate, float period)
    {
    }

    public void Unsubscribe(UpdateMethod onUpdate)
    {
    }
}