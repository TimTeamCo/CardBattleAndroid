namespace TTAuth
{
    public interface IIdentity : IProvidable<IIdentity>
    {
        SubIdentity GetSubIdentity(Auth.IIdentityType identityType);
    }
}
