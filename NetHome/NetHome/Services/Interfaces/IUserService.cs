using NetHome.Common;
using NetHome.Helpers;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IUserService
    {
        Task<RequestResult> Login(LoginRequest loginRequest);
        Task<RequestResult> Validate();
        Task<RequestResult> Register(RegisterRequest registerRequest);
        Task Logout();
    }
}
