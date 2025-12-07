using ShiftEaseAPI.Service.Account.DTOs;

namespace ShiftEaseAPI.Service.Account.User
{
    public interface IUserService
    {
        Task<ResponseDto> RegisterAsync(RegisterDto model);
        Task<LoginResponseDto> LoginAsync(LoginDto model);
        Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<ResponseDto> ResetPasswordAsync(string userId, string encodedToken);
    }
}
