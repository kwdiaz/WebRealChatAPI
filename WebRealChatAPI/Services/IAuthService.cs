using Microsoft.AspNetCore.Identity.Data;
using WebRealChatAPI.Models;

namespace WebRealChatAPI.Services
{
    public interface IAuthService
    {
        AuthResultModel Register(RegisterRequestModel request);
        AuthResultModel Login(LoginRequestModel request);
    }
}