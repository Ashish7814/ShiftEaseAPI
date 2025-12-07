using ShiftEaseAPI.Service.Account.DTOs;

namespace ShiftEaseAPI.Service.EmailTemplate
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
        Task<ResponseDto> ConfirmEmailAsync(string userId, string token);
    }
}
