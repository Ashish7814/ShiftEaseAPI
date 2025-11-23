using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interface
{
    public interface IAuthService
    {
        Task<Response> RegisterAsync(RegisterModel model);
        Task<LoginResponseDto?> LoginAsync(LoginModel model);
        Task<Response> ForgotPasswordAsync(ForgotPasswordModel model, string frontendResetUrlBase);
        Task<Response> ResetPasswordAsync(ResetPasswordModel model);
    }
}
