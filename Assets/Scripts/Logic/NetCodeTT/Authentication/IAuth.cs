using System.Threading.Tasks;

namespace NetCodeTT.Authentication
{
    public interface IAuth
    {
        void Init();
        Task SignInAnonymouslyAsync();
        void SetupEvents();
    }
}