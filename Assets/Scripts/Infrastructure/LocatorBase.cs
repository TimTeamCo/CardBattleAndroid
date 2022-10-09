using System;
using System.Collections.Generic;
using TTAuth;

public class LocatorBase
{
    private Dictionary<Type, object> m_provided = new Dictionary<Type, object>();

    /// On construction, we can prepare default implementations of any services we expect to be required. This way, if for some reason the actual implementations
    /// are never Provided (e.g. for tests), nothing will break.
    public LocatorBase()
    {
        Provide(new Messenger());
        Provide(new UpdateSlowSample());
        Provide(new IdentitySample());
        Provide(new NGO.InGameInputHandler());

        FinishConstruction();
    }
    
    protected virtual void FinishConstruction() { }
    
    /// Call this to indicate that something is available for global access.
    private void ProvideAny<T>(T instance) where T : IProvidable<T>
    {
        Type type = typeof(T);
        if (m_provided.ContainsKey(type))
        {
            var previousProvision = (T)m_provided[type];
            instance.OnReProvided(previousProvision);
            m_provided.Remove(type);
        }

        m_provided.Add(type, instance);
    }
    
    /// If a T has previously been Provided, this will retrieve it. Else, null is returned.
    private T Locate<T>() where T : class
    {
        Type type = typeof(T);
        if (!m_provided.ContainsKey(type))
        {
            return null;
        }
        return m_provided[type] as T;
    }
    
    // To limit global access to only components that should have it, and to reduce programmer error, we'll declare explicit flavors of Provide and getters for them.
    public IMessenger Messenger => Locate<IMessenger>();
    public void Provide(IMessenger messenger) { ProvideAny(messenger); }
    
    public IUpdateSlow UpdateSlow => Locate<IUpdateSlow>();
    public void Provide(IUpdateSlow updateSlow) { ProvideAny(updateSlow); }

    public IIdentity Identity => Locate<IIdentity>();
    public void Provide(IIdentity identity) { ProvideAny(identity); }

    public NGO.IInGameInputHandler InGameInputHandler => Locate<NGO.IInGameInputHandler>();
    public void Provide(NGO.IInGameInputHandler inputHandler) { ProvideAny(inputHandler); }

    // As you add more Provided types, be sure their default implementations are included in the constructor.
}
