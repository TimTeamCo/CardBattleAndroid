public class IdentitySample : IIdentity
{
    public SubIdentity GetSubIdentity(IIdentityType identityType)
    {
        return null;
    }

    public void OnReProvided(IIdentity other)
    {
    }
}