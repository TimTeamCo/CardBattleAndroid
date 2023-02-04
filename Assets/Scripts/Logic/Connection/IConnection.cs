
namespace Logic.Connection
{
    public interface IConnection
    {
        void Init();
        
        void HardCheckInternetConnection();
    }
}