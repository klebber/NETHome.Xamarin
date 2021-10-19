using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface ISignalRConnection
    {
        Task Connect();
        Task Disconnect();
        Task Switch(bool ison);
    }
}
