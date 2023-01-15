using System.Threading.Tasks;

namespace NetCodeTT.Authentication
{
    public interface IAuth
    {
        Task SignInAnonymouslyAsync();
    }
}