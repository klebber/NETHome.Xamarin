using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IWebSocketService
    {
        Task ConnectAsync();
        Task CloseAsync();
    }
}
