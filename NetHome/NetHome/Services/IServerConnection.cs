using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IServerConnection
    {
        Task<bool> Connect();
        Task Disconnect();
        Task Switch(bool ison);
    }
}
