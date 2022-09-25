using System;
using System.Collections.Generic;

/// Our internal representation of the local player's credentials, wrapping the data required for interfacing with the identities of that player in the services.
/// (In use here, it just wraps Auth, but it can be used to combine multiple sets of credentials into one concept of a player.)
public class Identity : IIdentity, IDisposable
{
    private Dictionary<IIdentityType, SubIdentity> m_subIdentities = new Dictionary<IIdentityType, SubIdentity>();

    public Identity(Action callbackOnAuthLogin)
    {
        m_subIdentities.Add(IIdentityType.Local, new SubIdentity());
        m_subIdentities.Add(IIdentityType.Auth, new SubIdentity_Authentication(callbackOnAuthLogin));
    }
    
    public void OnReProvided(IIdentity previousProvider)
    {
        if (previousProvider is Identity)
        {
            Identity prevIdentity = previousProvider as Identity;
            foreach (var entry in prevIdentity.m_subIdentities)
            {
                m_subIdentities.Add(entry.Key, entry.Value);
            }
        }
    }

    public SubIdentity GetSubIdentity(IIdentityType identityType)
    {
        return m_subIdentities[identityType];
    }

    public void Dispose()
    {
        foreach (var sub in m_subIdentities)
        {
            if (sub.Value is IDisposable)
            {
                (sub.Value as IDisposable).Dispose();
            }
        }
    }
}
