using NetHome.Common.Models;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IUserService
    {
        Task Login(LoginModel loginModel);
        Task Validate();
        Task Register(RegisterModel registerModel);
        UserModel GetUserData();
        void ClearUserData();
    }
}
