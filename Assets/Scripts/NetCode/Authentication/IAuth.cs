using System.Threading.Tasks;

namespace NetCode.Authentication
{
    public interface IAuth
    {
        Task SignInAnonymouslyAsync();
    }
}