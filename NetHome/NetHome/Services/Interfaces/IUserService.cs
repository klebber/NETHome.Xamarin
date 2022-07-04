using NetHome.Common;
using NetHome.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IUserService
    {
        Task<RequestResult> Login(LoginRequest loginRequest);
        Task<RequestResult> Validate();
        Task<RequestResult> Register(RegisterRequest registerRequest);
        Task<RequestResult> Update(UserModel user);
        Task<RequestResultPayload<UserModel>> GetUser(string id);
        Task<RequestResultPayload<ICollection<UserModel>>> GetAllUsers();
        Task Logout();
        Task<RequestResultPayload<ICollection<DeviceModel>>> GetAccessibleDevices(string userId);
        Task<RequestResult> AddDeviceAccess(DeviceAccessPayload dap);
        Task<RequestResult> RemoveDeviceAccess(DeviceAccessPayload dap);
    }
}
