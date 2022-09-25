public interface IIdentity : IProvidable<IIdentity>
{
    SubIdentity GetSubIdentity(IIdentityType identityType);
}

public enum IIdentityType
{
    Local = 0,
    Auth,
}
