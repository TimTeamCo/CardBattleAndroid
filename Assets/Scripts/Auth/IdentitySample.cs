using TTAuth;

public class IdentitySample : IIdentity
{
    public SubIdentity GetSubIdentity(Auth.IIdentityType identityType)
    {
        return null;
    }

    public void OnReProvided(IIdentity other)
    {
    }
}