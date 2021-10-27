using NetHome.Common;
using NetHome.Common.Models;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IUserService
    {
        Task Login(LoginRequest loginRequest);
        Task Validate();
        Task Register(RegisterRequest registerRequest);
        UserModel GetUserData();
        void ClearUserData();
    }
}
